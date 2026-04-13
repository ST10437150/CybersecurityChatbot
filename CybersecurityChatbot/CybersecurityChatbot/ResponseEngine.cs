using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    class ResponseEngine
    {
        private Dictionary<string, string> responses;

        public ResponseEngine()
        {
            responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "how are you", "I'm running smoothly and ready to keep you safe online!" },
                { "what's your purpose", "My purpose is to educate you about cybersecurity and help you stay safe online." },
                { "what can i ask you about", "You can ask me about: password safety, phishing, safe browsing, and online threats." },
                { "password", "Use strong passwords with at least 12 characters. Mix letters, numbers, and symbols. Never reuse passwords!" },
                { "phishing", "Phishing emails trick you into giving personal info. Always check the sender's email address and never click suspicious links." },
                { "safe browsing", "Always look for HTTPS in website URLs. Avoid downloading files from unknown sources." },
                { "malware", "Malware is harmful software. Keep your antivirus updated and avoid clicking unknown links or attachments." },
                { "privacy", "Protect your privacy online: use VPNs, check app permissions, and avoid sharing personal info publicly." },
                { "hello", "Hello! How can I help you with cybersecurity today?" },
                { "hi", "Hi there! Ask me anything about staying safe online." },
                { "bye", "Stay safe online! Goodbye!" },
                { "exit", "Exiting... Remember to stay cyber-aware!" }
            };
        }

        public string GetResponse(string input)
        {
            foreach (var key in responses.Keys)
            {
                if (input.ToLower().Contains(key.ToLower()))
                {
                    return responses[key];
                }
            }
            return null;
        }
    }
}
