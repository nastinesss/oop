using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    internal class GrowablePatientManager
    {
        private Patient[] _patients = new Patient[4];
        private int _count = 0;

        public int Count => _count;
        public int Capacity => _patients.Length;

        private void Resize()
        {
            int oldCapacity = _patients.Length;
            int newCapacity = oldCapacity * 2;

            Patient[] newArray = new Patient[newCapacity];
            for (int i = 0; i < _count; i++)
            {
                newArray[i] = _patients[i];
            }

            _patients = newArray;
            Console.WriteLine($"[GrowablePatientManager] Розширення: {oldCapacity} -> {newCapacity}");
        }

        public void Add(Patient patient)
        {
            if (_count == _patients.Length)
            {
                Resize();
            }

            _patients[_count] = patient;
            _count++;
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
            Console.WriteLine($"\n--- Список динамічного менеджера (Елементів: {Count}, Ємність: {Capacity}) ---");
            if (_count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                Console.WriteLine(_patients[i]);
            }
        }
    }
}
