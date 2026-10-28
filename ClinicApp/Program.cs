using System;
using System.Globalization;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Managers;
using ClinicApp.Interfaces;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

RunGrowableTest();

var clinic = new Clinic("Здоров'я Плюс");

try
{
    clinic.Patients.Add(new Patient("Іван", "Петренко", DateTime.Today.AddYears(-41), BloodType.APositive, "0501234567"));
    clinic.Patients.Add(new Patient("Олена", "Коваль", DateTime.Today.AddYears(-33), BloodType.BNegative, "0672345678"));
    clinic.Patients.Add(new Patient("Максим", "Бойко", DateTime.Today.AddYears(-16), BloodType.OPositive, "0933456789"));

    clinic.Doctors.Add(new Doctor("Олександр", "Мельник", Speciality.Cardiology, "DOC-10293", "0509876543") { Schedule = new WorkSchedule(8, 16) });
    clinic.Doctors.Add(new Doctor("Наталія", "Шевченко", Speciality.Pediatrics, "DOC-20394", "0673344556") { Schedule = new WorkSchedule(9, 18) });
    clinic.Doctors.Add(new Doctor("Сергій", "Бондар", Speciality.Neurology, "DOC-44812", "0671122334") { Schedule = new WorkSchedule(12, 20) });

    // 1. ВСТАВИТИ ТУТ: Використання нових методів для створення різних типів прийомів
    clinic.Appointments.Book(1, 1, DateTime.Today.AddHours(10), 30);
    clinic.Appointments.BookUrgent(2, 2, DateTime.Today.AddHours(14), "біль у грудях", 30);
    clinic.Appointments.BookSpecialist(3, 1, DateTime.Today.AddDays(1).AddHours(11), "кардіологія", 45);

    SeedMedicalRecords(clinic);
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка ініціалізації: {ex.Message}");
}

// 2. ВСТАВИТИ ТУТ: Демонстрація new проти override (Задача 3) з відповідями на запитання
Console.WriteLine("\n== new vs override ==");
UrgentAppointment urgentObj = new UrgentAppointment(1, 1, DateTime.Today.AddDays(2), "тест");
Appointment apptRef = urgentObj;

Console.WriteLine($"Через Appointment:       {apptRef.GetDescription()} | пріоритет {apptRef.GetPriority()}");
Console.WriteLine($"Через UrgentAppointment: {urgentObj.GetDescription()} | пріоритет {urgentObj.GetPriority()}");
/*
Відповіді на запитання:
1. Чому GetDescription() через змінну типу Appointment дає опис термінового прийому?
   - Тому що метод GetDescription() є віртуальним (virtual у базовому класі та override у підкласі), тому виклик розв'язується динамічно за реальним типом об'єкта в пам'яті (runtime dispatch).
2. Чому GetPriority() через ту саму змінну повертає 3, хоча об'єкт — UrgentAppointment?
   - Тому що метод GetPriority() у базовому класі не є віртуальним, а в підкласі він оголошений з модифікатором new (приховування). Виклик зв'язується статично за типом змінної (Appointment).
3. Що треба змінити в Appointment і UrgentAppointment, щоб GetPriority() теж працював поліморфно?
   - Потрібно в базовому класі Appointment позначити GetPriority() як virtual, а в підкласі UrgentAppointment замінити new на override.
*/

bool appRunning = true;
while (appRunning)
{
    Console.WriteLine("\n┌────────────────────────────────────────────────┐");
    Console.WriteLine("│                МЕДИЧНА КЛІНІКА                 │");
    Console.WriteLine("├────────────────────────────────────────────────┤");
    Console.WriteLine("│ 1. Пацієнти    — реєстрація, пошук             │");
    Console.WriteLine("│ 2. Лікарі      — персонал, розклад             │");
    Console.WriteLine("│ 3. Записи      — прийоми, скасування           │");
    Console.WriteLine("│ 4. Медична картка — діагнози, рецепти          │");
    Console.WriteLine("│ 5. Рахунки     — оплата, борги                 │");
    Console.WriteLine("│ 6. Звіт        — загальна статистика           │");
    Console.WriteLine("│ 0. Вийти                                       │");
    Console.WriteLine("└────────────────────────────────────────────────┘");
    Console.Write("Оберіть розділ: ");

    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            RunPatientsMenu(clinic);
            break;
        case "2":
            RunDoctorsMenu(clinic);
            break;
        case "3":
            RunAppointmentsMenu(clinic);
            break;
        case "4":
            RunMedicalRecordsMenu(clinic);
            break;
        case "5":
            RunBillingMenu(clinic);
            break;
        case "6":
            clinic.GenerateReport();
            break;
        case "0":
            appRunning = false;
            break;
        default:
            Console.WriteLine("Невірний вибір.");
            break;
    }
}

static void SeedMedicalRecords(Clinic clinic)
{
    DateTime today = DateTime.Today;

    clinic.MedicalRecords.Add(new Diagnosis(1, 1, today.AddDays(-30), "I10", "Гіпертонічна хвороба", isChronic: true));
    clinic.MedicalRecords.Add(new Diagnosis(1, 1, today.AddDays(-5), "J06.9", "Гостра респіраторна інфекція", isChronic: false));
    clinic.MedicalRecords.Add(new LabResult(1, 1, today.AddDays(-5), "Гемоглобін", 145, "г/л", "120-160", isNormal: true));
    clinic.MedicalRecords.Add(new LabResult(1, 1, today.AddDays(-3), "Холестерин", 6.2, "ммоль/л", "< 5.2", isNormal: false));
    clinic.MedicalRecords.Add(new Prescription(1, 1, today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));

    clinic.MedicalRecords.Add(new Prescription(2, 2, today.AddDays(-40), "Амоксицилін", "500 мг", 10, "3 рази на добу"));
    clinic.MedicalRecords.Add(new Diagnosis(2, 2, today.AddDays(-10), "M54.5", "Біль у попереку", isChronic: false));

    clinic.MedicalRecords.Add(new LabResult(3, 1, today.AddDays(-2), "Глюкоза", 4.8, "ммоль/л", "3.3-5.5", isNormal: true));
}

static void RunBillingMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n— Рахунки ————————————————");
        Console.WriteLine("1. Борги пацієнта");
        Console.WriteLine("2. Всі неоплачені записи");
        Console.WriteLine("3. Оплатити запис");
        Console.WriteLine("4. Загальний борг клініки");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть пункт: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                if (!TryParseInt("ID пацієнта: ", out int pId)) break;
                var unpaid = clinic.Billing.GetUnpaidByPatient(pId);
                decimal debt = clinic.Billing.GetPatientDebt(pId);

                if (unpaid.Length == 0)
                {
                    Console.WriteLine("Немає неоплачених записів.");
                }
                else
                {
                    Console.WriteLine($"Неоплачені записи пацієнта #{pId}:");
                    clinic.Billing.DisplayUnpaid(unpaid);
                }
                Console.WriteLine($"Борг: {debt.ToString(CultureInfo.InvariantCulture)} грн");
                break;

            case "2":
                var allUnpaid = clinic.Billing.GetAllUnpaid();
                clinic.Billing.DisplayUnpaid(allUnpaid);
                break;

            case "3":
                if (!TryParseInt("ID запису для оплати: ", out int aptId)) break;
                bool success = clinic.Billing.PayAppointment(aptId);
                if (success)
                {
                    Console.WriteLine($"Запис [{aptId}] оплачено.");
                }
                else
                {
                    Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                }
                break;

            case "4":
                decimal totalDebt = clinic.Billing.GetTotalDebt();
                Console.WriteLine($"Загальний борг клініки: {totalDebt.ToString(CultureInfo.InvariantCulture)} грн");
                break;

            case "0":
                back = true;
                break;

            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

static void RunMedicalRecordsMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n=== Меню: Медична картка ===");
        Console.WriteLine("1 — Картка пацієнта (зведення)");
        Console.WriteLine("2 — Усі записи пацієнта");
        Console.WriteLine("3 — Додати діагноз");
        Console.WriteLine("4 — Додати аналіз");
        Console.WriteLine("5 — Додати рецепт");
        Console.WriteLine("6 — Записи лікаря");
        Console.WriteLine("0 — Назад");
        Console.Write("Оберіть дію: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                clinic.Patients.DisplayAll();
                if (!TryParseInt("Введіть ID пацієнта: ", out int pId1)) break;
                if (clinic.Patients.FindById(pId1) == null)
                {
                    Console.WriteLine("Пацієнта з таким ID не знайдено.");
                    break;
                }
                clinic.MedicalRecords.DisplayPatientSummary(pId1);
                break;

            case "2":
                clinic.Patients.DisplayAll();
                if (!TryParseInt("Введіть ID пацієнта: ", out int pId2)) break;
                if (clinic.Patients.FindById(pId2) == null)
                {
                    Console.WriteLine("Пацієнта з таким ID не знайдено.");
                    break;
                }
                var patientRecords = clinic.MedicalRecords.GetByPatient(pId2);
                clinic.MedicalRecords.DisplayList(patientRecords);
                break;

            case "3":
                AddDiagnosisUI(clinic);
                break;

            case "4":
                AddLabResultUI(clinic);
                break;

            case "5":
                AddPrescriptionUI(clinic);
                break;

            case "6":
                clinic.Doctors.DisplayAll();
                if (!TryParseInt("Введіть ID лікаря: ", out int dId6)) break;
                if (clinic.Doctors.FindById(dId6) == null)
                {
                    Console.WriteLine("Лікаря з таким ID не знайдено.");
                    break;
                }
                var doctorRecords = clinic.MedicalRecords.GetByDoctor(dId6);
                clinic.MedicalRecords.DisplayList(doctorRecords);
                break;

            case "0":
                back = true;
                break;

            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

static void AddDiagnosisUI(Clinic clinic)
{
    Console.WriteLine("\n--- Додавання діагнозу ---");
    clinic.Patients.DisplayAll();
    if (!TryParseInt("ID пацієнта: ", out int pId)) return;
    if (clinic.Patients.FindById(pId) == null)
    {
        Console.WriteLine("Помилка: пацієнта з таким ID не знайдено.");
        return;
    }

    clinic.Doctors.DisplayAll();
    if (!TryParseInt("ID лікаря: ", out int dId)) return;
    if (clinic.Doctors.FindById(dId) == null)
    {
        Console.WriteLine("Помилка: лікаря з таким ID не знайдено.");
        return;
    }

    Console.Write("Код діагнозу (напр. J06.9): ");
    string code = Console.ReadLine()!;

    Console.Write("Опис: ");
    string description = Console.ReadLine()!;

    Console.Write("Хронічне? (1=так, 0=ні): ");
    bool isChronic = Console.ReadLine() == "1";

    try
    {
        var diagnosis = new Diagnosis(pId, dId, DateTime.Today, code, description, isChronic);
        clinic.MedicalRecords.Add(diagnosis);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
}

static void AddLabResultUI(Clinic clinic)
{
    Console.WriteLine("\n--- Додавання результату аналізу ---");
    clinic.Patients.DisplayAll();
    if (!TryParseInt("ID пацієнта: ", out int pId)) return;
    if (clinic.Patients.FindById(pId) == null)
    {
        Console.WriteLine("Помилка: пацієнта з таким ID не знайдено.");
        return;
    }

    clinic.Doctors.DisplayAll();
    if (!TryParseInt("ID лікаря: ", out int dId)) return;
    if (clinic.Doctors.FindById(dId) == null)
    {
        Console.WriteLine("Помилка: лікаря з таким ID не знайдено.");
        return;
    }

    Console.Write("Назва аналізу: ");
    string testName = Console.ReadLine()!;

    Console.Write("Значення (число з крапкою): ");
    if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
    {
        Console.WriteLine("Помилка: некоректне значення аналізу.");
        return;
    }

    Console.Write("Одиниці виміру: ");
    string unit = Console.ReadLine()!;

    Console.Write("Норма (текст, напр. 4.0-9.0): ");
    string range = Console.ReadLine()!;

    Console.Write("В нормі? (1=так, 0=ні): ");
    bool isNormal = Console.ReadLine() == "1";

    try
    {
        var labResult = new LabResult(pId, dId, DateTime.Today, testName, val, unit, range, isNormal);
        clinic.MedicalRecords.Add(labResult);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
}

static void AddPrescriptionUI(Clinic clinic)
{
    Console.WriteLine("\n--- Додавання рецепта ---");
    clinic.Patients.DisplayAll();
    if (!TryParseInt("ID пацієнта: ", out int pId)) return;
    if (clinic.Patients.FindById(pId) == null)
    {
        Console.WriteLine("Помилка: пацієнта з таким ID не знайдено.");
        return;
    }

    clinic.Doctors.DisplayAll();
    if (!TryParseInt("ID лікаря: ", out int dId)) return;
    if (clinic.Doctors.FindById(dId) == null)
    {
        Console.WriteLine("Помилка: лікаря з таким ID не знайдено.");
        return;
    }

    Console.Write("Препарат: ");
    string medication = Console.ReadLine()!;

    Console.Write("Дозування (напр. 10 мг): ");
    string dosage = Console.ReadLine()!;

    if (!TryParseInt("Кількість днів: ", out int durationDays)) return;

    Console.Write("Інструкція (Enter — пропустити): ");
    string instructions = Console.ReadLine()!;

    try
    {
        var prescription = new Prescription(pId, dId, DateTime.Today, medication, dosage, durationDays, instructions);
        clinic.MedicalRecords.Add(prescription);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
}

static bool TryParseInt(string prompt, out int result)
{
    Console.Write(prompt);
    string? input = Console.ReadLine();
    if (!int.TryParse(input, out result))
    {
        Console.WriteLine("Помилка: некоректне число, повернення у меню.");
        return false;
    }
    return true;
}

static void RunGrowableTest()
{
    Console.WriteLine("=== Демонстрація Task 8: GrowablePatientManager ===");
    var dynamicManager = new GrowablePatientManager();

    Console.WriteLine($"Початковий стан: Count = {dynamicManager.Count}, Capacity = {dynamicManager.Capacity}\n");

    for (int i = 1; i <= 20; i++)
    {
        var p = new Patient($"Тест{i}", $"Пацієнт{i}", new DateTime(1995, 5, 10), BloodType.APositive, "0500000000");
        dynamicManager.Add(p);
        Console.WriteLine($"Додано #{i}: Count = {dynamicManager.Count}, Capacity = {dynamicManager.Capacity}");
    }

    Console.WriteLine("\nПеревірка пошуку:");
    var found = dynamicManager.FindById(5);
    Console.WriteLine($"Пошук ID 5: {(found != null ? found.FullName : "не знайдено")}");

    var notFound = dynamicManager.FindById(999);
    Console.WriteLine($"Пошук ID 999: {(notFound != null ? notFound.FullName : "не знайдено")}");
    Console.WriteLine("===================================================\n");
}

static void RunPatientsMenu(Clinic clinic)
{
    bool exit = false;
    while (!exit)
    {
        Console.WriteLine("\n=== Меню: Пацієнти ===");
        Console.WriteLine("1. Показати всіх пацієнтів");
        Console.WriteLine("2. Додати пацієнта");
        Console.WriteLine("3. Знайти пацієнта");
        Console.WriteLine("4. Видалити пацієнта за ID");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть дію: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                clinic.Patients.DisplayAll();
                break;
            case "2":
                try
                {
                    Console.Write("Введіть ім'я: ");
                    string firstName = Console.ReadLine()!;

                    Console.Write("Введіть прізвище: ");
                    string lastName = Console.ReadLine()!;

                    Console.Write("Введіть дату народження (рррр-мм-дд): ");
                    DateTime dob = DateTime.Parse(Console.ReadLine()!);

                    Console.WriteLine("Оберіть групу крові:");
                    Console.WriteLine("0 - Unknown, 1 - APositive, 2 - ANegative, 3 - BPositive, 4 - BNegative, 5 - ABPositive, 6 - ABNegative, 7 - OPositive, 8 - ONegative");
                    Console.Write("Номер групи крові: ");
                    int bloodChoice = int.TryParse(Console.ReadLine(), out int bc) ? bc : 0;
                    BloodType blood = (BloodType)bloodChoice;

                    Console.Write("Введіть телефон: ");
                    string phone = Console.ReadLine()!;

                    clinic.Patients.Add(new Patient(firstName, lastName, dob, blood, phone));
                    Console.WriteLine("Пацієнта успішно додано!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "3":
                Console.Write("Введіть ім'я або прізвище: ");
                var found = clinic.Patients.FindByName(Console.ReadLine()!);
                foreach (var p in found) Console.WriteLine(p);
                break;
            case "4":
                Console.Write("Введіть ID: ");
                if (int.TryParse(Console.ReadLine(), out int deleteId)) clinic.Patients.Remove(deleteId);
                break;
            case "5":
                clinic.Patients.DisplayStats();
                break;
            case "0":
                exit = true;
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

static void RunDoctorsMenu(Clinic clinic)
{
    bool exit = false;
    while (!exit)
    {
        Console.WriteLine("\n=== Меню: Лікарі ===");
        Console.WriteLine("1. Показати всіх лікарів");
        Console.WriteLine("2. Додати лікаря");
        Console.WriteLine("3. Знайти за спеціальністю");
        Console.WriteLine("4. Видалити лікаря за ID");
        Console.WriteLine("5. Вільні години лікаря");
        Console.WriteLine("6. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть дію: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                clinic.Doctors.DisplayAll();
                break;
            case "2":
                try
                {
                    Console.Write("Введіть ім'я: ");
                    string firstName = Console.ReadLine()!;

                    Console.Write("Введіть прізвище: ");
                    string lastName = Console.ReadLine()!;

                    Console.WriteLine("Оберіть спеціальність:");
                    Console.WriteLine("0 - General, 1 - Cardiology, 2 - Neurology, 3 - Pediatrics, 4 - Surgery, 5 - Orthopedics, 6 - Dermatology, 7 - Emergency");
                    Console.Write("Номер спеціальності: ");
                    int specChoice = int.TryParse(Console.ReadLine(), out int sc) ? sc : 0;
                    Speciality speciality = (Speciality)specChoice;

                    Console.Write("Введіть номер ліцензії: ");
                    string license = Console.ReadLine()!;

                    Console.Write("Введіть телефон: ");
                    string phone = Console.ReadLine()!;

                    int startHour = 8;
                    Console.Write("Година початку (за замовчуванням 8): ");
                    string? startInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(startInput))
                    {
                        startHour = int.Parse(startInput);
                    }

                    int endHour = 17;
                    Console.Write("Година кінця (за замовчуванням 17): ");
                    string? endInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(endInput))
                    {
                        endHour = int.Parse(endInput);
                    }

                    var schedule = new WorkSchedule(startHour, endHour);
                    var doctor = new Doctor(firstName, lastName, speciality, license, phone)
                    {
                        Schedule = schedule
                    };

                    clinic.Doctors.Add(doctor);
                    Console.WriteLine("Лікаря успішно додано!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "3":
                Console.Write("Введіть спеціальність (назву або частину): ");
                var found = clinic.Doctors.FindBySpeciality(Console.ReadLine()!);
                foreach (var d in found) Console.WriteLine(d);
                break;
            case "4":
                Console.Write("Введіть ID лікаря: ");
                if (int.TryParse(Console.ReadLine(), out int deleteId)) clinic.Doctors.Remove(deleteId);
                break;
            case "5":
                Console.Write("ID лікаря (ціле): ");
                if (int.TryParse(Console.ReadLine(), out int docId))
                {
                    var doctor = clinic.Doctors.FindById(docId);
                    if (doctor == null)
                    {
                        Console.WriteLine("Лікаря не знайдено");
                        break;
                    }
                    Console.Write("Дата (dd.MM.yyyy): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        Console.Write("Скільки слотів (ціле): ");
                        if (int.TryParse(Console.ReadLine(), out int slotCount))
                        {
                            try
                            {
                                ISchedulable schedulableDoctor = doctor;
                                DateTime[] slots = schedulableDoctor.GetAvailableSlots(date, slotCount);
                                foreach (var slot in slots)
                                {
                                    Console.Write($"{slot:HH:mm} ");
                                }
                                Console.WriteLine();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Помилка: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Помилка: невірна кількість слотів.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Помилка: невірний формат дати.");
                    }
                }
                break;
            case "6":
                clinic.Doctors.DisplayStats();
                break;
            case "0":
                exit = true;
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

static void RunAppointmentsMenu(Clinic clinic)
{
    bool exit = false;
    while (!exit)
    {
        Console.WriteLine("\n=== Меню: Записи ===");
        Console.WriteLine("1. Показати майбутні записи");
        Console.WriteLine("2. Створити новий запис");
        Console.WriteLine("3. Скасувати запис");
        Console.WriteLine("4. Завершити запис");
        Console.WriteLine("5. Знайти записи пацієнта");
        Console.WriteLine("6. Знайти записи за датою");
        Console.WriteLine("8. Скасувати всі записи пацієнта");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть дію: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.WriteLine("\n--- Майбутні записи ---");
                clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                break;
            case "2":
                try
                {
                    Console.WriteLine("\nДоступні пацієнти:");
                    clinic.Patients.DisplayAll();

                    Console.WriteLine("\nДоступні лікарі:");
                    clinic.Doctors.DisplayAll();

                    Console.Write("\nВведіть ID пацієнта: ");
                    int patientId = int.Parse(Console.ReadLine()!);

                    Console.Write("Введіть ID лікаря: ");
                    int doctorId = int.Parse(Console.ReadLine()!);

                    Console.Write("Введіть дату та час (рррр-мм-дд ГГ:хх, напр. 2026-10-15 14:00): ");
                    DateTime dt = DateTime.Parse(Console.ReadLine()!);

                    Console.Write("Тривалість у хвилинах (за замовчуванням 30): ");
                    string? durInput = Console.ReadLine();
                    int duration = int.TryParse(durInput, out int dur) ? dur : 30;

                    clinic.Appointments.Book(patientId, doctorId, dt, duration);
                    Console.WriteLine("Запис успішно створено!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "3":
                Console.Write("Введіть ID запису для скасування: ");
                if (int.TryParse(Console.ReadLine(), out int cId))
                {
                    Console.Write("Причина скасування: ");
                    clinic.Appointments.Cancel(cId, Console.ReadLine()!);
                }
                break;
            case "4":
                Console.Write("Введіть ID запису для завершення: ");
                if (int.TryParse(Console.ReadLine(), out int compId))
                {
                    clinic.Appointments.Complete(compId);
                }
                break;
            case "5":
                Console.Write("Введіть ID пацієнта: ");
                if (int.TryParse(Console.ReadLine(), out int patSearchId))
                {
                    clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(patSearchId));
                }
                break;
            case "6":
                Console.Write("Введіть дату (рррр-мм-дд): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                {
                    clinic.Appointments.DisplayList(clinic.Appointments.GetByDate(searchDate));
                }
                break;
            case "8":
                Console.Write("ID пацієнта (ціле): ");
                if (int.TryParse(Console.ReadLine(), out int pId))
                {
                    Console.Write("Причина скасування (Enter — без причини): ");
                    string reason = Console.ReadLine() ?? string.Empty;
                    var patientApts = clinic.Appointments.GetByPatient(pId);
                    ICancellable[] cancellables = patientApts;
                    int cancelledCount = AppointmentManager.CancelAll(cancellables, reason);
                    Console.WriteLine($"Скасовано записів: {cancelledCount}");
                }
                break;
            case "0":
                exit = true;
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}