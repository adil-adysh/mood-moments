using System;

namespace mood_moments.Utilities
{
    public static class Logger
    {
        public static void LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.Now}: {message}");
        }

        public static void LogWarning(string message)
        {
            Console.WriteLine($"[WARNING] {DateTime.Now}: {message}");
        }

        public static void LogError(string message, Exception? ex = null)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now}: {message}");
            if (ex != null)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }
    }
}
