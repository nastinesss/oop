using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Interfaces
{
    interface IPayable
    {
        decimal GetCost();
        bool IsPaid { get; }
        void MarkPaid();
    }
}
