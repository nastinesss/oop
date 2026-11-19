using System;
using System.IO;
using System.Text;

namespace ClinicApp.Utils
{
    public class ClinicLogger
    {
        private readonly string _logPath;

        public ClinicLogger(string logPath = "clinic.log")
        {
            _logPath = logPath;
        }

        public void LogInfo(string message)
        {
            Write("INFO", message);
        }

        public void LogWarning(string message)
        {
            Write("WARN", message);
        }

        public void LogError(string message)
        {
            Write("ERROR", message);
        }

        private void Write(string level, string message)
        {
            // Один запис лога завжди займає один рядок.
            message = message.Replace("\r", " ").Replace("\n", " ");

            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                $"[{level,-5}] {message}";

            File.AppendAllText(
                _logPath,
                line + Environment.NewLine,
                Encoding.UTF8);
        }

        public string[] GetLastLines(int n)
        {
            if (n <= 0 || !Exists())
            {
                return Array.Empty<string>();
            }

            string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);

            int count = Math.Min(n, lines.Length);
            int start = lines.Length - count;

            string[] result = new string[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = lines[start + i];
            }

            return result;
        }

        public void Clear()
        {
            if (Exists())
            {
                File.Delete(_logPath);
            }
        }

        public bool Exists()
        {
            return File.Exists(_logPath);
        }
    }
}