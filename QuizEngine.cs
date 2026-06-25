using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public bool IsTrueFalse { get; set; }
        public bool TrueFalseAnswer { get; set; }
        public string Explanation { get; set; }
    }

    class QuizEngine
    {
        private List<QuizQuestion> questions;
        private int currentIndex = 0;
        private int score = 0;
        public bool IsActive { get; private set; } = false;
        public bool AwaitingAnswer { get; private set; } = false;

        public QuizEngine()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report it as phishing", "D) Ignore it" },
                    CorrectIndex = 2,
                    IsTrueFalse = false,
                    Explanation = "Correct! Reporting phishing emails helps protect yourself and others."
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password on multiple sites is safe if the password is strong.",
                    IsTrueFalse = true,
                    TrueFalseAnswer = false,
                    Explanation = "False! If one site is breached, attackers access all your accounts."
                },
                new QuizQuestion
                {
                    Question = "What does HTTPS in a URL indicate?",
                    Options = new List<string> { "A) The site is fast", "B) The site is encrypted and more secure", "C) The site is free", "D) The site is popular" },
                    CorrectIndex = 1,
                    IsTrueFalse = false,
                    Explanation = "HTTPS means your connection is encrypted, protecting your data."
                },
                new QuizQuestion
                {
                    Question = "True or False: Two-factor authentication (2FA) makes your account significantly more secure.",
                    IsTrueFalse = true,
                    TrueFalseAnswer = true,
                    Explanation = "True! 2FA adds a second layer — even if your password is stolen, attackers can't get in."
                },
                new QuizQuestion
                {
                    Question = "Which of the following is the strongest password?",
                    Options = new List<string> { "A) password123", "B) John1990", "C) P@ssw0rd!", "D) xK#9mL!2qZ$v" },
                    CorrectIndex = 3,
                    IsTrueFalse = false,
                    Explanation = "Long, random passwords with symbols and mixed case are hardest to crack."
                },
                new QuizQuestion
                {
                    Question = "What is social engineering in cybersecurity?",
                    Options = new List<string> { "A) Building social media apps", "B) Manipulating people into revealing confidential info", "C) Engineering software for social networks", "D) A type of firewall" },
                    CorrectIndex = 1,
                    IsTrueFalse = false,
                    Explanation = "Social engineering tricks people rather than hacking systems."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi is safe for online banking.",
                    IsTrueFalse = true,
                    TrueFalseAnswer = false,
                    Explanation = "False! Public Wi-Fi can be intercepted. Use a VPN or mobile data for sensitive activities."
                },
                new QuizQuestion
                {
                    Question = "What does malware stand for?",
                    Options = new List<string> { "A) Malfunctioning software", "B) Malicious software", "C) Managed software", "D) Manual software" },
                    CorrectIndex = 1,
                    IsTrueFalse = false,
                    Explanation = "Malware is any software designed to harm, exploit or disrupt systems."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should click 'Unsubscribe' in suspicious emails.",
                    IsTrueFalse = true,
                    TrueFalseAnswer = false,
                    Explanation = "False! This can confirm your email is active and lead to phishing sites."
                },
                new QuizQuestion
                {
                    Question = "Which action best protects your social media privacy?",
                    Options = new List<string> { "A) Share your location in every post", "B) Use your real name everywhere", "C) Set your profile to private and limit personal info", "D) Accept all friend requests" },
                    CorrectIndex = 2,
                    IsTrueFalse = false,
                    Explanation = "Limiting who sees your info reduces exposure to scammers and identity thieves."
                },
                new QuizQuestion
                {
                    Question = "What is ransomware?",
                    Options = new List<string> { "A) Software that optimises your PC", "B) A type of antivirus", "C) Malware that locks your files and demands payment", "D) A backup tool" },
                    CorrectIndex = 2,
                    IsTrueFalse = false,
                    Explanation = "Ransomware encrypts your files and demands a ransom. Regular backups are your best defence."
                },
                new QuizQuestion
                {
                    Question = "True or False: Keeping your software updated helps protect against cyberattacks.",
                    IsTrueFalse = true,
                    TrueFalseAnswer = true,
                    Explanation = "True! Updates patch security vulnerabilities that attackers exploit."
                }
            };
        }

        public string StartQuiz()
        {
            IsActive = true;
            currentIndex = 0;
            score = 0;
            AwaitingAnswer = true;
            return GetCurrentQuestion();
        }

        public string GetCurrentQuestion()
        {
            if (currentIndex >= questions.Count) return EndQuiz();
            var q = questions[currentIndex];
            string text = $"Question {currentIndex + 1} of {questions.Count}:\n{q.Question}\n";
            if (q.IsTrueFalse)
                text += "Type: TRUE or FALSE";
            else
            {
                foreach (var opt in q.Options)
                    text += $"\n{opt}";
                text += "\nType A, B, C or D.";
            }
            return text;
        }

        public string SubmitAnswer(string input)
        {
            if (!IsActive || !AwaitingAnswer) return null;
            var q = questions[currentIndex];
            bool correct = false;
            string lower = input.Trim().ToLower();

            if (q.IsTrueFalse)
            {
                if (lower == "true" || lower == "t") correct = q.TrueFalseAnswer == true;
                else if (lower == "false" || lower == "f") correct = q.TrueFalseAnswer == false;
                else return "Please type TRUE or FALSE.";
            }
            else
            {
                int answerIndex = lower switch { "a" => 0, "b" => 1, "c" => 2, "d" => 3, _ => -1 };
                if (answerIndex == -1) return "Please type A, B, C or D.";
                correct = answerIndex == q.CorrectIndex;
            }

            if (correct) score++;
            string result = correct ? $"✓ Correct! {q.Explanation}" : $"✗ Not quite. {q.Explanation}";
            currentIndex++;

            if (currentIndex >= questions.Count)
            {
                AwaitingAnswer = false;
                return result + "\n\n" + EndQuiz();
            }
            return result + "\n\n" + GetCurrentQuestion();
        }

        private string EndQuiz()
        {
            IsActive = false;
            AwaitingAnswer = false;
            string feedback = score >= 10 ? "Outstanding! You're a cybersecurity pro!" :
                              score >= 7 ? "Great job! Solid cybersecurity knowledge." :
                              score >= 5 ? "Good effort! Keep learning to stay safe." :
                                            "Keep learning! Practice makes perfect.";
            return $"Quiz complete! You scored {score}/{questions.Count}.\n{feedback}";
        }

        public void StopQuiz() { IsActive = false; AwaitingAnswer = false; }
    }
} // task 2: quiz implemented
