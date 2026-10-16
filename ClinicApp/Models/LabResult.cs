using ClinicApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Models
{
    public class LabResult : MedicalRecord
    {
        private string _testName = string.Empty;
        private string _unit = string.Empty;
        private string _referenceRange = string.Empty;

        public string TestName
        {
            get => _testName;
            set => _testName = ClinicValidator.ValidateName(value, nameof(TestName));
        }

        public double Value { get; set; }

        public string Unit
        {
            get => _unit;
            set => _unit = ClinicValidator.ValidateName(value, nameof(Unit));
        }

        public string ReferenceRange
        {
            get => _referenceRange;
            set => _referenceRange = ClinicValidator.ValidateName(value, nameof(ReferenceRange));
        }

        public bool IsNormal { get; set; }

        public LabResult(
            int patientId,
            int doctorId,
            DateTime date,
            string testName,
            double value,
            string unit,
            string referenceRange,
            bool isNormal)
            : base(patientId, doctorId, date)
        {
            TestName = testName;
            Value = value;
            Unit = unit;
            ReferenceRange = referenceRange;
            IsNormal = isNormal;
        }

        public override string GetRecordType()
        {
            return "Аналіз";
        }

        public override string GetSummary()
        {
            string summary = $"{TestName}: {Value} {Unit} (норма: {ReferenceRange})";

            if (!IsNormal)
            {
                summary += " \u2206 поза нормою";
            }

            return summary;
        }
    }
}