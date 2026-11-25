using System;
using System.IO;
using System.Text;
using ClinicApp.Events;

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
        public void OnPatientAdded(object? sender, PatientEventArgs e)
        {
            LogInfo($"Додано пацієнта #{e.PatientId}: {e.FullName}");
        }

        public void OnAppointmentBooked(object? sender, AppointmentEventArgs e)
        {
            LogInfo(
                $"Запис #{e.AppointmentId} створено: " +
                $"пацієнт #{e.PatientId} → лікар #{e.DoctorId}, " +
                $"{e.ScheduledAt:dd.MM.yyyy HH:mm}");
        }

        public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
        {
            LogInfo(
                $"Запис #{e.AppointmentId} скасовано: " +
                $"пацієнт #{e.PatientId}, лікар #{e.DoctorId}");
        }

        public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
        {
            LogInfo(
                $"Запис #{e.AppointmentId} завершено: " +
                $"пацієнт #{e.PatientId}");
        }

        public void OnUrgentAppointmentBooked(
            object? sender,
            AppointmentEventArgs e)
        {
            string message =
                $"Терміновий запис #{e.AppointmentId}: " +
                $"пацієнт #{e.PatientId} → лікар #{e.DoctorId}, " +
                $"{e.ScheduledAt:dd.MM.yyyy HH:mm}. " +
                $"Причина: {e.Notes}";

            LogWarning(message);

            Directory.CreateDirectory("alerts");

            string path = Path.Combine(
                "alerts",
                $"urgent_{DateTime.Now:yyyy-MM-dd}.txt");

            message = message.Replace("\r", " ").Replace("\n", " ");

            File.AppendAllText(
                path,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}" +
                Environment.NewLine,
                Encoding.UTF8);
        }

        public void OnPaymentReceived(object? sender, PaymentEventArgs e)
        {
            LogInfo(
                $"Отримано оплату за запис #{e.AppointmentId}: " +
                $"{e.Amount:F2} грн");
        }

        public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
        {
            LogInfo(
                $"План #{e.PlanId} завершено: " +
                $"пацієнт #{e.PatientId}, діагноз: {e.Diagnosis}");
        }
    }
}