using ClinicApp.Enums;
using ClinicApp.Utils;
using System;

namespace ClinicApp.Models
{
    public class Doctor
    {
        private static int _nextId = 1;

        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _licenseNumber = string.Empty;
        private string _phone = string.Empty;

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

        public Speciality Speciality { get; set; }

        public string LicenseNumber
        {
            get => _licenseNumber;
            set => _licenseNumber = ClinicValidator.ValidateLicenseNumber(value);
        }

        public string Phone
        {
            get => _phone;
            set => _phone = ClinicValidator.ValidatePhone(value);
        }

        public WorkSchedule Schedule { get; set; } = new WorkSchedule(8, 17);

        public string FullName => $"{FirstName} {LastName}";
        public int WorkingHoursPerDay => Schedule.HoursPerDay;
        public string WorkSchedule => Schedule.Display;
        public bool IsAvailableNow => Schedule.IsNow;

        public Doctor()
            : this("Черговий", "Лікар", Speciality.General)
        {
        }

        public Doctor(string firstName, string lastName, Speciality speciality)
            : this(firstName, lastName, speciality, "UA-00000", "0000000000")
        {
        }

        public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
        {
            Id = _nextId++;
            FirstName = firstName;
            LastName = lastName;
            Speciality = speciality;
            LicenseNumber = licenseNumber;
            Phone = phone;
            Schedule = new WorkSchedule(8, 17);
        }

        public bool CanAcceptAt(int hour)
        {
            return Schedule.Contains(hour);
        }

        public override string ToString()
        {
            string status = IsAvailableNow ? "доступний" : "не в робочий час";
            return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | Графік: {WorkSchedule} | Ліцензія: {LicenseNumber} | Тел: {ClinicFormatter.FormatPhone(Phone)} | Статус: {status}";
        }
    }
}