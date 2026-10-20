using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Interfaces
{
    interface ICancellable
    {
        bool IsCancelled { get; }
        string CancellationReason { get; }
        bool Cancel(string reason = "");
    }
}
