using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Comparators
{
    public class DoctorStatsByRevenue : IComparer<DoctorStats>
    {
        public int Compare(DoctorStats? x, DoctorStats? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            // Спадання: більша виручка вище
            return y.TotalRevenue.CompareTo(x.TotalRevenue);
        }
    }
}