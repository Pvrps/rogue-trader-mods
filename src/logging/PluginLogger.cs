using System;
using System.IO;

namespace Purps.RogueTrader.Logging
{
    public static class PluginLogger
    {
        private static string logDir = null;
        private static readonly string defaultLogFileName = "Plugin";

        public static void Init(string modDirectory)
        {
            logDir = modDirectory;
        }

        public static void Log(string message)
        {
            Log(defaultLogFileName, message);
        }

        public static void Log(string fileName, string message)
        {
            if (string.IsNullOrEmpty(logDir))
                logDir = Directory.GetCurrentDirectory(); // fallback, but not ideal
            string logFilePath = Path.Combine(logDir, fileName + ".log");
            string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
            File.AppendAllText(logFilePath, line);
        }
    }
}