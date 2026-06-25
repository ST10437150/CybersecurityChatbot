using System.Windows.Documents;
using System.Windows.Controls;

namespace CybersecurityChatbot
{
    class ChatBot
    {
        private string userName = string.Empty;
        private string favoriteTopic = string.Empty;
        private bool awaitingName = true;
        private bool awaitingTopic = false;
        private bool awaitingTaskTitle = false;
        private bool awaitingTaskReminder = false;
        private string pendingTaskTitle = string.Empty;
        private string lastTopic = string.Empty;

        private ResponseEngine engine;
        private WpfUI ui;
        private TaskManager taskManager;
        private QuizEngine quizEngine;
        private ActivityLog activityLog;

        public ChatBot(RichTextBox chatBox, ScrollViewer scroller, ActivityLog log, TaskManager tasks, QuizEngine quiz)
        {
            engine = new ResponseEngine();
            ui = new WpfUI(chatBox, scroller);
            taskManager = tasks;
            quizEngine = quiz;
            activityLog = log;
        }

        public void Start()
        {
            ui.ShowBotMessage("Before we begin, what is your name?");
        }

        public void HandleInput(string input)
        {
            ui.ShowUserMessage(input);

            if (awaitingName)
            {
                if (string.IsNullOrWhiteSpace(input)) { ui.ShowBotMessage("Name cannot be empty."); return; }
                userName = input.Trim();
                awaitingName = false;
                ui.ShowBotMessage($"Welcome, {userName}! I'm your Cybersecurity Awareness Assistant.");
                ui.ShowBotMessage("What is your favourite cybersecurity topic? (e.g. passwords, phishing, privacy)");
                awaitingTopic = true;
                return;
            }

            if (awaitingTopic)
            {
                favoriteTopic = input.Trim();
                awaitingTopic = false;
                ui.ShowBotMessage($"Great! I'll remember you're interested in {favoriteTopic}.");
                ui.ShowBotMessage("Type 'help' to see what I can do!");
                return;
            }

            // Quiz active — forward answers
            if (quizEngine.IsActive && quizEngine.AwaitingAnswer)
            {
                string quizResult = quizEngine.SubmitAnswer(input);
                if (quizResult != null)
                {
                    ui.ShowBotMessage(quizResult);
                    if (!quizEngine.IsActive) activityLog.Add("Quiz completed");
                    return;
                }
            }

            // Awaiting reminder for a task
            if (awaitingTaskReminder)
            {
                awaitingTaskReminder = false;
                string lower2 = input.Trim().ToLower();
                string reminder = (lower2 == "no" || lower2 == "n" || lower2 == "skip") ? null : input.Trim();
                string desc = engine.GetTaskDescription(pendingTaskTitle);
                string res = taskManager.AddTask(pendingTaskTitle, desc, reminder);
                ui.ShowBotMessage(res);
                activityLog.Add($"Task added: '{pendingTaskTitle}'" + (reminder != null ? $" (Reminder: {reminder})" : ""));
                pendingTaskTitle = string.Empty;
                return;
            }

            // Awaiting task title
            if (awaitingTaskTitle)
            {
                awaitingTaskTitle = false;
                pendingTaskTitle = input.Trim();
                awaitingTaskReminder = true;
                ui.ShowBotMessage($"Task: '{pendingTaskTitle}'. Would you like a reminder? (e.g. 'in 3 days', or type 'no')");
                return;
            }

            string lower = input.Trim().ToLower();

            if (lower == "exit" || lower == "bye") { ui.ShowBotMessage($"Goodbye, {userName}! Stay safe online!"); return; }

            if (lower == "help" || lower.Contains("what can you do")) { ShowHelp(); return; }

            // Activity Log
            if (lower.Contains("show activity log") || lower.Contains("activity log") ||
                lower.Contains("what have you done") || lower.Contains("recent actions") || lower.Contains("show log"))
            {
                ui.ShowBotMessage(activityLog.GetLog());
                return;
            }

            // Quiz
            if (lower.Contains("quiz") || lower.Contains("start quiz") || lower.Contains("mini game"))
            {
                if (quizEngine.IsActive) { ui.ShowBotMessage("Quiz already in progress! Answer the current question."); return; }
                activityLog.Add("Quiz started");
                ui.ShowBotMessage("Starting Cybersecurity Quiz!");
                ui.ShowBotMessage(quizEngine.StartQuiz());
                return;
            }

            if (lower.Contains("stop quiz")) { quizEngine.StopQuiz(); ui.ShowBotMessage("Quiz stopped."); return; }

            // View tasks
            if (lower.Contains("view task") || lower.Contains("show task") || lower.Contains("my tasks") || lower.Contains("list task"))
            {
                ui.ShowBotMessage(taskManager.FormatTaskList());
                return;
            }

            // Add task
            if (lower.Contains("add task") || lower.Contains("create task") || lower.Contains("new task"))
            {
                string title = ExtractAfterKeyword(input, new[] { "add task", "create task", "new task" });
                if (string.IsNullOrWhiteSpace(title))
                {
                    awaitingTaskTitle = true;
                    ui.ShowBotMessage("What is the title of the task?");
                }
                else
                {
                    pendingTaskTitle = title.Trim();
                    awaitingTaskReminder = true;
                    ui.ShowBotMessage($"Task: '{pendingTaskTitle}'. Would you like a reminder? (e.g. 'in 3 days', or type 'no')");
                }
                return;
            }

            // Complete task
            if (lower.Contains("complete task") || lower.Contains("mark task") || lower.Contains("done task"))
            {
                int num = ExtractNumber(lower);
                if (num > 0) { ui.ShowBotMessage(taskManager.MarkCompleted(num)); activityLog.Add($"Task #{num} completed"); }
                else ui.ShowBotMessage("Specify the task number. E.g. 'complete task 1'");
                return;
            }

            // Delete task
            if (lower.Contains("delete task") || lower.Contains("remove task"))
            {
                int num = ExtractNumber(lower);
                if (num > 0) { ui.ShowBotMessage(taskManager.DeleteTask(num)); activityLog.Add($"Task #{num} deleted"); }
                else ui.ShowBotMessage("Specify the task number. E.g. 'delete task 1'");
                return;
            }

            // NLP: remind me to...
            if (lower.Contains("remind me to") || lower.Contains("remind me about") || lower.Contains("set a reminder"))
            {
                string taskTitle = ExtractAfterKeyword(input, new[] { "remind me to", "remind me about", "set a reminder for", "set a reminder to" });
                if (!string.IsNullOrWhiteSpace(taskTitle))
                {
                    string desc = engine.GetTaskDescription(taskTitle) ?? $"Remember to: {taskTitle}";
                    ui.ShowBotMessage(taskManager.AddTask(taskTitle, desc, "as requested"));
                    activityLog.Add($"Reminder set: '{taskTitle}'");
                }
                else ui.ShowBotMessage("What would you like me to remind you about?");
                return;
            }

            // Follow-up
            if (lower.Contains("tell me more") || lower.Contains("more") || lower.Contains("another tip"))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                {
                    ui.ShowBotMessage(engine.GetFollowUp(lastTopic) ?? "Stay vigilant online!");
                    return;
                }
            }

            // Sentiment
            string sentiment = engine.DetectSentiment(lower);
            if (sentiment != null) ui.ShowBotMessage(sentiment);

            // Memory
            if (lower.Contains("what do you remember") || lower.Contains("what do you know about me"))
            {
                ui.ShowBotMessage($"Your name is {userName} and you're interested in {(string.IsNullOrEmpty(favoriteTopic) ? "cybersecurity" : favoriteTopic)}.");
                return;
            }

            // Keyword response
            string response = engine.GetResponse(lower);
            if (response != null)
            {
                lastTopic = engine.GetMatchedKeyword(lower);
                if (!string.IsNullOrEmpty(favoriteTopic) && lower.Contains(favoriteTopic.ToLower()))
                    ui.ShowBotMessage($"As someone interested in {favoriteTopic}: {response}");
                else
                    ui.ShowBotMessage(response);
                activityLog.Add($"Topic discussed: {lastTopic}");
            }
            else
            {
                ui.ShowBotMessage("I didn't understand that. Try: passwords, phishing, quiz, add task, show tasks, or 'help'.");
            }
        }

        private void ShowHelp()
        {
            ui.ShowBotMessage(
                $"Here's what I can do, {userName}:\n" +
                "• Cybersecurity info: passwords, phishing, malware, privacy, 2fa, scam, safe browsing\n" +
                "• Tasks: 'add task [title]', 'view tasks', 'complete task 1', 'delete task 1'\n" +
                "• Reminders: 'remind me to update my password'\n" +
                "• Quiz: type 'quiz' to start the mini-game\n" +
                "• Log: type 'show activity log'\n" +
                "• Memory: type 'what do you remember'\n" +
                "• Type 'bye' to exit"
            );
        }

        private string ExtractAfterKeyword(string input, string[] keywords)
        {
            string lower = input.ToLower();
            foreach (var kw in keywords)
            {
                int idx = lower.IndexOf(kw);
                if (idx >= 0)
                {
                    string after = input.Substring(idx + kw.Length).Trim();
                    if (!string.IsNullOrWhiteSpace(after)) return after;
                }
            }
            return null;
        }

        private int ExtractNumber(string input)
        {
            foreach (var word in input.Split(' '))
                if (int.TryParse(word, out int n)) return n;
            return -1;
        }
    }
} // task 1: task assistent implemented
