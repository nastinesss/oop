using System;

namespace ClinicApp.Models
{
    public class DoctorStats : IComparable<DoctorStats>
    {
        public int DoctorId { get; }
        public string FullName { get; }
        public int AppointmentCount { get; }
        public decimal TotalRevenue { get; }
        public DateTime LastAppointmentDate { get; }

        public DoctorStats(int doctorId, string fullName, int appointmentCount, decimal totalRevenue, DateTime lastAppointmentDate)
        {
            DoctorId = doctorId;
            FullName = fullName;
            AppointmentCount = appointmentCount;
            TotalRevenue = totalRevenue;
            LastAppointmentDate = lastAppointmentDate;
        }

        public int CompareTo(DoctorStats? other)
        {
            if (other == null) return 1;
            // Більша кількість прийомів має бути першою (сортування за спаданням)
            return other.AppointmentCount.CompareTo(AppointmentCount);
        }

        public override string ToString()
        {
            string dateStr = LastAppointmentDate == DateTime.MinValue ? "—" : LastAppointmentDate.ToString("dd.MM.yyyy");
            return $"[{DoctorId}] {FullName,-16} | Прийомів: {AppointmentCount} | Виручка: {TotalRevenue:F2} грн | Останній: {dateStr}";
        }
    }
}