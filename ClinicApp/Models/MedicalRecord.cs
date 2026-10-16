using ClinicApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Models
{
    public abstract class MedicalRecord
    {
        private static int _nextId = 1;

        public int Id { get; }
        public int PatientId { get; }
        public int DoctorId { get; }
        public DateTime Date { get; }
        public string Notes { get; set; } = string.Empty;

        protected MedicalRecord(int patientId, int doctorId, DateTime date)
        {
            PatientId = ClinicValidator.ValidatePositiveNumber(patientId, nameof(patientId));
            DoctorId = ClinicValidator.ValidatePositiveNumber(doctorId, nameof(doctorId));
            Date = date;

            Id = _nextId++;
        }

        public abstract string GetSummary();

        public virtual string GetRecordType()
        {
            return "Медичний запис";
        }

        public virtual bool IsActive()
        {
            return Date >= DateTime.Today.AddMonths(-6);
        }

        public override string ToString()
        {
            string dateStr = Date.ToString("dd.MM.yyyy");
            string result = $"[{Id}] {GetRecordType()} | {dateStr} | {GetSummary()}";

            if (!string.IsNullOrWhiteSpace(Notes))
            {
                result += $" | {Notes}";
            }

            return result;
        }
    }
}
