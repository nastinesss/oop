using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Managers;

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

    clinic.Appointments.Book(1, 1, DateTime.Today.AddHours(10), 30);
    clinic.Appointments.Book(2, 2, DateTime.Today.AddHours(14), 45);
    clinic.Appointments.Book(3, 1, DateTime.Today.AddDays(1).AddHours(11), 30);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine(ex.Message);
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

bool appRunning = true;
while (appRunning)
{
    Console.WriteLine($"\n===== КЛІНІКА: \"{clinic.Name}\" =====");
    Console.WriteLine("1. Пацієнти");
    Console.WriteLine("2. Лікарі");
    Console.WriteLine("3. Записи");
    Console.WriteLine("4. Розклад на сьогодні");
    Console.WriteLine("5. Розклад на довільну дату");
    Console.WriteLine("6. Згенерувати загальний звіт");
    Console.WriteLine("0. Вихід");
    Console.Write("Оберіть пункт: ");

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
            clinic.DisplaySchedule(DateTime.Today);
            break;
        case "5":
            Console.Write("Введіть дату (рррр-мм-дд): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
            {
                clinic.DisplaySchedule(dt);
            }
            else
            {
                Console.WriteLine("Некоректний формат дати.");
            }
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
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
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
        Console.WriteLine("5. Статистика");
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
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
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
                    int pId = int.Parse(Console.ReadLine()!);

                    Console.Write("Введіть ID лікаря: ");
                    int dId = int.Parse(Console.ReadLine()!);

                    Console.Write("Введіть дату та час (рррр-мм-дд ГГ:хх, напр. 2026-10-15 14:00): ");
                    DateTime dt = DateTime.Parse(Console.ReadLine()!);

                    Console.Write("Тривалість у хвилинах (за замовчуванням 30): ");
                    string? durInput = Console.ReadLine();
                    int duration = int.TryParse(durInput, out int dur) ? dur : 30;

                    clinic.Appointments.Book(pId, dId, dt, duration);
                    Console.WriteLine("Запис успішно створено!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
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
            case "0":
                exit = true;
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}