using ClinicApp.Enums;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class Patient
    {
        private static int _nextId = 1;

        // Приватні поля (інкапсуляція)
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
                    throw new ArgumentException("Ім'я не може бути порожнім.", nameof(FirstName));

                _firstName = value.Trim();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Прізвище не може бути порожнім.", nameof(LastName));

                _lastName = value.Trim();
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value.Date > DateTime.Today)
                    throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата народження не може бути в майбутньому.");

                _dateOfBirth = value;
            }
        }

        public BloodType BloodType { get; set; }

        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim() ?? string.Empty;
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
            : this(firstName, lastName, new DateTime(2000, 1, 1), BloodType.Unknown, "0000000000", "")
        {
        }

        public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone, string email = "")
        {
            Id = _nextId++;
            // Присвоєння через властивості змушує відпрацьовувати сеттери з валідацією
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