using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    internal class SessionManager
    {
        private readonly string _sessionPath;

        public SessionManager(string sessionPath = "session.dat")
        {
            _sessionPath = sessionPath;
        }

        public bool Exists()
        {
            return File.Exists(_sessionPath);
        }

        public void Save(Clinic clinic)
        {
            Patient[] patients = clinic.Patients.GetAll();

            // Перевіряємо сумісність із простим CSV до перезапису файлу.
            foreach (Patient patient in patients)
            {
                if (HasCsvSeparator(patient.FirstName) ||
                    HasCsvSeparator(patient.LastName) ||
                    HasCsvSeparator(patient.Phone))
                {
                    throw new InvalidOperationException(
                        "Не вдалося зберегти сесію: значення містить " +
                        "кому або перенесення рядка.");
                }
            }

            using (StreamWriter writer =
                new StreamWriter(_sessionPath, false, Encoding.UTF8))
            {
                writer.WriteLine("[PATIENTS]");

                foreach (Patient patient in patients)
                {
                    string date = patient.DateOfBirth.ToString(
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture);

                    writer.WriteLine(
                        $"{patient.FirstName}," +
                        $"{patient.LastName}," +
                        $"{date}," +
                        $"{patient.BloodType}," +
                        $"{patient.Phone}");
                }

                writer.WriteLine("[END]");
            }
        }

        public int Load(Clinic clinic)
        {
            if (!Exists())
            {
                return 0;
            }

            string[] lines = File.ReadAllLines(
                _sessionPath,
                Encoding.UTF8);

            string currentSection = "";
            int loaded = 0;

            foreach (string originalLine in lines)
            {
                string line = originalLine.Trim();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith("["))
                {
                    currentSection = line;

                    if (currentSection == "[END]")
                    {
                        break;
                    }

                    continue;
                }

                if (currentSection != "[PATIENTS]")
                {
                    continue;
                }

                try
                {
                    Patient patient = CsvImporter.ParsePatient(line);

                    if (AlreadyExists(clinic, patient))
                    {
                        continue;
                    }

                    clinic.Patients.Add(patient);
                    loaded++;
                }
                catch (Exception)
                {
                    // Пошкоджений рядок не зупиняє завантаження.
                    continue;
                }
            }

            return loaded;
        }

        private bool AlreadyExists(Clinic clinic, Patient candidate)
        {
            Patient[] patients = clinic.Patients.GetAll();

            foreach (Patient patient in patients)
            {
                if (string.Equals(
                        patient.FirstName,
                        candidate.FirstName,
                        StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(
                        patient.LastName,
                        candidate.LastName,
                        StringComparison.OrdinalIgnoreCase) &&
                    patient.DateOfBirth.Date ==
                        candidate.DateOfBirth.Date)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasCsvSeparator(string value)
        {
            return value.Contains(",") ||
                   value.Contains("\r") ||
                   value.Contains("\n");
        }
    }
}