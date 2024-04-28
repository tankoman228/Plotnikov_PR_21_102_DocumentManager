using System.Windows.Controls;
using System.Windows.Documents;

namespace Plotnikov_PR_21_102_DocumentManager.SpecialModules
{
    /// <summary>
    /// Работа с содержимым RichTextBox как со строками
    /// </summary>
    public static class RichTextBoxExtensions
    {
        public static string GetText(RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return textRange.Text;
        }

        public static void SetText(RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        public static bool IsEmpty(RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return string.IsNullOrWhiteSpace(textRange.Text);
        }
    }

}
