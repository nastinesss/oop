using System.Collections.Generic;
using System.Linq;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class ReportManager
    {
        private readonly AppointmentManager _appointments;
        private readonly DoctorManager _doctors;
        private readonly PatientManager _patients;
        private readonly AnalyticsManager _analytics;

        public ReportManager(
            AppointmentManager appointments,
            DoctorManager doctors,
            PatientManager patients)
        {
            _appointments = appointments;
            _doctors = doctors;
            _patients = patients;
            _analytics = new AnalyticsManager(appointments, doctors, patients);
        }

        public IEnumerable<SpecialityReport> GetSpecialityStats()
        {
            Appointment[] appointments = _appointments.GetAll();

            return _doctors.GetAll()
                .GroupBy(d => d.Speciality)
                .Select(group =>
                {
                    int[] doctorIds = group.Select(d => d.Id).ToArray();

                    var visits = appointments
                        .Where(a => doctorIds.Contains(a.DoctorId))
                        .ToArray();

                    return new SpecialityReport(
                        group.Key,
                        doctorIds.Length,
                        visits.Length,
                        visits.Sum(a => a.GetCost()));
                })
                .OrderByDescending(report => report.TotalRevenue);
        }

        public string? FindBusiestDoctorName()
        {
            return _analytics.ComputeDoctorStats()
                .OrderByDescending(stats => stats.AppointmentCount)
                .FirstOrDefault()?.FullName;
        }

        public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
        {
            Patient[] patients = _patients.GetAll();

            if (minVisits <= 0)
            {
                return patients.Select(patient => patient.FullName);
            }

            return _appointments.GetAll()
                .GroupBy(a => a.PatientId)
                .Where(group => group.Count() >= minVisits)
                .Join(
                    patients,
                    group => group.Key,
                    patient => patient.Id,
                    (group, patient) => patient.FullName);
        }

        public IEnumerable<DoctorStats> GetTopEarners(int n)
        {
            return _analytics.ComputeDoctorStats()
                .OrderByDescending(stats => stats.TotalRevenue)
                .Take(n);
        }

        public bool HasAnyUrgentAppointments()
        {
            return _appointments.GetAll().Any(a => a is UrgentAppointment);
        }

        public IEnumerable<Speciality> GetActiveSpecialities()
        {
            return _doctors.GetAll()
                .Select(d => d.Speciality)
                .Distinct()
                .OrderBy(speciality => speciality);
        }

        public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
        {
            return _appointments.GetAll()
                .GroupBy(a => new { a.ScheduledAt.Year, a.ScheduledAt.Month })
                .OrderBy(group => group.Key.Year)
                .ThenBy(group => group.Key.Month)
                .Select(group => (
                    Year: group.Key.Year,
                    Month: group.Key.Month,
                    Total: group.Sum(a => a.GetCost())));
        }
    }
}