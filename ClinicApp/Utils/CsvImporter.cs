using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    internal static class CsvImporter
    {
        public static ImportResult ImportPatients(
            Clinic clinic,
            string filePath)
        {
            ImportResult result = new ImportResult();

            if (!File.Exists(filePath))
            {
                result.AddError(0, "Файл не знайдено.");
                return result;
            }

            string[] lines;

            try
            {
                lines = File.ReadAllLines(filePath, Encoding.UTF8);
            }
            catch (IOException ex)
            {
                result.AddError(0, $"Не вдалося прочитати файл: {ex.Message}");
                return result;
            }
            catch (UnauthorizedAccessException ex)
            {
                result.AddError(0, $"Немає доступу до файлу: {ex.Message}");
                return result;
            }

            // Перший рядок — заголовок CSV.
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                try
                {
                    Patient patient = ParsePatient(lines[i]);

                    clinic.Patients.Add(patient);
                    result.AddSuccess();
                }
                catch (Exception ex)
                {
                    result.AddError(i + 1, ex.Message);
                }
            }

            return result;
        }

        // Також використовується SessionManager у завданні 4.
        internal static Patient ParsePatient(string line)
        {
            string[] parts = line.Split(',');

            if (parts.Length != 5)
            {
                throw new FormatException(
                    $"Рядок має {parts.Length} поле(ів) замість 5.");
            }

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }

            if (string.IsNullOrWhiteSpace(parts[0]))
            {
                throw new ArgumentException("Ім'я не може бути порожнім.");
            }

            if (string.IsNullOrWhiteSpace(parts[1]))
            {
                throw new ArgumentException(
                    "Прізвище не може бути порожнім.");
            }

            DateTime dateOfBirth = DateTime.ParseExact(
                parts[2],
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture);

            BloodType bloodType = (BloodType)Enum.Parse(
                typeof(BloodType),
                parts[3]);

            // Enum.Parse може прийняти число, якого немає в enum.
            if (!Enum.IsDefined(typeof(BloodType), bloodType))
            {
                throw new ArgumentException("Невідома група крові.");
            }

            return new Patient(
                parts[0],
                parts[1],
                dateOfBirth,
                bloodType,
                parts[4]);
        }
    }
}