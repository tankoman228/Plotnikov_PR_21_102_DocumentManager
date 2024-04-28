using Plotnikov_PR_21_102_DocumentManager.SpecialModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Plotnikov_PR_21_102_DocumentManager.Entity;

namespace Plotnikov_PR_21_102_DocumentManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для TestCasesForTesters.xaml
    /// </summary>
    public partial class TestCasesForTesters : Window
    {
        public TestCasesForTesters()
        {
            InitializeComponent();

            lbTestCases.SelectionChanged += LbTestCases_SelectionChanged;
            btnNew.Click += BtnNew_Click;
            btnNext.Click += BtnNext_Click;
            btnPrevious.Click += BtnPrevious_Click;
            btnSave.Click += BtnSave_Click;
            btnDelete.Click += BtnDelete_Click;
            new TextResizer(this, 1.3f);

            using (var db = new Entities1())
            {
                lbTestCases.ItemsSource = db.test_cases.Include("projects").ToList();
            }
        }


        private test_case_results last_selected_tc_result;
        private void UpdateInterface(test_cases test_case, test_case_results test_case_result)
        {
            //Задание и проверка значений для обновления интерфейса
            if (test_case == null)
            {
                if (test_case_result != null)
                {
                    test_case = test_case_result.test_cases;
                }
                else
                {
                    Alert.Error("Не выбран тест-кейс!", MessageBoxImage.Information);
                    return;
                }
            }

            cbSuccess.IsChecked = true;

            //Заполнение результатов тестирования
            if (test_case_result == null)
            {
                RichTextBoxExtensions.SetText(rtbErrorDescr, "");
                RichTextBoxExtensions.SetText(rtbRealResult, "");
                RichTextBoxExtensions.SetText(rtbReproduce, "");
                lDate.Text = "";
                btnDelete.IsEnabled = false;
            }
            else
            {
                RichTextBoxExtensions.SetText(rtbErrorDescr, test_case_result.bug_report_description);
                RichTextBoxExtensions.SetText(rtbRealResult, test_case_result.real_result);
                RichTextBoxExtensions.SetText(rtbReproduce, test_case_result.bug_report_reproducibility);
                lDate.Text = test_case_result.when_datetime.ToString();
                btnDelete.IsEnabled = true;
                last_selected_tc_result = test_case_result;
                cbSuccess.IsChecked = !test_case_result.contains_bug_report;
            }

            //Заполнение остальной таблицы
            tbID.Text = test_case.id_in_project;
            tbName.Text = test_case.name;
            tbPriority.Text = test_case.priority.ToString();
            RichTextBoxExtensions.SetText(rtbPrecondition, test_case.preconditions);
            RichTextBoxExtensions.SetText(rtbEstimated, test_case.estimated_result);
            RichTextBoxExtensions.SetText(rtbPostCondition, test_case.postcondition);
            lbSteps.ItemsSource = test_case.steps.Split('\n');
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (last_selected_tc_result == null ||
                !Alert.ConfirmAction("Вы действительно хотите удалить данные о проведённом тестировании?"))
                return;

            using (var db = new Entities1())
            {
                var r = db.test_case_results.
                    Where(x => x.id_test_case_result == last_selected_tc_result.id_test_case_result).
                    First();
                db.test_case_results.Remove(r);
                db.SaveChanges();
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Проверка заполненности полей
            if (cbSuccess.IsChecked == false && RichTextBoxExtensions.IsEmpty(rtbErrorDescr))
            {
                Alert.Error("Ошибка не описана!"); return;
            }
            if (cbSuccess.IsChecked == false && RichTextBoxExtensions.IsEmpty(rtbReproduce))
            {
                Alert.Error("Не описана воспроизводимость ошибки!"); return;
            }
            if (RichTextBoxExtensions.IsEmpty(rtbRealResult))
            {
                Alert.Error("Не описан результат теста!"); return;
            }


            //INSERT или UPDATE?
            if (last_selected_tc_result == null)
            { //INSERT
                using (var db = new Entities1())
                {
                    var res = new test_case_results();
                    res.contains_bug_report = cbSuccess.IsChecked == false;
                    res.real_result = RichTextBoxExtensions.GetText(rtbRealResult);
                    if (cbSuccess.IsChecked == false)
                    {
                        res.bug_report_description = RichTextBoxExtensions.GetText(rtbErrorDescr);
                        res.bug_report_reproducibility = RichTextBoxExtensions.GetText(rtbReproduce);
                    }
                    res.id_test_case = ((test_cases)lbTestCases.SelectedItem).id_test_case;
                    res.id_tester = UserSession.worker.id_worker;
                    res.when_datetime = DateTime.Now;

                    db.test_case_results.Add(res);
                    db.SaveChanges();
                    Alert.Success("Успешно добавлен результат");
                }

            } 
            else
            {   //UPDATE
                using (var db = new Entities1())
                {
                    var res = db.test_case_results.
                        Where(x => x.id_test_case_result == last_selected_tc_result.id_test_case_result).
                        First();
                    res.contains_bug_report = cbSuccess.IsChecked == false;
                    res.real_result = RichTextBoxExtensions.GetText(rtbRealResult);
                    if (cbSuccess.IsChecked == false)
                    {
                        res.bug_report_description = RichTextBoxExtensions.GetText(rtbErrorDescr);
                        res.bug_report_reproducibility = RichTextBoxExtensions.GetText(rtbReproduce);
                    }
                    else
                    {
                        res.bug_report_description = null;
                        res.bug_report_reproducibility = null;
                    }
                    db.SaveChanges();
                    Alert.Success("Успешно обновлён результат");
                }
            }
        }


        private int index_res = 0;
        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (lbTestCases.SelectedItem == null)
                return;
            var tc = (test_cases)lbTestCases.SelectedItem;

            index_res--;
            using (var db = new Entities1())
            {
                var res = db.test_case_results.
                    Where(x => x.id_test_case == tc.id_test_case).
                    Where(x => x.id_tester == UserSession.worker.id_worker).ToArray();
                if (res.Length == 0)
                    return;

                if (index_res < 0 || index_res >= res.Length)
                    index_res = res.Length - 1;

                UpdateInterface(tc, res[index_res]);
            }
        }
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (lbTestCases.SelectedItem == null)
                return;
            var tc = (test_cases)lbTestCases.SelectedItem;

            index_res++;
            using (var db = new Entities1())
            {
                var res = db.test_case_results.
                    Where(x => x.id_test_case == tc.id_test_case).
                    Where(x => x.id_tester == UserSession.worker.id_worker).ToArray();
                if (res.Length == 0)
                    return;

                if (index_res < 0 || index_res >= res.Length)
                    index_res = 0;

                UpdateInterface(tc, res[index_res]);
            }
        }


        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            last_selected_tc_result = null;
            btnDelete.IsEnabled = false;

            UpdateInterface((test_cases)lbTestCases.SelectedItem, null);
            lDate.Text = DateTime.Now.ToString();
        }

        private void LbTestCases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbTestCases.SelectedItem != null)
            {
                UpdateInterface((test_cases)lbTestCases.SelectedItem, null);
            }
        }
    }
}
