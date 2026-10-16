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

        public Diagnosis[] GetDiagnoses(int patientId)
        {
            int count = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is Diagnosis)
                {
                    count++;
                }
            }

            Diagnosis[] result = new Diagnosis[count];
            int index = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is Diagnosis d)
                {
                    result[index++] = d;
                }
            }

            return result;
        }

        public LabResult[] GetLabResults(int patientId)
        {
            int count = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is LabResult)
                {
                    count++;
                }
            }

            LabResult[] result = new LabResult[count];
            int index = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is LabResult lr)
                {
                    result[index++] = lr;
                }
            }

            return result;
        }

        public Prescription[] GetPrescriptions(int patientId)
        {
            int count = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is Prescription)
                {
                    count++;
                }
            }

            Prescription[] result = new Prescription[count];
            int index = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is Prescription p)
                {
                    result[index++] = p;
                }
            }

            return result;
        }

        public Diagnosis[] GetChronicDiagnoses(int patientId)
        {
            int count = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
                {
                    count++;
                }
            }

            Diagnosis[] result = new Diagnosis[count];
            int index = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
                {
                    result[index++] = d;
                }
            }

            return result;
        }

        public Prescription[] GetActivePrescriptions(int patientId)
        {
            int count = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
                {
                    count++;
                }
            }

            Prescription[] result = new Prescription[count];
            int index = 0;
            for (int i = 0; i < Count; i++)
            {
                if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
                {
                    result[index++] = p;
                }
            }

            return result;
        }

        public void DisplayPatientSummary(int patientId)
        {
            MedicalRecord[] patientRecords = GetByPatient(patientId);

            if (patientRecords.Length == 0)
            {
                Console.WriteLine($"Записи для пацієнта #{patientId} відсутні.");
                return;
            }

            int diagCount = 0;
            int labCount = 0;
            int prescCount = 0;

            for (int i = 0; i < patientRecords.Length; i++)
            {
                if (patientRecords[i] is Diagnosis) diagCount++;
                else if (patientRecords[i] is LabResult) labCount++;
                else if (patientRecords[i] is Prescription) prescCount++;
            }

            Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
            Console.WriteLine($"Всього записів: {patientRecords.Length} (діагнозів: {diagCount}, аналізів: {labCount}, рецептів: {prescCount})");

            Diagnosis[] chronic = GetChronicDiagnoses(patientId);
            Console.WriteLine($"Хронічні діагнози ({chronic.Length}):");
            if (chronic.Length == 0)
            {
                Console.WriteLine("  (відсутні)");
            }
            else
            {
                for (int i = 0; i < chronic.Length; i++)
                {
                    Console.WriteLine($"  {chronic[i]}");
                }
            }

            Prescription[] activePrescriptions = GetActivePrescriptions(patientId);
            Console.WriteLine($"Активні рецепти ({activePrescriptions.Length}):");
            if (activePrescriptions.Length == 0)
            {
                Console.WriteLine("  (відсутні)");
            }
            else
            {
                for (int i = 0; i < activePrescriptions.Length; i++)
                {
                    Console.WriteLine($"  {activePrescriptions[i]} | до {activePrescriptions[i].ExpiresAt:dd.MM.yyyy}");
                }
            }
        }
    }
}
