using ClinicApp.Enums;
using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Managers
{
    public class DoctorManager
    {
        private const int MaxDoctors = 50;
        private readonly Doctor[] _doctors = new Doctor[MaxDoctors];
        private int _count = 0;

        public int Count => _count;

        public Doctor? this[int index]
        {
            get
            {
                if (index >= 0 && index < _count)
                {
                    return _doctors[index];
                }
                return null;
            }
        }

        public void Add(Doctor doctor)
        {
            if (_count >= MaxDoctors)
            {
                Console.WriteLine("Помилка: досягнуто ліміт лікарів (50).");
                return;
            }

            _doctors[_count] = doctor;
            _count++;
            Console.WriteLine($"Лікаря '{doctor.FullName}' успішно додано з ID: {doctor.Id}.");
        }

        public Doctor? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Id == id)
                {
                    return _doctors[i];
                }
            }
            return null;
        }

        public bool TryFindById(int id, out Doctor? doctor)
        {
            doctor = FindById(id);
            return doctor != null;
        }

        public Doctor[] FindBySpeciality(string speciality)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Speciality.ToString().Contains(speciality, StringComparison.OrdinalIgnoreCase))
                {
                    matchCount++;
                }
            }

            Doctor[] result = new Doctor[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Speciality.ToString().Contains(speciality, StringComparison.OrdinalIgnoreCase))
                {
                    result[index++] = _doctors[i];
                }
            }

            return result;
        }

        public Doctor[] FindBySpeciality(Speciality speciality)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Speciality == speciality)
                {
                    matchCount++;
                }
            }

            Doctor[] result = new Doctor[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Speciality == speciality)
                {
                    result[index++] = _doctors[i];
                }
            }

            return result;
        }

        public Doctor[] GetAll()
        {
            Doctor[] copy = new Doctor[_count];
            for (int i = 0; i < _count; i++)
            {
                copy[i] = _doctors[i];
            }
            return copy;
        }

        public bool Remove(int id)
        {
            int foundIndex = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Id == id)
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
                _doctors[i] = _doctors[i + 1];
            }

            _doctors[_count - 1] = null!;
            _count--;
            return true;
        }

        public void DisplayAll()
        {
            if (_count == 0)
            {
                Console.WriteLine("Список лікарів порожній.");
                return;
            }

            Console.WriteLine("\n--- Список лікарів ---");
            for (int i = 0; i < _count; i++)
            {
                Console.WriteLine(_doctors[i]);
            }
        }

        public void DisplayStats()
        {
            if (_count == 0)
            {
                Console.WriteLine("Немає даних для формування статистики.");
                return;
            }

            int availableNowCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].IsAvailableNow)
                {
                    availableNowCount++;
                }
            }

            Console.WriteLine("\n--- Статистика лікарів ---");
            Console.WriteLine($"Загальна кількість: {_count}");
            Console.WriteLine($"Доступні зараз:     {availableNowCount}");
            Console.WriteLine("Спеціальності:");

            for (int i = 0; i < _count; i++)
            {
                bool alreadySeen = false;
                for (int j = 0; j < i; j++)
                {
                    if (_doctors[i].Speciality == _doctors[j].Speciality)
                    {
                        alreadySeen = true;
                        break;
                    }
                }

                if (!alreadySeen)
                {
                    int specCount = 0;
                    for (int k = 0; k < _count; k++)
                    {
                        if (_doctors[i].Speciality == _doctors[k].Speciality)
                        {
                            specCount++;
                        }
                    }
                    Console.WriteLine($"  - {_doctors[i].Speciality}: {specCount}");
                }
            }
        }
    }
}