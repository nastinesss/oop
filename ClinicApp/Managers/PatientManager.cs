using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Events;
using System;
using System.Collections.Generic;

namespace ClinicApp.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients = new List<Patient>();
        public event EventHandler<PatientEventArgs>? PatientAdded;

        public int Count => _patients.Count;

        public void Add(Patient patient)
        {
            _patients.Add(patient);

            PatientAdded?.Invoke(
                this,
                new PatientEventArgs(patient.Id, patient.FullName));
        }

        public Patient? FindById(int id)
        {
            foreach (var patient in _patients)
            {
                if (patient.Id == id)
                    return patient;
            }
            return null;
        }

        public Patient[] FindByName(string name)
        {
            List<Patient> matches = new List<Patient>();
            foreach (var patient in _patients)
            {
                if (patient.FullName.Contains(name, System.StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(patient);
                }
            }
            return matches.ToArray();
        }

        public Patient[] FindByBloodType(BloodType bloodType)
        {
            List<Patient> matches = new List<Patient>();
            foreach (var patient in _patients)
            {
                if (patient.BloodType == bloodType)
                {
                    matches.Add(patient);
                }
            }
            return matches.ToArray();
        }

        public bool Remove(int id)
        {
            var patient = FindById(id);
            if (patient != null)
            {
                return _patients.Remove(patient);
            }
            return false;
        }

        public Patient[] GetAll()
        {
            return _patients.ToArray();
        }

        public void DisplayAll()
        {
            if (_patients.Count == 0)
            {
                Console.WriteLine("Список пацієнтів порожній.");
                return;
            }

            Console.WriteLine("\n--- Список пацієнтів ---");
            foreach (var patient in _patients)
            {
                Console.WriteLine(patient);
            }
        }
        public void DisplayStats()
        {
            Console.WriteLine($"\n--- Статистика пацієнтів ---");
            Console.WriteLine($"Загальна кількість пацієнтів: {_patients.Count}");
        }
    }
}
