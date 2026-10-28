using ClinicApp.Enums;
using ClinicApp.Interfaces;

namespace ClinicApp.Models
{
    public class Appointment : IPayable, ICancellable
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public AppointmentStatus Status { get; set; }

        public bool IsPaid { get; set; }
        public bool IsCancelled { get; set; }
        public string CancellationReason { get; set; }

        public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledAt = scheduledAt;
            DurationMinutes = durationMinutes;
            Status = AppointmentStatus.Scheduled;
            IsPaid = false;
            IsCancelled = false;
            CancellationReason = string.Empty;
        }

        public virtual decimal GetCost()
        {
            return DurationMinutes * 10m;
        }

        public virtual string GetDescription()
        {
            return "Прийом";
        }

        public int GetPriority()
        {
            return 3;
        }

        public void MarkPaid()
        {
            IsPaid = true;
        }

        public bool Cancel(string reason = "")
        {
            if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.Cancelled)
            {
                return false;
            }
            Status = AppointmentStatus.Cancelled;
            IsCancelled = true;
            CancellationReason = reason;
            return true;
        }

        public override string ToString()
        {
            string timeRange = $"{ScheduledAt:dd.MM.yyyy HH:mm}–{ScheduledAt.AddMinutes(DurationMinutes):HH:mm}";
            return $"[{Id}] {GetDescription()} | Пацієнт #{PatientId} → Лікар #{DoctorId} | {timeRange} | {Status} | {GetCost():F2} грн";
        }
    }
}