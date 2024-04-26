using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Plotnikov_PR_21_102_DocumentManager
{
    /// <summary>
    /// Умное масштабирование текста, учитывает изначальные размеры и делает это максимально правильно и грамотно
    /// Текст не уедет куда не надо
    /// В окне просто создать (прописать) new TextResizer(this, Максимальный множитель увеличения текста);
    /// </summary>
    internal class TextResizer
    {
        private readonly Window window;
        private readonly Dictionary<Control, double> originalFontSizes = new Dictionary<Control, double>();
        private float maxSize;
        private float scale = 1;
        private double startWidth, startHeight;

        public TextResizer(Window window, float maxSize = 4)
        {
            this.window = window;
            this.maxSize = maxSize;

            startWidth = window.Width;
            startHeight = window.Height;

            window.SizeChanged += (x, ъ) => ResizeTextElements();
            ResizeTextElements();
        }

        private void ResizeTextElements()
        {
            scale = CalculateNewTextScale();
            foreach (var element in FindVisualChildren<Control>(window))
            {
                if (!originalFontSizes.ContainsKey(element))
                {
                    originalFontSizes[element] = element.FontSize;
                }

                element.FontSize = originalFontSizes[element] * scale;
            }
        }

        private float CalculateNewTextScale()
        {
            return Math.Min(maxSize, (float)Math.Min(window.ActualHeight / startHeight, window.ActualWidth / startWidth));
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T foundChild)
                    {
                        yield return foundChild;
                    }

                    foreach (T nestedChild in FindVisualChildren<T>(child))
                    {
                        yield return nestedChild;
                    }
                }
            }
        }
    }

}
