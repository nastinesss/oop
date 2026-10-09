using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Utils
{
    internal class ClinicValidator
    {
        public static string ValidateName(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{fieldName} не може бути порожнім.", fieldName);
            }

            string trimmed = value.Trim();
            if (trimmed.Length > 50)
            {
                throw new ArgumentException($"{fieldName} не може перевищувати 50 символів.", fieldName);
            }

            return trimmed;
        }

        public static DateTime ValidateDateOfBirth(DateTime dob)
        {
            if (dob.Date > DateTime.Today)
            {
                throw new ArgumentOutOfRangeException(nameof(dob), "Дата народження не може бути в майбутньому.");
            }

            if (dob.Date < DateTime.Today.AddYears(-130))
            {
                throw new ArgumentOutOfRangeException(nameof(dob), "Вік пацієнта не може перевищувати 130 років.");
            }

            return dob;
        }

        public static string ValidatePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return string.Empty;
            }

            string trimmed = phone.Trim();
            foreach (char c in trimmed)
            {
                if (!char.IsDigit(c) && c != '+' && c != '-' && c != '(' && c != ')' && c != ' ')
                {
                    throw new ArgumentException("Номер телефону містить недопустимі символи.", nameof(phone));
                }
            }

            return trimmed;
        }

        public static string ValidateLicenseNumber(string? license)
        {
            if (string.IsNullOrWhiteSpace(license))
            {
                throw new ArgumentException("Номер ліцензії не може бути порожнім.", nameof(license));
            }

            return license.Trim();
        }

        public static void ValidateWorkSchedule(int start, int end)
        {
            if (start < 0 || start > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Година початку має бути від 0 до 23.");
            }

            if (end < 1 || end > 24)
            {
                throw new ArgumentOutOfRangeException(nameof(end), "Година закінчення має бути від 1 до 24.");
            }

            if (start >= end)
            {
                throw new ArgumentException("Година початку зміни має бути меншою за годину закінчення.");
            }
        }

        public static DateTime ValidateAppointmentDate(DateTime date)
        {
            if (date < DateTime.Now.AddMinutes(-5))
            {
                throw new ArgumentException("Час запису не може бути в минулому.", nameof(date));
            }

            return date;
        }

        public static int ValidateDuration(int durationMinutes)
        {
            if (durationMinutes <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(durationMinutes), "Тривалість має бути більшою за 0 хвилин.");
            }

            return durationMinutes;
        }
    }
}
