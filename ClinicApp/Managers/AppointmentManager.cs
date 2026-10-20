using ClinicApp.Interfaces;
using ClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Managers
{
    public class AppointmentManager
    {
        private const int MaxAppointments = 500;
        private readonly Appointment[] _appointments = new Appointment[MaxAppointments];
        private int _count = 0;

        private readonly PatientManager _patients;
        private readonly DoctorManager _doctors;

        public int Count => _count;

        public Appointment? this[int index]
        {
            get
            {
                if (index >= 0 && index < _count)
                {
                    return _appointments[index];
                }
                return null;
            }
        }

        public AppointmentManager(PatientManager patients, DoctorManager doctors)
        {
            _patients = patients;
            _doctors = doctors;
        }

        private Appointment? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].Id == id)
                {
                    return _appointments[i];
                }
            }
            return null;
        }

        public Appointment[] GetAll()
        {
            Appointment[] result = new Appointment[_count];
            for (int i = 0; i < _count; i++)
            {
                result[i] = _appointments[i];
            }
            return result;
        }

        public static int CancelAll(ICancellable[] items, string reason = "")
        {
            int cancelledCount = 0;
            foreach (var item in items)
            {
                if (item != null && item.Cancel(reason))
                {
                    cancelledCount++;
                }
            }
            return cancelledCount;
        }

        public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            if (_count >= MaxAppointments)
            {
                Console.WriteLine("Помилка: досягнуто ліміт записів (500).");
                return false;
            }

            var patient = _patients.FindById(patientId);
            if (patient == null)
            {
                Console.WriteLine($"Помилка: пацієнта з ID {patientId} не існує.");
                return false;
            }

            var doctor = _doctors.FindById(doctorId);
            if (doctor == null)
            {
                Console.WriteLine($"Помилка: лікаря з ID {doctorId} не існує.");
                return false;
            }

            var apt = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
            _appointments[_count] = apt;
            _count++;

            Console.WriteLine($"Успішно створено запис №{apt.Id} для {patient.FullName} до лікаря {doctor.FullName}.");
            return true;
        }

        public bool Cancel(int id, string reason = "")
        {
            var apt = FindById(id);
            if (apt == null)
            {
                Console.WriteLine($"Запис №{id} не знайдено.");
                return false;
            }

            bool result = apt.Cancel(reason);
            if (result)
            {
                Console.WriteLine($"Запис №{id} успішно скасовано.");
            }
            else
            {
                Console.WriteLine($"Не вдалося скасувати запис №{id} (поточний статус: {apt.Status}).");
            }
            return result;
        }

        public bool Complete(int id)
        {
            var apt = FindById(id);
            if (apt == null)
            {
                Console.WriteLine($"Запис №{id} не знайдено.");
                return false;
            }

            bool result = apt.Complete();
            if (result)
            {
                Console.WriteLine($"Запис №{id} успішно завершено.");
            }
            else
            {
                Console.WriteLine($"Не вдалося завершити запис №{id} (поточний статус: {apt.Status}).");
            }
            return result;
        }

        public Appointment[] GetByPatient(int patientId)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].PatientId == patientId)
                {
                    matchCount++;
                }
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].PatientId == patientId)
                {
                    result[index++] = _appointments[i];
                }
            }
            return result;
        }

        public Appointment[] GetByDoctor(int doctorId)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].DoctorId == doctorId)
                {
                    matchCount++;
                }
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].DoctorId == doctorId)
                {
                    result[index++] = _appointments[i];
                }
            }
            return result;
        }

        public Appointment[] GetByDate(DateTime date)
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].ScheduledAt.Date == date.Date)
                {
                    matchCount++;
                }
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].ScheduledAt.Date == date.Date)
                {
                    result[index++] = _appointments[i];
                }
            }
            return result;
        }

        public Appointment[] GetByDate(int year, int month, int day)
        {
            return GetByDate(new DateTime(year, month, day));
        }

        public Appointment[] GetUpcoming()
        {
            int matchCount = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].IsUpcoming)
                {
                    matchCount++;
                }
            }

            Appointment[] result = new Appointment[matchCount];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].IsUpcoming)
                {
                    result[index++] = _appointments[i];
                }
            }
            return result;
        }

        public void DisplayAppointment(Appointment apt)
        {
            var patient = _patients.FindById(apt.PatientId);
            string patientName = (patient != null) ? patient.FullName : $"Пацієнт #{apt.PatientId}";

            var doctor = _doctors.FindById(apt.DoctorId);
            string doctorName = (doctor != null) ? doctor.FullName : $"Лікар #{apt.DoctorId}";

            string line = $"[{apt.Id}] {apt.ScheduledAt:yyyy-MM-dd HH:mm}–{apt.EndsAt:HH:mm} | {patientName} -> {doctorName} | Статус: {apt.Status}";
            if (apt.Notes.Length > 0)
            {
                line += $" | Примітка: {apt.Notes}";
            }
            Console.WriteLine(line);
        }

        public void DisplayList(Appointment[] list)
        {
            if (list.Length == 0)
            {
                Console.WriteLine("Записів не знайдено.");
                return;
            }

            for (int i = 0; i < list.Length; i++)
            {
                DisplayAppointment(list[i]);
            }
        }
    }
}