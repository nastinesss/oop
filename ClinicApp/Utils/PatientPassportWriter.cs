using System;
using System.IO;
using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    internal class PatientPassportWriter
    {
        private readonly Clinic _clinic;
        private readonly string _directory;

        public PatientPassportWriter(
            Clinic clinic,
            string directory = "patients")
        {
            _clinic = clinic;
            _directory = directory;

            Directory.CreateDirectory(_directory);
        }

        public void OnPatientAdded(object? sender, PatientEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        public void OnAppointmentCompleted(
            object? sender,
            AppointmentEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        public void OnPlanCompleted(
            object? sender,
            TreatmentPlanEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        private void WritePassport(int patientId)
        {
            Patient? patient = _clinic.Patients.FindById(patientId);

            if (patient == null)
            {
                return;
            }

            string path = Path.Combine(
                _directory,
                $"passport_{patientId}.txt");

            MedicalRecord[] records =
                _clinic.MedicalRecords.GetByPatient(patientId);

            using (StreamWriter writer =
                new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine(
                    $"=== Паспорт пацієнта #{patient.Id} ===");

                writer.WriteLine(
                    $"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");

                writer.WriteLine($"Ім'я: {patient.FullName}");

                writer.WriteLine(
                    $"Дата народження: " +
                    $"{patient.DateOfBirth:dd.MM.yyyy} " +
                    $"({ClinicFormatter.FormatAge(patient.Age)})");

                writer.WriteLine(
                    $"Група крові: " +
                    $"{ClinicFormatter.FormatBloodType(patient.BloodType)}");

                writer.WriteLine(
                    $"Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");

                writer.WriteLine();
                writer.WriteLine("-- Діагнози --");

                foreach (MedicalRecord record in records)
                {
                    if (record is Diagnosis diagnosis)
                    {
                        writer.WriteLine(diagnosis);
                    }
                }

                writer.WriteLine();
                writer.WriteLine("-- Аналізи --");

                foreach (MedicalRecord record in records)
                {
                    if (record is LabResult labResult)
                    {
                        writer.WriteLine(labResult);
                    }
                }

                writer.WriteLine();
                writer.WriteLine("-- Рецепти --");

                foreach (MedicalRecord record in records)
                {
                    if (record is Prescription prescription)
                    {
                        writer.WriteLine(prescription);
                    }
                }

                writer.WriteLine();
                writer.WriteLine("-- Записи --");

                Appointment[] appointments =
                    _clinic.Appointments.GetByPatient(patientId);

                foreach (Appointment appointment in appointments)
                {
                    writer.WriteLine(appointment);
                }

                writer.WriteLine();
                writer.WriteLine("-- Плани лікування --");

                TreatmentPlan[] plans =
                    _clinic.TreatmentPlans.GetByPatient(patientId);

                foreach (TreatmentPlan plan in plans)
                {
                    writer.WriteLine(plan);
                }

                writer.WriteLine();
                writer.WriteLine(
                    $"Заборгованість: " +
                    $"{_clinic.Billing.GetPatientDebt(patientId):F2} грн");
            }
        }
    }
}