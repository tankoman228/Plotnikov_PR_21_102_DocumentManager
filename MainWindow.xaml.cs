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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Plotnikov_PR_21_102_DocumentManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            new SpecialModules.TextResizer(this, 4);

            btnLogin.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text.Length == 0) {
                SpecialModules.Alert.Error("Введите имя пользователя!", MessageBoxImage.Exclamation); return;
            }
            if (tbPassword.Password.Length == 0)
            {
                SpecialModules.Alert.Error("Введите пароль!", MessageBoxImage.Exclamation); return;
            }
            MessageBox.Show("hehe");
        }
    }
}
