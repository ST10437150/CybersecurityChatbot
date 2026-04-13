using System;

namespace CybersecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            AudioPlayer.PlayGreeting("greeting.wav");
            ConsoleUI.DisplayLogo();

            ChatBot bot = new ChatBot();
            bot.Start();
        }
    }
}
