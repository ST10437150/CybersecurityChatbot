using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        private ChatBot bot;
        private QuizEngine quizEngine;
        private TaskManager taskManager;
        private ActivityLog activityLog;

        public MainWindow()
        {
            InitializeComponent();
            activityLog = new ActivityLog();
            taskManager = new TaskManager();
            quizEngine = new QuizEngine();
            bot = new ChatBot(ChatBox, ChatScroller, activityLog, taskManager, quizEngine);
            AudioPlayer.PlayGreeting("greeting.wav");
            bot.Start();
            RefreshTaskList();
        }

        // CHAT TAB
        private void SendButton_Click(object sender, RoutedEventArgs e) => ProcessInput();
        private void UserInput_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) ProcessInput(); }
        private void ProcessInput()
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;
            UserInput.Clear();
            bot.HandleInput(input);
        }

        // TASK TAB
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleInput.Text.Trim();
            string reminder = TaskReminderInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(title)) { MessageBox.Show("Please enter a task title."); return; }
            string desc = new ResponseEngine().GetTaskDescription(title);
            string result = taskManager.AddTask(title, desc, string.IsNullOrWhiteSpace(reminder) ? null : reminder);
            activityLog.Add($"Task added: '{title}'");
            MessageBox.Show(result, "Task Added");
            TaskTitleInput.Text = "";
            TaskReminderInput.Text = "";
            RefreshTaskList();
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedIndex < 0) { MessageBox.Show("Select a task first."); return; }
            string result = taskManager.MarkCompleted(TaskListView.SelectedIndex + 1);
            activityLog.Add($"Task #{TaskListView.SelectedIndex + 1} completed");
            MessageBox.Show(result);
            RefreshTaskList();
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedIndex < 0) { MessageBox.Show("Select a task first."); return; }
            if (MessageBox.Show("Delete this task?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                string result = taskManager.DeleteTask(TaskListView.SelectedIndex + 1);
                activityLog.Add($"Task #{TaskListView.SelectedIndex + 1} deleted");
                MessageBox.Show(result);
                RefreshTaskList();
            }
        }

        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e) => RefreshTaskList();
        private void RefreshTaskList()
        {
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = taskManager.GetAllTasks();
        }

        // QUIZ TAB
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (quizEngine.IsActive) { AppendQuizMessage("Quiz already running!"); return; }
            activityLog.Add("Quiz started");
            AppendQuizMessage("=== Cybersecurity Mini-Game Started! ===");
            AppendQuizMessage(quizEngine.StartQuiz());
        }

        private void StopQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quizEngine.StopQuiz();
            activityLog.Add("Quiz stopped");
            AppendQuizMessage("Quiz stopped. Press Start Quiz to play again.");
        }

        private void QuizAnswerButton_Click(object sender, RoutedEventArgs e) => ProcessQuizAnswer();
        private void QuizAnswerInput_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) ProcessQuizAnswer(); }
        private void ProcessQuizAnswer()
        {
            string answer = QuizAnswerInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(answer)) return;
            QuizAnswerInput.Clear();
            AppendQuizMessage($"> You: {answer}");
            if (!quizEngine.IsActive) { AppendQuizMessage("No quiz in progress. Press Start Quiz."); return; }
            string result = quizEngine.SubmitAnswer(answer);
            if (result != null)
            {
                AppendQuizMessage(result);
                if (!quizEngine.IsActive) activityLog.Add("Quiz completed");
            }
        }

        private void AppendQuizMessage(string message)
        {
            var para = new System.Windows.Documents.Paragraph();
            para.Margin = new Thickness(0, 4, 0, 0);
            para.Inlines.Add(new Run(message) { Foreground = Brushes.White });
            QuizBox.Document.Blocks.Add(para);
        }

        // LOG TAB
        private void RefreshLogButton_Click(object sender, RoutedEventArgs e) => RefreshLog();
        private void RefreshLog()
        {
            LogListBox.Items.Clear();
            foreach (var line in activityLog.GetLog().Split('\n'))
                if (!string.IsNullOrWhiteSpace(line)) LogListBox.Items.Add(line.Trim());
        }
    }
}