using ClinicApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Models
{
    public class Prescription : MedicalRecord
    {
        private string _medicationName = string.Empty;
        private string _dosage = string.Empty;
        private int _durationDays;

        public string MedicationName
        {
            get => _medicationName;
            set => _medicationName = ClinicValidator.ValidateName(value, nameof(MedicationName));
        }

        public string Dosage
        {
            get => _dosage;
            set => _dosage = ClinicValidator.ValidateName(value, nameof(Dosage));
        }

        public int DurationDays
        {
            get => _durationDays;
            set => _durationDays = ClinicValidator.ValidatePositiveNumber(value, nameof(DurationDays));
        }

        public string Instructions { get; set; } = string.Empty;

        public DateTime ExpiresAt => Date.AddDays(DurationDays);

        public Prescription(
            int patientId,
            int doctorId,
            DateTime date,
            string medicationName,
            string dosage,
            int durationDays,
            string instructions = "")
            : base(patientId, doctorId, date)
        {
            MedicationName = medicationName;
            Dosage = dosage;
            DurationDays = durationDays;
            Instructions = instructions;
        }

        public override string GetRecordType()
        {
            return "Рецепт";
        }

        public override string GetSummary()
        {
            string summary = $"{MedicationName} {Dosage} \u00d7 {DurationDays} днів";

            if (!string.IsNullOrWhiteSpace(Instructions))
            {
                summary += $" ({Instructions})";
            }

            return summary;
        }

        public override bool IsActive()
        {
            return ExpiresAt.Date >= DateTime.Today;
        }
    }
}
