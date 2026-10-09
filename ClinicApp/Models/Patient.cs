using ClinicApp.Enums;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class Patient
    {
        private static int _nextId = 1;

        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private DateTime _dateOfBirth;
        private string _phone = string.Empty;

        public int Id { get; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ім'я не може бути порожнім або містити лише пробіли.", nameof(FirstName));
                }
                string trimmed = value.Trim();
                if (trimmed.Length > 50)
                {
                    throw new ArgumentException("Ім'я не може перевищувати 50 символів.", nameof(FirstName));
                }
                _firstName = trimmed;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Прізвище не може бути порожнім або містити лише пробіли.", nameof(LastName));
                }
                string trimmed = value.Trim();
                if (trimmed.Length > 50)
                {
                    throw new ArgumentException("Прізвище не може перевищувати 50 символів.", nameof(LastName));
                }
                _lastName = trimmed;
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value.Date > DateTime.Today)
                {
                    throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата народження не може бути в майбутньому.");
                }
                if (value.Year < 1900)
                {
                    throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата народження не може бути раніше 1900 року.");
                }
                _dateOfBirth = value;
            }
        }

        public BloodType BloodType { get; set; }

        public string Phone
        {
            get => _phone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Номер телефону не може бути порожнім.", nameof(Phone));
                }
                string trimmed = value.Trim();
                bool valid = (trimmed.Length == 10) || (trimmed.StartsWith("+38") && trimmed.Length == 13);
                if (valid)
                {
                    int startIndex = trimmed.StartsWith("+") ? 1 : 0;
                    for (int i = startIndex; i < trimmed.Length; i++)
                    {
                        if (!char.IsDigit(trimmed[i]))
                        {
                            valid = false;
                            break;
                        }
                    }
                }
                if (!valid)
                {
                    throw new ArgumentException("Номер телефону повинен містити тільки цифри (10 цифр або +38 та 10 цифр).", nameof(Phone));
                }
                _phone = trimmed;
            }
        }

        public string Email { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        public bool IsAdult => Age >= 18;

        public Patient()
            : this("Невідомий", "Пацієнт")
        {
        }

        public Patient(string firstName, string lastName)
            : this(firstName, lastName, new DateTime(2000, 1, 1), BloodType.Unknown, "0500000000", "")
        {
        }

        public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone, string email = "")
        {
            Id = _nextId++;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            BloodType = bloodType;
            Phone = phone;
            Email = email;
        }

        public string GetAgeCategory()
        {
            if (Age < 18)
            {
                return "дитина";
            }
            if (Age < 60)
            {
                return "дорослий";
            }
            return "літній";
        }

        public override string ToString()
        {
            return $"[{Id}] {FullName} | Вік: {ClinicFormatter.FormatAge(Age)} ({GetAgeCategory()}) | Кров: {ClinicFormatter.FormatBloodType(BloodType)} | Тел: {ClinicFormatter.FormatPhone(Phone)}";
        }
    }
}