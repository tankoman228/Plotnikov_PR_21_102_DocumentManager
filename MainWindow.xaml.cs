using Plotnikov_PR_21_102_DocumentManager.Entity;
using Plotnikov_PR_21_102_DocumentManager.SpecialModules;
using Plotnikov_PR_21_102_DocumentManager.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private int bad_attempts = 0; //Скольуо неудачных попыток авторизации выполнено

        public MainWindow()
        {
            InitializeComponent();

            new TextResizer(this, 4);

            btnLogin.Click += BtnLogin_Click;
        }

        //При открытии ругает пользоавтеля. Вызывать в цикле, чтобы ещё сильнее насолить хакеру
        public MainWindow(object block)
        {
            InitializeComponent();
            new Thread(() =>
            {
                Alert.Error("Вы заблокированы за попытку взлома!");
            }).Start();
            tbUsername.Text = "ЗАБЛОКИРОВАННОЕ УСТРОЙСТВО";
        }

        //Авторизация
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //Проверка заполненности
            if (tbUsername.Text.Length == 0) {
                Alert.Error("Введите имя пользователя!", MessageBoxImage.Exclamation); return;
            }
            if (tbPassword.Password.Length == 0)
            {
                Alert.Error("Введите пароль!", MessageBoxImage.Exclamation); return;
            }
            
            //Проверка, верны ли данные
            using (var db = new Entities1())
            {
                var acc = db.workers.Where(x => x.email.Equals(tbUsername.Text)).FirstOrDefault();

                if (acc == null)
                {
                    Alert.Error("Неизвестный пользователь");
                    return;
                }

                if (!PasswordHash__PLUG.isPasswordHash(tbPassword.Password, acc.password))
                {
                    bad_attempts++;
                    Alert.Error($"Неверный пароль, попытка {bad_attempts}/4");

                    if (bad_attempts >= 4)
                    {
                        Alert.Error($"Попытка взлома обнаружена! Ваше устройство будет заблокировано");
                        Alert.Error($"Подтвердите, что вы согласны с наказанием за хаккерскую атаку", MessageBoxImage.Hand);
                        while (true)
                        {
                            new MainWindow(null).Show();
                        }
                    }

                    return;
                }

                UserSession.worker = acc;
                // Переход куда нужно после окончания авторизации
                switch (acc.roles.id_role)
                {
                    case 1: new Documents(true).Show(); break;
                    case 2: new Documents(false).Show(); break;
                    case 3: new ComingSoon().Show(); break;
                    case 4: new Workers().Show(); break;
                    case 5: new TestCasesForTesters().Show(); break;

                    default:
                        Alert.Error("Unknown role");
                        return;
                }
                Close();
            }
        }
    }
}
