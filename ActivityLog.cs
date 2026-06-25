using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    class ActivityLog
    {
        private List<string> log = new List<string>();
        private const int MaxVisible = 10;

        public void Add(string action)
        {
            log.Add($"[{DateTime.Now:HH:mm:ss}] {action}");
        }

        public string GetLog()
        {
            if (log.Count == 0) return "No activity recorded yet.";
            int start = Math.Max(0, log.Count - MaxVisible);
            string result = "Here's a summary of recent actions:\n";
            int number = 1;
            for (int i = start; i < log.Count; i++)
            {
                result += $"{number}. {log[i]}\n";
                number++;
            }
            if (log.Count > MaxVisible)
                result += $"(Showing last {MaxVisible} of {log.Count} total)";
            return result.TrimEnd();
        }
    }
} // Task 4: Activity log implemented
