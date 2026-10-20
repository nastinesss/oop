using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class Appointment : IPayable, ICancellable
    {
        private static int _nextId = 1;

        private DateTime _scheduledAt;
        private int _durationMinutes;
        private bool _isPaid = false;

        public int Id { get; }
        public int PatientId { get; }
        public int DoctorId { get; }

        public DateTime ScheduledAt
        {
            get => _scheduledAt;
            set => _scheduledAt = ClinicValidator.ValidateAppointmentDate(value);
        }

        public int DurationMinutes
        {
            get => _durationMinutes;
            set => _durationMinutes = ClinicValidator.ValidatePositiveNumber(value, nameof(DurationMinutes));
        }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string Notes { get; private set; }

        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

        public bool IsPaid => _isPaid;

        public bool IsCancelled => Status == AppointmentStatus.Cancelled;
        public string CancellationReason => IsCancelled ? Notes : "";

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

        public decimal GetCost()
        {
            return DurationMinutes * 10m;
        }

        public void MarkPaid()
        {
            if (!IsCancelled)
            {
                _isPaid = true;
            }
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
            string baseStr = $"[{Id}] {ScheduledAt:yyyy-MM-dd HH:mm}–{EndsAt:HH:mm} ({DurationMinutes} хв) | Пацієнт: #{PatientId} | Лікар: #{DoctorId} | Статус: {Status} | Оплачено: {IsPaid}";
            if (Notes.Length > 0)
            {
                baseStr += $" | Примітка: {Notes}";
            }
            return baseStr;
        }
    }
}