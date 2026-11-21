using System;
using System.IO;
using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    internal class ClinicExporter
    {
        private readonly Clinic _clinic;
        private readonly string _baseDir;

        public ClinicExporter(Clinic clinic, string baseDir = "reports")
        {
            _clinic = clinic;
            _baseDir = baseDir;
        }

        private string CreateReportPath(string fileName)
        {
            string directory = Path.Combine(
                _baseDir,
                DateTime.Now.ToString("yyyy-MM-dd"));

            Directory.CreateDirectory(directory);

            return Path.Combine(directory, fileName);
        }

        private void WriteHeader(StreamWriter writer, string title)
        {
            writer.WriteLine($"=== {title} — звіт ===");
            writer.WriteLine($"Клініка: {_clinic.Name}");
            writer.WriteLine(
                $"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");

            writer.WriteLine("----------------------------------------");
        }

        private void WriteSummary(StreamWriter writer, int count)
        {
            writer.WriteLine("----------------------------------------");
            writer.WriteLine($"Всього: {count}");
        }

        public string ExportPatients()
        {
            string path = CreateReportPath("patients.txt");
            Patient[] patients = _clinic.Patients.GetAll();

            using (StreamWriter writer =
                new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteHeader(writer, "Пацієнти");

                for (int i = 0; i < patients.Length; i++)
                {
                    writer.WriteLine($"{i + 1,3}. {patients[i]}");
                }

                WriteSummary(writer, patients.Length);
            }

            return path;
        }

        public string ExportAppointments()
        {
            string path = CreateReportPath("appointments.txt");
            Appointment[] appointments = _clinic.Appointments.GetAll();

            using (StreamWriter writer =
                new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteHeader(writer, "Записи на прийом");

                for (int i = 0; i < appointments.Length; i++)
                {
                    writer.WriteLine($"{i + 1,3}. {appointments[i]}");
                }

                WriteSummary(writer, appointments.Length);
            }

            return path;
        }

        public string ExportBilling()
        {
            string path = CreateReportPath("billing.txt");
            IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();

            using (StreamWriter writer =
                new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteHeader(writer, "Неоплачені записи");

                for (int i = 0; i < unpaid.Length; i++)
                {
                    writer.WriteLine(
                        $"{i + 1,3}. {unpaid[i]} | " +
                        $"Оплачено: {unpaid[i].IsPaid}");
                }

                WriteSummary(writer, unpaid.Length);

                writer.WriteLine(
                    $"Загальний борг: {_clinic.Billing.GetTotalDebt():F2} грн");
            }

            return path;
        }

        public string ExportTreatmentPlans()
        {
            string path = CreateReportPath("treatment_plans.txt");
            TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();

            using (StreamWriter writer =
                new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteHeader(writer, "Плани лікування");

                for (int i = 0; i < plans.Length; i++)
                {
                    writer.WriteLine($"{i + 1,3}. {plans[i]}");
                }

                WriteSummary(writer, plans.Length);
            }

            return path;
        }

        public string[] ExportAll()
        {
            return new string[]
            {
                ExportPatients(),
                ExportAppointments(),
                ExportBilling(),
                ExportTreatmentPlans()
            };
        }
    }
}