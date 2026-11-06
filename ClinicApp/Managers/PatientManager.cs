using ClinicApp.Enums;
using ClinicApp.Models;
using System.Collections.Generic;

namespace ClinicApp.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients = new List<Patient>();

        public int Count => _patients.Count;

        public void Add(Patient patient)
        {
            _patients.Add(patient);
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
    }
}