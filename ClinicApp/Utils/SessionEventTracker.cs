using System;
using System.IO;
using System.Text;
using ClinicApp.Events;

namespace ClinicApp.Utils
{
    internal class SessionEventTracker
    {
        private readonly Clinic _clinic;

        public int PatientsAdded { get; private set; }
        public int AppointmentsBooked { get; private set; }
        public int UrgentBooked { get; private set; }
        public int AppointmentsCancelled { get; private set; }
        public int AppointmentsCompleted { get; private set; }
        public int PaymentsReceived { get; private set; }
        public int PlansCompleted { get; private set; }

        public SessionEventTracker(Clinic clinic)
        {
            _clinic = clinic;
        }

        public void OnPatientAdded(object? sender, PatientEventArgs e)
        {
            PatientsAdded++;
        }

        public void OnAppointmentBooked(
            object? sender,
            AppointmentEventArgs e)
        {
            AppointmentsBooked++;
        }

        public void OnUrgentAppointmentBooked(
            object? sender,
            AppointmentEventArgs e)
        {
            UrgentBooked++;
        }

        public void OnAppointmentCancelled(
            object? sender,
            AppointmentEventArgs e)
        {
            AppointmentsCancelled++;

            if (!_clinic.WaitingRoom.IsEmpty)
            {
                var nextPatient = _clinic.WaitingRoom.Peek();

                Console.WriteLine(
                    $"[ЧЕРГА] Слот звільнився. " +
                    $"Наступний: {nextPatient.FullName}");
            }
        }

        public void OnAppointmentCompleted(
            object? sender,
            AppointmentEventArgs e)
        {
            AppointmentsCompleted++;
        }

        public void OnPaymentReceived(
            object? sender,
            PaymentEventArgs e)
        {
            PaymentsReceived++;
        }

        public void OnPlanCompleted(
            object? sender,
            TreatmentPlanEventArgs e)
        {
            PlansCompleted++;
        }

        private string[] GetSummaryLines()
        {
            return new string[]
            {
                "=== Підсумок сесії ===",
                $"Сформовано: {DateTime.Now:dd.MM.yyyy HH:mm:ss}",
                $"Пацієнтів додано:    {PatientsAdded}",
                $"Записів створено:    {AppointmentsBooked} " +
                    $"(термінових: {UrgentBooked})",
                $"Скасовано:           {AppointmentsCancelled}",
                $"Завершено:           {AppointmentsCompleted}",
                $"Оплат:               {PaymentsReceived}",
                $"Планів завершено:    {PlansCompleted}"
            };
        }

        public void PrintSummary()
        {
            Console.WriteLine();

            foreach (string line in GetSummaryLines())
            {
                Console.WriteLine(line);
            }
        }

        public void SaveSummary(string path = "session_summary.txt")
        {
            using (StreamWriter writer =
                new StreamWriter(path, false, Encoding.UTF8))
            {
                foreach (string line in GetSummaryLines())
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}