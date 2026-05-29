using System.Windows;
using System.Windows.Input;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        private ChatBot bot;

        public MainWindow()
        {
            InitializeComponent();
            bot = new ChatBot(ChatBox, ChatScroller);
            AudioPlayer.PlayGreeting("greeting.wav");
            bot.Start();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ProcessInput();
        }

        private void ProcessInput()
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;
            UserInput.Clear();
            bot.HandleInput(input);
        }
    }
}