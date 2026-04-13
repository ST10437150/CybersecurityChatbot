using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CybersecurityChatbot
{
    class AudioPlayer
    {
        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string szSound,
            IntPtr hMod, uint fdwSound);

        private const uint SND_FILENAME = 0x00020000;
        private const uint SND_ASYNC = 0x0001;

        public static void PlayGreeting(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    PlaySound(filePath, IntPtr.Zero,
                        SND_FILENAME | SND_ASYNC);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  [Audio file not found]");
                    Console.ResetColor();
                }
                
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  [Could not play audio]");
                Console.ResetColor();
            }
        }
    }
}
