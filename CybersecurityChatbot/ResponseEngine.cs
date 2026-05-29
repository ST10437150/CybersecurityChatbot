using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    class ResponseEngine
    {
        private Dictionary<string, string> responses;
        private Dictionary<string, List<string>> randomResponses;
        private Dictionary<string, string> followUps;
        private string matchedKeyword = string.Empty;
        private Random random = new Random();

        public ResponseEngine()
        {
            responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
{ "how are you", "I'm running smoothly and ready to keep you safe online!" },
{ "what's your purpose", "My purpose is to educate you about cybersecurity and help you stay safe online." },
{ "what can i ask you about", "You can ask me about: password safety, phishing, safe browsing, and online threats." },
{ "password", "Use strong passwords with at least 12 characters. Mix letters, numbers, and symbols. Never reuse passwords!" },
{ "safe browsing", "Always look for HTTPS in website URLs. Avoid downloading files from unknown sources." },
{ "malware", "Malware is harmful software. Keep your antivirus updated and avoid clicking unknown links or attachments." },
{ "privacy", "Protect your privacy online: use VPNs, check app permissions, and avoid sharing personal info publicly." },
{ "scam", "Be cautious of unexpected messages asking for money or personal info. Verify the source before responding." },
{ "hello", "Hello! How can I help you with cybersecurity today?" },
{ "hi", "Hi there! Ask me anything about staying safe online." },
{ "bye", "Stay safe online! Goodbye!" }
};

            randomResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
{
{ "phishing", new List<string>
{
"Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
"Phishing emails often create urgency. Don't rush — verify the sender before clicking any link.",
"Check the sender's email address carefully. Fake domains often look very similar to real ones.",
"Never enter your credentials on a page you reached via an email link. Go directly to the website instead."
}
},
{ "2fa", new List<string>
{
"Two-factor authentication adds an extra layer of security. Enable it on all your important accounts.",
"Even if someone gets your password, 2FA can stop them from accessing your account.",
"Use an authenticator app instead of SMS for stronger 2FA protection."
}
}
};

            followUps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
{ "password", "Also consider using a password manager to generate and store complex passwords securely." },
{ "phishing", "Another tip: hover over links before clicking to preview the actual URL destination." },
{ "privacy", "You can also review your social media privacy settings regularly to limit who sees your data." },
{ "malware", "Run regular system scans and keep your operating system updated to patch security vulnerabilities." },
{ "scam", "If you suspect a scam, report it to your local cybercrime authority and block the sender immediately." },
{ "safe browsing", "Consider using a browser extension like uBlock Origin to block malicious ads and trackers." }
};
        }

        public string GetResponse(string input)
        {
            // Check random responses first
            foreach (var key in randomResponses.Keys)
            {
                if (input.ToLower().Contains(key.ToLower()))
                {
                    matchedKeyword = key;
                    var list = randomResponses[key];
                    return list[random.Next(list.Count)];
                }
            }

            // Check standard responses
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

        public string GetMatchedKeyword(string input)
        {
            return matchedKeyword;
        }

        public string GetFollowUp(string topic)
        {
            if (followUps.ContainsKey(topic))
                return followUps[topic];
            return null;
        }

        public string DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("afraid"))
                return "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe.";

            if (input.Contains("frustrated") || input.Contains("angry") || input.Contains("annoyed"))
                return "I hear you — dealing with cyber threats can be really frustrating. Let's work through this together.";

            if (input.Contains("curious") || input.Contains("interested") || input.Contains("want to know"))
                return "That's great that you're curious! Staying informed is one of the best ways to protect yourself online.";

            if (input.Contains("confused") || input.Contains("don't understand") || input.Contains("not sure"))
                return "No worries at all! Let me try to explain it more clearly for you.";

            return null;
        }
    }
}
// Memory and recall implemented
