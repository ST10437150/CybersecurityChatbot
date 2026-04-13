using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    internal class ResponseHandler
    {
        // Dictionary of keywords → responses
        private static readonly Dictionary<string, string> Responses =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
        {
            { "password",
                "🔑 Password Safety Tips:\n" +
                "  • Use at least 12 characters\n" +
                "  • Mix uppercase, lowercase, numbers & symbols\n" +
                "  • Never reuse passwords across sites\n" +
                "  • Consider using a password manager like Bitwarden" },

            { "phishing",
                "🎣 Phishing Awareness:\n" +
                "  • Always verify the sender's email address\n" +
                "  • Never click suspicious links in emails\n" +
                "  • Banks never ask for passwords via email\n" +
                "  • When in doubt, go directly to the website" },

            { "browsing",
                "🌐 Safe Browsing Tips:\n" +
                "  • Look for HTTPS (🔒) before entering personal info\n" +
                "  • Avoid public Wi-Fi for banking or shopping\n" +
                "  • Keep your browser updated\n" +
                "  • Use an ad blocker to reduce malware risk" },

            { "malware",
                "🦠 Malware Protection:\n" +
                "  • Install reputable antivirus software\n" +
                "  • Never download software from unknown sites\n" +
                "  • Keep your operating system updated\n" +
                "  • Scan USB drives before opening files" },

            { "scam",
                "⚠️ Avoiding Scams:\n" +
                "  • If it sounds too good to be true, it probably is\n" +
                "  • Never send money to unknown recipients\n" +
                "  • Verify caller identity before sharing information\n" +
                "  • Report scams to the SAPS Cybercrime Unit" },

            { "how are you",
                "I'm running at full security protocols and feeling great! " +
                "How can I help keep you safe online today? 😊" },

            { "purpose",
                "My purpose is to educate South African citizens about " +
                "cybersecurity threats and how to stay safe online. " +
                "I can help with topics like phishing, passwords, " +
                "safe browsing, malware, and scams!" },

            { "what can i ask",
                "You can ask me about:\n" +
                "  • password  — password safety tips\n" +
                "  • phishing  — recognising phishing attacks\n" +
                "  • browsing  — safe browsing practices\n" +
                "  • malware   — protecting against malware\n" +
                "  • scam      — avoiding online scams\n" +
                "  • exit      — to quit the chatbot" }
        };

        // Find a matching response
        public static string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return null; // handled as invalid input

            foreach (var entry in Responses)
            {
                if (userInput.Contains(entry.Key,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return entry.Value;
                }
            }
            return null; // no match found
        }
    }
}
