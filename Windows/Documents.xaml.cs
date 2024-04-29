using Plotnikov_PR_21_102_DocumentManager.Entity;
using Plotnikov_PR_21_102_DocumentManager.SpecialModules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Plotnikov_PR_21_102_DocumentManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для Documents.xaml
    /// </summary>
    public partial class Documents : Window
    {
        private bool read_only;
        private documents current_document;

        public Documents(bool read_only)
        {
            InitializeComponent();
            new TextResizer(this);
            this.read_only = read_only;

            if (read_only)
            {
                btnInsert.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                tbName.IsReadOnly = true;
                rtbDescr.IsReadOnly = true;

                MessageBox.Show(
                    "Список документов доступен только для чтения!", 
                    "Ограничение", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
            }
            else
            {
                btnInsert.Click += BtnInsert_Click;
                btnUpdate.Click += BtnUpdate_Click;
                btnDelete.Click += BtnDelete_Click;
            }

            lbDocuments.SelectionChanged += LbDocuments_SelectionChanged;
            cbProject.SelectionChanged += CbProject_SelectionChanged;
            
            using (var db = new Entities1())
            {
                cbType.ItemsSource = db.document_types.OrderBy(x => x.id_document_type).ToList();
                cbProject.ItemsSource = db.project_workers.Where(x => x.id_worker == UserSession.worker.id_worker)
                    .Include("projects").ToList();
                if (cbProject.Items.Count > 0)
                    cbProject.SelectedIndex = 0;
            }
        }

        private void CbProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProject.SelectedIndex == -1)
                return;

            var id_project = ((project_workers)cbProject.SelectedItem).id_project;
            using (var db = new Entities1())
            {
                lbDocuments.ItemsSource = db.documents.Where(x => x.id_project == id_project).Include("document_types").ToList();
            }
        }

        private void LbDocuments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            current_document = (documents)lbDocuments.SelectedItem;

            if (current_document == null)
            {
                tbName.Text = "";
                RichTextBoxExtensions.SetText(rtbDescr, "");
                cbType.SelectedIndex = 0;
                return;
            }
                
            tbName.Text = current_document.name;
            RichTextBoxExtensions.SetText(rtbDescr, current_document.description);
            cbType.SelectedIndex = current_document.id_document_type - 1;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (current_document == null)
            {
                Alert.Error("Не выбран документ!", MessageBoxImage.Exclamation); return;
            }

            if (!Alert.ConfirmAction("Вы действительно хотите УДАЛИТЬ документ?"))
                return;

            try
            {
                using (var db = new Entities1())
                {
                    var d = db.documents.Where(x => x.id_document == current_document.id_document).First();
                    db.documents.Remove(d);
                    db.SaveChanges();
                    CbProject_SelectionChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Alert.Error("Невозможно выполнить операцию, нарушение целостности данных: \n\n" + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (current_document == null)
            {
                Alert.Error("Не выбран документ!", MessageBoxImage.Exclamation); return;
            }

            if (tbName.Text.Length < 2)
            {
                Alert.Error("Имя документа не может быть пустым"); return;
            }

            if (RichTextBoxExtensions.GetText(rtbDescr).Length < 2 && 
                Alert.ConfirmAction("Вы не ввели описание. Хотите ввести описание документа перед сохранением?"))
                return;

            if (!Alert.ConfirmAction("Вы действительно хотите ОБНОВИТЬ существующий документ?"))
                return;

            try
            {
                using (var db = new Entities1())
                {
                    var d = db.documents.Where(x => x.id_document == current_document.id_document).First();
                    
                    d.name = tbName.Text;
                    d.description = RichTextBoxExtensions.GetText(rtbDescr);
                    d.id_document_type = cbType.SelectedIndex + 1;

                    db.SaveChanges();
                    CbProject_SelectionChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Alert.Error("Невозможно выполнить операцию, нарушение целостности данных: \n\n" + ex.Message);
            }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text.Length < 2)
            {
                Alert.Error("Имя документа не может быть пустым"); return;
            }

            if (RichTextBoxExtensions.GetText(rtbDescr).Length < 2 &&
                Alert.ConfirmAction("Вы не ввели описание. Хотите ввести описание документа перед сохранением?"))
                return;

            if (!Alert.ConfirmAction("Вы действительно хотите добавить НОВЫЙ документ?"))
                return;

            try
            {
                using (var db = new Entities1())
                {
                    var d = new documents();

                    d.name = tbName.Text;
                    d.description = RichTextBoxExtensions.GetText(rtbDescr);
                    d.id_document_type = cbType.SelectedIndex + 1;
                    d.id_project = ((project_workers)cbProject.SelectedItem).id_project;

                    db.documents.Add(d);

                    db.SaveChanges();
                    CbProject_SelectionChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Alert.Error("Невозможно выполнить операцию, нарушение целостности данных: \n\n" + ex.Message);
            }
        }
    }
}
