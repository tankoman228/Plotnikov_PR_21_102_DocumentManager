﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Plotnikov_PR_21_102_DocumentManager.SpecialModules
{
    /// <summary>
    /// Вывод сообщений во всплывающие окна MessageBox
    /// </summary>
    internal class Alert
    {
        public static void Error(string message, MessageBoxImage image = MessageBoxImage.Error)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, image);
        }

        public static void Success(string message, MessageBoxImage image = MessageBoxImage.Information)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK, image);
        }

        public static bool ConfirmAction(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }
}
