using Plotnikov_PR_21_102_DocumentManager.Entity;
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

namespace Plotnikov_PR_21_102_DocumentManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для Workers.xaml
    /// </summary>
    public partial class Workers : Window
    {
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
                cbPost.ItemsSource = db.posts.ToList();
                cbRole.ItemsSource = db.roles.ToList();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
