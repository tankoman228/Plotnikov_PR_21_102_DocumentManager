using Plotnikov_PR_21_102_DocumentManager.Entity;
using Plotnikov_PR_21_102_DocumentManager.SpecialModules;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Plotnikov_PR_21_102_DocumentManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для Workers.xaml
    /// </summary>
    public partial class Workers : Window
    {
        workers worker;

        public Workers()
        {
            InitializeComponent();
            new TextResizer(this);

            btnInsert.Click += BtnInsert_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;

            using (var db = new Entities1())
            {
                lbDocuments.ItemsSource = db.workers.Include("posts").ToList();
                cbPost.ItemsSource = db.posts.OrderBy(x => x.id_post).ToList();
                cbRole.ItemsSource = db.roles.OrderBy(x => x.id_role).ToList();
            }

            lbDocuments.SelectionChanged += LbDocuments_SelectionChanged;
        }

        private void LbDocuments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                using (var db = new Entities1())
                {
                    lbDocuments.ItemsSource = db.workers.Include("posts").ToList();
                }
            }

            worker = (workers)lbDocuments.SelectedItem;
            if (worker != null)
            {
                if (worker.lastname == null)
                    tbName.Text = $"{worker.name} {worker.sirname}";
                else tbName.Text = $"{worker.name} {worker.sirname} {worker.lastname}";

                tbMail.Text = worker.email;
                tbPhone.Text = worker.phone;
                tbContract.Text = worker.contract;
                tbSalary.Text = worker.approximate_salary.ToString();

                cbPost.SelectedIndex = worker.id_post - 1;
                cbRole.SelectedIndex = worker.id_role - 1;
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (worker == null)
            {
                Alert.Error("Работник не выбран"); return;
            }
            if (!Alert.ConfirmAction($"Вы уверены, что хотите удалить {worker.name} {worker.email}?"))
                return;

            try
            {
                using (var db = new Entities1())
                {
                    var w = db.workers.Where(x => x.id_worker == worker.id_worker).First();
                    db.workers.Remove(w);
                    db.SaveChanges();

                    Alert.Success("Работник успешно удалён");
                    LbDocuments_SelectionChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Alert.Error($"Невозможно удалить пользователя {worker.email}, возможно, имеются связанные с пользователем записи" +
                    $". Советуется удалить эти записи перед удалением, чтобы не нарушалась целостность данных в " +
                    $"базе. \n\n {ex.Message}");
            }
        }


        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (worker == null)
            {
                Alert.Error("Работник не выбран"); return;
            }
            if (!Alert.ConfirmAction($"Вы уверены, что хотите изменить данные для {worker.name} {worker.email}?"))
                return;

            try
            {
                using (var db = new Entities1())
                {
                    var w = db.workers.Where(x => x.id_worker == worker.id_worker).First();

                    var fname = tbName.Text.Split(' ');
                    if (fname.Length != 2 && fname.Length != 3)
                    {
                        Alert.Error("Неверно введённое ФИО"); return;
                    }

                    w.name = fname[0];
                    w.sirname = fname[1];
                    if (fname.Length == 3)
                        w.lastname = fname[2];

                    // Проверка и сохранение почты
                    if (Regex.IsMatch(tbMail.Text, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                    {
                        w.email = tbMail.Text;
                    }
                    else
                    {
                        Alert.Error("Неверный формат почты!");
                        return;
                    }

                    // Сохранение номера телефона
                    if (!Regex.IsMatch(tbPhone.Text, @"\+\d\d\d\d\d\d\d\d\d\d\d\z"))
                    {
                        Alert.Error("Неверный формат номера телефона!");
                        return;
                    }
                    w.phone = tbPhone.Text;

                    try
                    {
                        w.approximate_salary = decimal.Parse(tbSalary.Text);
                    }
                    catch { Alert.Error("Неверно введённые данные о зарплате! Введите целое или дробное число"); return; }

                    w.contract = tbContract.Text;

                    if (pbPassword.Password.Length > 0 &&
                        Alert.ConfirmAction("Вы действительно хотите сменить пароль пользователя?"))
                    {
                        w.password = PasswordHash__PLUG.getHash(pbPassword.Password);
                    }
                    w.id_post = cbPost.SelectedIndex + 1;
                    w.id_role = cbRole.SelectedIndex + 1;

                    db.SaveChanges();
                    LbDocuments_SelectionChanged(null, null);
                    Alert.Success("Работник обновлён");
                }
            }
            catch (Exception ex)
            {
                Alert.Error($"Невозможно изменить пользователя {worker.email}, возможно, дублируется email" +
                    $". \n\n {ex.Message}");
            }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (!Alert.ConfirmAction($"Вы уверены, что хотите добавить нового пользователя?"))
                return;

            if (cbPost.SelectedItem == null || cbRole.SelectedItem == null)
            {
                Alert.Error("Не указана должность и/или роль"); return;
            }

            try
            {
                using (var db = new Entities1())
                {
                    var w = new workers();

                    var fname = tbName.Text.Split(' ');
                    if (fname.Length != 2 && fname.Length != 3)
                    {
                        Alert.Error("Неверно введённое ФИО"); return;
                    }

                    w.name = fname[0];
                    w.sirname = fname[1];
                    if (fname.Length == 3)
                        w.lastname = fname[2];

                    // Проверка и сохранение почты
                    if (Regex.IsMatch(tbMail.Text, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                    {
                        w.email = tbMail.Text;
                    }
                    else
                    {
                        Alert.Error("Неверный формат почты!");
                        return;
                    }

                    // Сохранение номера телефона
                    if (!Regex.IsMatch(tbPhone.Text, @"\+\d\d\d\d\d\d\d\d\d\d\d\z"))
                    {
                        Alert.Error("Неверный формат номера телефона!");
                        return;
                    }
                    w.phone = tbPhone.Text;


                    try
                    {
                        w.approximate_salary = decimal.Parse(tbSalary.Text);
                    }
                    catch { Alert.Error("Неверно введённые данные о зарплате! Введите целое или дробное число"); return; }

                    w.contract = tbContract.Text;

                    if (pbPassword.Password.Length == 0)
                    {
                        Alert.Error("Не указан пароль для пользователя"); return;
                    }
                    else
                        w.password = PasswordHash__PLUG.getHash(pbPassword.Password);

                    w.id_post = cbPost.SelectedIndex + 1;
                    w.id_role = cbRole.SelectedIndex + 1;

                    db.workers.Add(w);

                    db.SaveChanges();
                    LbDocuments_SelectionChanged(null, null);
                    Alert.Success("Добавлен новый работник");
                }
            }
            catch (Exception ex)
            {
                Alert.Error($"Невозможно добавить пользователя {worker.email}, возможно, дублируется email" +
                    $". \n\n {ex.Message}");
            }
        }
    }
}
