using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    class ResponseEngine
    {
        private Dictionary<string, string> responses;
        private Dictionary<string, List<string>> randomResponses;
        private Dictionary<string, string> followUps;
        private Dictionary<string, string> taskDescriptions;
        private string matchedKeyword = string.Empty;
        private Random random = new Random();

        public ResponseEngine()
        {
            responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "how are you", "I'm running smoothly and ready to keep you safe online!" },
                { "what's your purpose", "My purpose is to educate you about cybersecurity and help you stay safe online." },
                { "what can i ask you about", "You can ask about: passwords, phishing, safe browsing, malware, privacy, 2FA, scams. Type 'help' for all commands." },
                { "password", "Use strong passwords with at least 12 characters. Mix letters, numbers, and symbols. Never reuse passwords!" },
                { "safe browsing", "Always look for HTTPS in website URLs. Avoid downloading files from unknown sources." },
                { "malware", "Malware is harmful software. Keep your antivirus updated and avoid clicking unknown links." },
                { "privacy", "Protect your privacy: use VPNs, check app permissions, and avoid sharing personal info publicly." },
                { "scam", "Be cautious of unexpected messages asking for money or personal info. Verify the source first." },
                { "ransomware", "Ransomware encrypts your files and demands payment. Keep regular backups!" },
                { "vpn", "A VPN encrypts your internet traffic — essential on public Wi-Fi." },
                { "firewall", "A firewall monitors network traffic based on security rules. Always keep it enabled." },
                { "antivirus", "Antivirus software detects and removes malicious programs. Keep it updated." },
                { "social engineering", "Social engineering manipulates people into revealing confidential info. Always verify identities." },
                { "hello", "Hello! How can I help you with cybersecurity today?" },
                { "hi", "Hi there! Ask me anything about staying safe online." }
            };

            randomResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "phishing", new List<string> {
                    "Be cautious of emails asking for personal info. Scammers disguise themselves as trusted organisations.",
                    "Phishing emails create urgency. Don't rush — verify the sender before clicking any link.",
                    "Check the sender's email address carefully. Fake domains look very similar to real ones.",
                    "Never enter credentials on a page you reached via an email link. Go directly to the website."
                }},
                { "2fa", new List<string> {
                    "Two-factor authentication adds an extra security layer. Enable it on all important accounts.",
                    "Even if someone gets your password, 2FA stops them from accessing your account.",
                    "Use an authenticator app instead of SMS for stronger 2FA protection."
                }},
                { "update", new List<string> {
                    "Always keep your software and OS updated — updates patch vulnerabilities attackers exploit.",
                    "Enable automatic updates so you never miss a critical security patch.",
                    "Outdated software is one of the top causes of successful cyberattacks."
                }}
            };

            followUps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", "Consider using a password manager to generate and store complex passwords securely." },
                { "phishing", "Tip: hover over links before clicking to preview the actual URL destination." },
                { "privacy", "Review your social media privacy settings regularly to limit who sees your data." },
                { "malware", "Run regular system scans and keep your OS updated to patch security vulnerabilities." },
                { "scam", "If you suspect a scam, report it to your local cybercrime authority and block the sender." },
                { "safe browsing", "Use a browser extension like uBlock Origin to block malicious ads and trackers." },
                { "2fa", "Consider a hardware security key for the strongest form of two-factor authentication." },
                { "ransomware", "Back up your data offline or to the cloud regularly so ransomware can't hold you hostage." }
            };

            taskDescriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", "Update your passwords to strong, unique ones using a password manager." },
                { "2fa", "Enable two-factor authentication on all important accounts." },
                { "two-factor", "Enable two-factor authentication on all important accounts." },
                { "two factor", "Enable two-factor authentication on all important accounts." },
                { "privacy", "Review and update your privacy settings on social media and apps." },
                { "antivirus", "Install or update your antivirus software to protect against malware." },
                { "update", "Check for and install all pending software and OS updates." },
                { "backup", "Create a backup of your important files to a secure location." },
                { "vpn", "Set up a VPN to protect your connection on public networks." },
                { "firewall", "Check that your firewall is enabled and properly configured." }
            };
        }

        public string GetResponse(string input)
        {
            foreach (var key in randomResponses.Keys)
            {
                if (input.ToLower().Contains(key.ToLower()))
                {
                    matchedKeyword = key;
                    var list = randomResponses[key];
                    return list[random.Next(list.Count)];
                }
            }
            foreach (var key in responses.Keys)
            {
                if (input.ToLower().Contains(key.ToLower()))
                {
                    matchedKeyword = key;
                    return responses[key];
                }
            }
            return null;
        }

        public string GetMatchedKeyword(string input) => matchedKeyword;

        public string GetFollowUp(string topic)
        {
            return followUps.ContainsKey(topic) ? followUps[topic] : null;
        }

        public string GetTaskDescription(string taskTitle)
        {
            if (string.IsNullOrWhiteSpace(taskTitle)) return "Complete this cybersecurity task.";
            string lower = taskTitle.ToLower();
            foreach (var key in taskDescriptions.Keys)
                if (lower.Contains(key.ToLower())) return taskDescriptions[key];
            return $"Complete the task: {taskTitle}";
        }

        public string DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("afraid"))
                return "It's understandable to feel that way. Let me share some tips to help you stay safe.";
            if (input.Contains("frustrated") || input.Contains("angry") || input.Contains("annoyed"))
                return "I hear you — dealing with cyber threats is frustrating. Let's work through this together.";
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("want to know"))
                return "Great that you're curious! Staying informed is one of the best ways to protect yourself.";
            if (input.Contains("confused") || input.Contains("don't understand") || input.Contains("not sure"))
                return "No worries! Let me try to explain it more clearly.";
            return null;
        }
    }
}