using ClinicApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Models
{
    public class Diagnosis : MedicalRecord
    {
        private string _diagnosisCode = string.Empty;
        private string _description = string.Empty;

        public string DiagnosisCode
        {
            get => _diagnosisCode;
            set => _diagnosisCode = ClinicValidator.ValidateName(value, nameof(DiagnosisCode));
        }

        public string Description
        {
            get => _description;
            set => _description = ClinicValidator.ValidateName(value, nameof(Description));
        }

        public bool IsChronic { get; set; }

        public Diagnosis(
            int patientId,
            int doctorId,
            DateTime date,
            string diagnosisCode,
            string description,
            bool isChronic = false)
            : base(patientId, doctorId, date)
        {
            DiagnosisCode = diagnosisCode;
            Description = description;
            IsChronic = isChronic;
        }

        public override string GetRecordType()
        {
            return "Діагноз";
        }

        public override string GetSummary()
        {
            string summary = $"{DiagnosisCode}: {Description}";
            if (IsChronic)
            {
                summary += " [хронічне]";
            }
            return summary;
        }
    }
}
