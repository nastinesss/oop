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

        // Додано множник знижки (за замовчуванням 1.0 — без знижки)
        public decimal DiscountMultiplier { get; set; } = 1.0m;

        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
        public bool IsUpcoming => Status == AppointmentStatus.Scheduled && ScheduledAt > DateTime.Now;
        public string Notes { get; set; } = string.Empty;

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
            DiscountMultiplier = 1.0m;
        }

        // Обрано Варіант А: множник знижки у базовому класі Appointment.
        // Це дозволяє застосовувати знижку до будь-якого типу прийомів (звичайний, терміновий, спеціаліста) 
        // без потреби створювати нові підкласи, що відповідає принципу Open/Closed (OCP).
        public virtual decimal GetCost()
        {
            // Базова вартість із врахуванням знижкового множника
            return DurationMinutes * 10m * DiscountMultiplier;
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
            string timeRange = $"{ScheduledAt:dd.MM.yyyy HH:mm}–{ScheduledAt.AddMinutes(DurationMinutes):HH:mm}";
            return $"[{Id}] {GetDescription()} | Пацієнт #{PatientId} → Лікар #{DoctorId} | {timeRange} | {Status} | {GetCost():F2} грн";
        }
    }
}