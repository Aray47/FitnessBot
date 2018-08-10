using System;

namespace FitBot.Model
{
    [Serializable]
    public partial class FitBotResponses
    {

        static DateTime nyrs = new DateTime(2018, 1, 1);
        public static double daysSinceNewYears = nyrs.Subtract(DateTime.Today).TotalDays;

        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static String[] greetOptsArray =
        {
            "Hello, I'm FatBot... I mean FitBot.",
            "Well if it isn't you... whoever you are.",
            "Welcome, I'm FitBot.",
            "Hello there, I'm FitBot!",
            $"New Years Resolution eh? You're {daysSinceNewYears} days behind!".Replace("-", "")
        };

        public static String[] nameOptsArray =
        {
            "What is your name?",
            "What do you call yourself?",
            "What's your name?",
            "Would you be so kind as to give me your name? "
        };

        public static String[] nameResponseOptsArray =
        {
            "...eh, standard.",
            " what a lovely name!",
            " -- I've never heard that name before."
        };

        public static String[] dontUnderstandAge =
        {
            "I'm sorry, I don't understand your reply. Age is a number.",
            "Age is an integer. 10 or 11. Nothing in between.",
            "I'm not a mind reader, give me your age.",
            "I don't care if you're 22 and a half, you're still 22."
        };

        public static String[] calcBMIOptsArray =
        {
            "Calculating your BMI now...",
            "Hang on a second, crunching some numbers",
            "Give me a moment..."
        };

        public static String[] tooOldOptsArray =
        {
            "Too old, go get a twinkie and enjoy it.",
            "If you're 100 or over, you certainly shouldn't give a damn about your BMI, live a little."
        };
    }
}