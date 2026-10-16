using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Managers
{
    public class MedicalRecordManager
    {
        private readonly MedicalRecord[] _records = new MedicalRecord[1000];

        public int Count { get; private set; } = 0;

        public MedicalRecord? this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    return null;
                }
                return _records[index];
            }
        }

        public void Add(MedicalRecord record)
        {
            if (record == null)
            {
                return;
            }

            if (Count >= _records.Length)
            {
                Console.WriteLine("Помилка: досягнуто ліміт записів (1000).");
                return;
            }

            _records[Count] = record;
            Count++;
            Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
        }

        public MedicalRecord? FindById(int id)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].Id == id)
                {
                    return _records[i];
                }
            }
            return null;
        }

        public MedicalRecord[] GetByPatient(int patientId)
        {
            int matches = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId)
                {
                    matches++;
                }
            }

            MedicalRecord[] result = new MedicalRecord[matches];
            int index = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId)
                {
                    result[index++] = _records[i];
                }
            }

            return result;
        }

        public MedicalRecord[] GetByDoctor(int doctorId)
        {
            int matches = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].DoctorId == doctorId)
                {
                    matches++;
                }
            }

            MedicalRecord[] result = new MedicalRecord[matches];
            int index = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].DoctorId == doctorId)
                {
                    result[index++] = _records[i];
                }
            }

            return result;
        }

        public void DisplayAll()
        {
            if (Count == 0)
            {
                Console.WriteLine("Записи відсутні.");
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                Console.WriteLine(_records[i]);
            }
        }

        public void DisplayList(MedicalRecord[] list)
        {
            if (list == null || list.Length == 0)
            {
                Console.WriteLine("Записи відсутні.");
                return;
            }

            for (int i = 0; i < list.Length; i++)
            {
                Console.WriteLine(list[i]);
            }
        }
    }
}
