using System;

namespace CybersecurityChatbot
{
    class ChatBot
    {
        private string userName = string.Empty;
        private ResponseEngine engine;

        public ChatBot()
        {
            engine = new ResponseEngine();
        }

        public void Start()
        {
            GetUserName();
            ConsoleUI.ShowBotMessage($"Welcome, {userName}! I'm your Cybersecurity Awareness Assistant.");
            ConsoleUI.ShowBotMessage("Type 'exit' or 'bye' to quit. Ask me anything about cybersecurity!");
            ConsoleUI.PrintDivider();
            RunConversation();
        }

        private void GetUserName()
        {
            ConsoleUI.ShowBotMessage("Before we begin, what is your name?");

            while (true)
            {
                ConsoleUI.ShowUserPrompt();
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleUI.ShowError("Name cannot be empty. Please enter your name.");
                }
                else
                {
                    userName = input.Trim();
                    break;
                }
            }
        }

        private void RunConversation()
        {
            while (true)
            {
                ConsoleUI.ShowUserPrompt();
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleUI.ShowError("I didn't quite understand that. Could you rephrase?");
                    continue;
                }

                input = input.Trim();

                if (input.ToLower() == "exit" || input.ToLower() == "bye")
                {
                    ConsoleUI.ShowBotMessage($"Goodbye, {userName}! Stay safe online!");
                    break;
                }

                string response = engine.GetResponse(input);

                if (response != null)
                {
                    ConsoleUI.ShowBotMessage(response);
                }
                else
                {
                    ConsoleUI.ShowBotMessage("I didn't quite understand that. Could you rephrase? Try asking about: passwords, phishing, or safe browsing.");
                }
            }
        }
    }
}