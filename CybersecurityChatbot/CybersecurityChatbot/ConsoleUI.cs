using System;
using System.Threading;

namespace CybersecurityChatbot
{
    class ConsoleUI
    {
        public static void DisplayLogo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
  ██████╗██╗   ██╗██████╗ ███████╗██████╗ 
 ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗
 ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝
 ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗
 ╚██████╗   ██║   ██████╔╝███████╗██║  ██║
  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝
            ");
            Console.ResetColor();
            PrintDivider();
            TypeText("  Cybersecurity Awareness Bot - South Africa", ConsoleColor.Green);
            PrintDivider();
            Console.WriteLine();
        }

        public static void PrintDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  ══════════════════════════════════════════");
            Console.ResetColor();
        }

        public static void TypeText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(18);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void ShowBotMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n  [Bot]: ");
            Console.ResetColor();
            TypeText(message, ConsoleColor.White);
        }

        public static void ShowUserPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n  [You]: ");
            Console.ResetColor();
        }

        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  [!] {message}");
            Console.ResetColor();
        }
    }
}
