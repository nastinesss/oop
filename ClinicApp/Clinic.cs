using ClinicApp.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    internal class Clinic
    {
        public string Name { get; set; }
        public PatientManager Patients { get; }
        public DoctorManager Doctors { get; }
        public AppointmentManager Appointments { get; }

        public MedicalRecordManager MedicalRecords { get; }

        public BillingManager Billing { get; }

        public Clinic(string name)
        {
            Name = name;
            Patients = new PatientManager();
            Doctors = new DoctorManager();
            Appointments = new AppointmentManager(Patients, Doctors);
            MedicalRecords = new MedicalRecordManager();

            Billing = new BillingManager(Appointments);
        }

        public void DisplaySchedule(DateTime date)
        {
            Console.WriteLine($"\n=== Розклад клініки \"{Name}\" на {date:yyyy-MM-dd} ===");
            var dailyAppointments = Appointments.GetByDate(date);
            Appointments.DisplayList(dailyAppointments);
        }

        public void GenerateReport()
        {
            var upcoming = Appointments.GetUpcoming();
            var allDoctors = Doctors.GetAll();

            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ ЗВІТ ДІЯЛЬНОСТІ КЛІНІКИ: {Name,-27}║");
            Console.WriteLine("╠══════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Зареєстровано пацієнтів: {Patients.Count,-27}║");
            Console.WriteLine($"║ Працює лікарів:          {Doctors.Count,-27}║");
            Console.WriteLine($"║ Майбутніх записів:       {upcoming.Length,-27}║");
            Console.WriteLine($"║ Всього медичних записів: {MedicalRecords.Count,-27}║");
            Console.WriteLine("╠══════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Навантаження лікарів (майбутні прийоми):             ║");

            if (allDoctors.Length == 0)
            {
                Console.WriteLine("║  - Лікарів немає в базі                             ║");
            }
            else
            {
                for (int i = 0; i < allDoctors.Length; i++)
                {
                    var doc = allDoctors[i];
                    int aptCount = 0;

                    for (int j = 0; j < upcoming.Length; j++)
                    {
                        if (upcoming[j].DoctorId == doc.Id)
                        {
                            aptCount++;
                        }
                    }

                    string line = $"  - {doc.FullName} ({doc.Speciality}): {aptCount} запис(ів)";
                    if (line.Length > 52)
                    {
                        line = line.Substring(0, 49) + "...";
                    }
                    Console.WriteLine($"║ {line,-52} ║");
                }
            }

            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
        }
    }
}