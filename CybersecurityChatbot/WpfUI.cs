using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace CybersecurityChatbot
{
    class WpfUI
    {
        private RichTextBox chatBox;
        private ScrollViewer scroller;

        public WpfUI(RichTextBox chatBox, ScrollViewer scroller)
        {
            this.chatBox = chatBox;
            this.scroller = scroller;
        }

        public void ShowBotMessage(string message)
        {
            var para = new Paragraph();
            para.Margin = new Thickness(0, 4, 0, 0);

            var label = new Run("[Bot]: ") { Foreground = Brushes.LimeGreen, FontWeight = FontWeights.Bold };
            var text = new Run(message) { Foreground = Brushes.White };

            para.Inlines.Add(label);
            para.Inlines.Add(text);
            chatBox.Document.Blocks.Add(para);
            scroller.ScrollToBottom();
        }

        public void ShowUserMessage(string message)
        {
            var para = new Paragraph();
            para.Margin = new Thickness(0, 4, 0, 0);

            var label = new Run("[You]: ") { Foreground = new SolidColorBrush(Color.FromRgb(255, 215, 0)), FontWeight = FontWeights.Bold };
            var text = new Run(message) { Foreground = Brushes.LightGray };

            para.Inlines.Add(label);
            para.Inlines.Add(text);
            chatBox.Document.Blocks.Add(para);
            scroller.ScrollToBottom();
        }
    }
}