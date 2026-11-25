using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class AnalyticsManager
    {
        private readonly AppointmentManager _appointments;
        private readonly DoctorManager _doctors;
        private readonly PatientManager _patients;

        public AnalyticsManager(
            AppointmentManager appointments,
            DoctorManager doctors,
            PatientManager patients)
        {
            _appointments = appointments;
            _doctors = doctors;
            _patients = patients;
        }

        public IEnumerable<DoctorStats> ComputeDoctorStats()
        {
            Appointment[] appointments = _appointments.GetAll();

            return _doctors.GetAll().Select(doctor =>
            {
                var visits = appointments
                    .Where(a => a.DoctorId == doctor.Id)
                    .ToArray();

                DateTime lastDate = visits.Any()
                    ? visits.Max(a => a.ScheduledAt)
                    : DateTime.MinValue;

                return new DoctorStats(
                    doctor.Id,
                    doctor.FullName,
                    visits.Count(),
                    visits.Sum(a => a.GetCost()),
                    lastDate);
            });
        }

        public IEnumerable<PatientStats> ComputePatientStats()
        {
            Appointment[] appointments = _appointments.GetAll();

            return _patients.GetAll().Select(patient =>
            {
                var visits = appointments
                    .Where(a => a.PatientId == patient.Id)
                    .ToArray();

                DateTime lastDate = visits.Any()
                    ? visits.Max(a => a.ScheduledAt)
                    : DateTime.MinValue;

                return new PatientStats(
                    patient.Id,
                    patient.FullName,
                    visits.Count(),
                    visits.Sum(a => a.GetCost()),
                    lastDate);
            });
        }
    }
}