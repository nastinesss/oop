using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    internal class Appointment
    {
        private static int _nextId = 1;

        public int Id { get; }
        public int PatientId { get; }
        public int DoctorId { get; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; private set; }
        public string Notes { get; private set; }

        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == "Scheduled";

        public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            Id = _nextId++;
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledAt = scheduledAt;
            DurationMinutes = durationMinutes;
            Status = "Scheduled";
            Notes = "";
        }

        public bool Cancel(string reason = "")
        {
            if (Status != "Scheduled")
            {
                return false;
            }

            Status = "Cancelled";
            if (!string.IsNullOrWhiteSpace(reason))
            {
                Notes = reason;
            }
            return true;
        }

        public bool Complete()
        {
            if (Status != "Scheduled")
            {
                return false;
            }

            Status = "Completed";
            return true;
        }

        public override string ToString()
        {
            string baseStr = $"[{Id}] {ScheduledAt:yyyy-MM-dd HH:mm}–{EndsAt:HH:mm} ({DurationMinutes} хв) | Пацієнт: #{PatientId} | Лікар: #{DoctorId} | Статус: {Status}";
            if (Notes.Length > 0)
            {
                baseStr += $" | Примітка: {Notes}";
            }
            return baseStr;
        }
    }
}
