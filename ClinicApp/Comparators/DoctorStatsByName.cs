using System;
using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Comparators
{
    public class DoctorStatsByName : IComparer<DoctorStats>
    {
        public int Compare(DoctorStats? x, DoctorStats? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            // Зростання за ім'ям (А -> Я)
            return string.Compare(x.FullName, y.FullName, StringComparison.CurrentCulture);
        }
    }
}