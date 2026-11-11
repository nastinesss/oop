using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Comparators
{
    public class PatientStatsByLastVisit : IComparer<PatientStats>
    {
        public int Compare(PatientStats? x, PatientStats? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            // Спадання: найновіший візит вище
            return y.LastVisitDate.CompareTo(x.LastVisitDate);
        }
    }
}