using System;
using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private static readonly Regex PhoneRegex = new Regex(@"^(\+38)?\d{10}$", RegexOptions.Compiled);
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public static string ValidateName(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{fieldName} не може бути порожнім або містити лише пробіли.", fieldName);
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

        if (dob.Year < 1900)
        {
            throw new ArgumentOutOfRangeException(nameof(dob), "Дата народження не може бути раніше 1900 року.");
        }

        return dob;
    }

    public static string ValidatePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Номер телефону не може бути порожнім.", nameof(phone));
        }

        string trimmed = phone.Trim();
        if (!PhoneRegex.IsMatch(trimmed))
        {
            throw new ArgumentException("Номер телефону повинен містити 10 цифр (або у форматі +38 та 10 цифр).", nameof(phone));
        }

        return trimmed;
    }

    public static string ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return string.Empty;
        }

        string trimmed = email.Trim();
        if (!EmailRegex.IsMatch(trimmed))
        {
            throw new ArgumentException("Некоректний формат email.", nameof(email));
        }

        return trimmed;
    }

    public static string ValidateLicenseNumber(string? license)
    {
        if (string.IsNullOrWhiteSpace(license))
        {
            throw new ArgumentException("Номер ліцензії не може бути порожнім або містити лише пробіли.", nameof(license));
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
            throw new ArgumentOutOfRangeException(nameof(end), "Година завершення має бути від 1 до 24.");
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

    public static int ValidatePositiveNumber(int number, string paramName)
    {
        if (number <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "Значення має бути більшим за 0.");
        }

        return number;
    }
}