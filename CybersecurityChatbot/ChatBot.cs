using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;

namespace CybersecurityChatbot
{
    class ChatBot
    {
        private string userName = string.Empty;
        private string favoriteTopic = string.Empty;
        private bool awaitingName = true;
        private bool awaitingTopic = false;
        private string lastTopic = string.Empty;

        private ResponseEngine engine;
        private WpfUI ui;

        public ChatBot(RichTextBox chatBox, ScrollViewer scroller)
        {
            engine = new ResponseEngine();
            ui = new WpfUI(chatBox, scroller);
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
                if (string.IsNullOrWhiteSpace(input))
                {
                    ui.ShowBotMessage("Name cannot be empty. Please enter your name.");
                    return;
                }
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
                ui.ShowBotMessage($"Great! I'll remember that you're interested in {favoriteTopic}. It's a crucial part of staying safe online.");
                ui.ShowBotMessage("Type 'exit' or 'bye' to quit. Ask me anything about cybersecurity!");
                return;
            }

            string lower = input.ToLower();

            if (lower == "exit" || lower == "bye")
            {
                ui.ShowBotMessage($"Goodbye, {userName}! Stay safe online!");
                return;
            }

            // Follow-up / conversation flow
            if (lower.Contains("tell me more") || lower.Contains("explain more") || lower.Contains("give me another tip") || lower.Contains("more"))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                {
                    string followUp = engine.GetFollowUp(lastTopic);
                    ui.ShowBotMessage(followUp ?? $"As someone interested in {favoriteTopic}, here's a key tip: {engine.GetResponse(favoriteTopic) ?? "Stay vigilant online!"}");
                    return;
                }
            }

            // Sentiment detection
            string sentiment = engine.DetectSentiment(lower);
            if (sentiment != null)
            {
                ui.ShowBotMessage(sentiment);
                // still continue to give a tip
            }

            // Memory recall
            if (lower.Contains("what do you remember") || lower.Contains("what do you know about me"))
            {
                ui.ShowBotMessage($"I remember that your name is {userName} and you're interested in {(string.IsNullOrEmpty(favoriteTopic) ? "cybersecurity" : favoriteTopic)}.");
                return;
            }

            // Keyword response
            string response = engine.GetResponse(lower);
            if (response != null)
            {
                // Track last topic for follow-up
                lastTopic = engine.GetMatchedKeyword(lower);
                // Personalise with memory
                if (!string.IsNullOrEmpty(favoriteTopic) && lower.Contains(favoriteTopic.ToLower()))
                {
                    ui.ShowBotMessage($"As someone interested in {favoriteTopic}, you might want to know: {response}");
                }
                else
                {
                    ui.ShowBotMessage(response);
                }
            }
            else
            {
                ui.ShowBotMessage("I'm not sure I understand. Could you try rephrasing? Try asking about: passwords, phishing, privacy, malware, or safe browsing.");
            }
        }
    }
}
// Sentiment detection implemented
