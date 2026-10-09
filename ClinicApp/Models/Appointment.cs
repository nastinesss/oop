using ClinicApp.Enums;
using System;

namespace ClinicApp.Models
{
    public class Appointment
    {
        private static int _nextId = 1;

        // Приватне поле для дати
        private DateTime _scheduledAt;

        public int Id { get; }
        public int PatientId { get; }
        public int DoctorId { get; }

        public DateTime ScheduledAt
        {
            get => _scheduledAt;
            set
            {
                if (value < DateTime.Now.AddMinutes(-5))
                    throw new ArgumentException("Час прийому не може бути в минулому.", nameof(ScheduledAt));

                _scheduledAt = value;
            }
        }

        public int DurationMinutes { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string Notes { get; private set; }

        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

        public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            Id = _nextId++;
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledAt = scheduledAt;
            DurationMinutes = durationMinutes;
            Status = AppointmentStatus.Scheduled;
            Notes = "";
        }

        public bool Cancel(string reason = "")
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                return false;
            }

            Status = AppointmentStatus.Cancelled;
            if (!string.IsNullOrWhiteSpace(reason))
            {
                Notes = reason;
            }
            return true;
        }

        public bool Complete()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                return false;
            }

            Status = AppointmentStatus.Completed;
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