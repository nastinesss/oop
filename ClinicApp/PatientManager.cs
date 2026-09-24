using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    internal class PatientManager
    {
        private const int MaxPatients = 100;
        private readonly Patient[] _patients = new Patient[MaxPatients];
        private int _count = 0;

        public int Count => _count;

        public void Add(Patient patient)
        {
            if (_count >= MaxPatients)
            {
                Console.WriteLine("Помилка: досягнуто ліміт пацієнтів (100).");
                return;
            }

            _patients[_count] = patient;
            _count++;
            Console.WriteLine($"Пацієнта '{patient.FullName}' успішно додано з ID: {patient.Id}.");
        }

        public Patient? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i].Id == id)
                {
                    return _patients[i];
                }
            }
            return null;
        }

        public Patient[] FindByName(string query)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i].FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    _patients[i].LastName.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    matchCount++;
                }
            }

            Patient[] result = new Patient[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i].FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    _patients[i].LastName.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    result[index++] = _patients[i];
                }
            }

            return result;
        }

        public bool Remove(int id)
        {
            int foundIndex = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i].Id == id)
                {
                    foundIndex = i;
                    break;
                }
            }

            if (foundIndex == -1)
            {
                return false;
            }

            for (int i = foundIndex; i < _count - 1; i++)
            {
                _patients[i] = _patients[i + 1];
            }

            _patients[_count - 1] = null!;
            _count--;
            return true;
        }

        public void DisplayAll()
        {
            if (_count == 0)
            {
                Console.WriteLine("Список пацієнтів порожній.");
                return;
            }

            Console.WriteLine("\n--- Список пацієнтів ---");
            for (int i = 0; i < _count; i++)
            {
                Console.WriteLine(_patients[i]);
            }
        }

        public void DisplayStats()
        {
            if (_count == 0)
            {
                Console.WriteLine("Немає даних для формування статистики.");
                return;
            }

            int totalAge = 0;
            int adultCount = 0;
            Patient youngest = _patients[0];
            Patient oldest = _patients[0];

            for (int i = 0; i < _count; i++)
            {
                var p = _patients[i];
                totalAge += p.Age;

                if (p.IsAdult)
                {
                    adultCount++;
                }

                if (p.Age < youngest.Age)
                {
                    youngest = p;
                }

                if (p.Age > oldest.Age)
                {
                    oldest = p;
                }
            }

            double avgAge = (double)totalAge / _count;

            Console.WriteLine("\n--- Статистика пацієнтів ---");
            Console.WriteLine($"Загальна кількість: {_count}");
            Console.WriteLine($"Середній вік:       {avgAge:F1} р.");
            Console.WriteLine($"Наймолодший:        {youngest.FullName} ({youngest.Age} р.)");
            Console.WriteLine($"Найстарший:         {oldest.FullName} ({oldest.Age} р.)");
            Console.WriteLine($"Кількість дорослих: {adultCount} з {_count}");
        }
    }
}
