using ClinicApp.Interfaces;
using ClinicApp.Managers;
using System;
using System.Collections.Generic;

namespace ClinicApp.Managers
{
    public class BillingManager
    {
        private readonly AppointmentManager _appointmentsManager;

        public BillingManager(AppointmentManager appointmentsManager)
        {
            _appointmentsManager = appointmentsManager;
        }

        public IPayable[] GetAllUnpaid()
        {
            var all = _appointmentsManager.GetAll();
            List<IPayable> unpaidList = new List<IPayable>();

            foreach (var apt in all)
            {
                if (apt is IPayable payable && !payable.IsPaid)
                {
                    if (apt is ICancellable cancellable && cancellable.IsCancelled)
                    {
                        continue;
                    }
                    unpaidList.Add(payable);
                }
            }

            return unpaidList.ToArray();
        }

        public IPayable[] GetUnpaidByPatient(int patientId)
        {
            var all = _appointmentsManager.GetByPatient(patientId);
            List<IPayable> unpaidList = new List<IPayable>();

            foreach (var apt in all)
            {
                if (apt is IPayable payable && !payable.IsPaid)
                {
                    if (apt is ICancellable cancellable && cancellable.IsCancelled)
                    {
                        continue;
                    }
                    unpaidList.Add(payable);
                }
            }

            return unpaidList.ToArray();
        }

        public decimal GetTotalDebt()
        {
            var unpaid = GetAllUnpaid();
            decimal totalDebt = 0;

            foreach (var item in unpaid)
            {
                totalDebt += item.GetCost();
            }

            return totalDebt;
        }

        public decimal GetPatientDebt(int patientId)
        {
            var unpaid = GetUnpaidByPatient(patientId);
            decimal patientDebt = 0;

            foreach (var item in unpaid)
            {
                patientDebt += item.GetCost();
            }

            return patientDebt;
        }

        public bool PayAppointment(int appointmentId)
        {
            var all = _appointmentsManager.GetAll();
            foreach (var apt in all)
            {
                if (apt.Id == appointmentId)
                {
                    if (apt.IsPaid || (apt is ICancellable c && c.IsCancelled))
                    {
                        return false;
                    }

                    apt.MarkPaid();
                    return true;
                }
            }
            return false;
        }

        public void DisplayUnpaid(IPayable[] items)
        {
            if (items == null || items.Length == 0)
            {
                Console.WriteLine("Немає неоплачених записів.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine($"Запис | Вартість: {item.GetCost():F2} грн | Оплачено: {item.IsPaid}");
            }
        }
    }
}