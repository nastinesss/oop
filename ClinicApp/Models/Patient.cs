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
        private string _email = string.Empty;

        public int Id { get; }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = ClinicValidator.ValidateName(value, nameof(FirstName));
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = ClinicValidator.ValidateName(value, nameof(LastName));
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = ClinicValidator.ValidateDateOfBirth(value);
        }

        public BloodType BloodType { get; set; }

        public string Phone
        {
            get => _phone;
            set => _phone = ClinicValidator.ValidatePhone(value);
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _email = string.Empty;
                }
                else
                {
                    _email = ClinicValidator.ValidateEmail(value);
                }
            }
        }

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