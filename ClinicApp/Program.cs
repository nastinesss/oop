using ClinicApp;

var patientManager = new PatientManager();
patientManager.Add(new Patient("Іван", "Петренко", DateTime.Today.AddYears(-41), "A+", "0501234567"));
patientManager.Add(new Patient("Олена", "Коваль", DateTime.Today.AddYears(-33), "B-", "0672345678"));
patientManager.Add(new Patient("Максим", "Бойко", DateTime.Today.AddYears(-16), "0+", "0933456789"));

var doctorManager = new DoctorManager();
doctorManager.Add(new Doctor("Олександр", "Мельник", "Кардіологія", "DOC-10293", "0509876543") { WorkStartHour = 8, WorkEndHour = 16 });
doctorManager.Add(new Doctor("Наталія", "Шевченко", "Педіатрія", "DOC-20394", "0673344556") { WorkStartHour = 9, WorkEndHour = 18 });
doctorManager.Add(new Doctor("Сергій", "Бондар", "Неврологія", "DOC-44812", "0671122334") { WorkStartHour = 12, WorkEndHour = 20 });

var appointmentManager = new AppointmentManager(patientManager, doctorManager);

appointmentManager.Book(1, 1, DateTime.Today.AddHours(10), 30);
appointmentManager.Book(2, 2, DateTime.Today.AddHours(14), 45);
appointmentManager.Book(3, 1, DateTime.Today.AddDays(1).AddHours(11), 30);

bool appRunning = true;
while (appRunning)
{
    Console.WriteLine("\n===== ГОЛОВНЕ МЕНЮ КЛІНІКИ =====");
    Console.WriteLine("1. Меню: Пацієнти");
    Console.WriteLine("2. Меню: Лікарі");
    Console.WriteLine("3. Меню: Записи");
    Console.WriteLine("0. Вихід");
    Console.Write("Оберіть пункт: ");

    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            RunPatientsMenu(patientManager);
            break;
        case "2":
            RunDoctorsMenu(doctorManager);
            break;
        case "3":
            RunAppointmentsMenu(appointmentManager, patientManager, doctorManager);
            break;
        case "0":
            appRunning = false;
            break;
        default:
            Console.WriteLine("Невірний вибір.");
            break;
    }
}


static void RunAppointmentsMenu(AppointmentManager aptManager, PatientManager patManager, DoctorManager docManager)
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
                aptManager.DisplayList(aptManager.GetUpcoming());
                break;

            case "2":
                Console.WriteLine("\nДоступні пацієнти:");
                patManager.DisplayAll();

                Console.WriteLine("\nДоступні лікарі:");
                docManager.DisplayAll();

                Console.Write("\nВведіть ID пацієнта: ");
                int pId = int.Parse(Console.ReadLine()!);

                Console.Write("Введіть ID лікаря: ");
                int dId = int.Parse(Console.ReadLine()!);

                Console.Write("Введіть дату та час (рррр-мм-дд ГГ:хх, напр. 2026-10-15 14:00): ");
                DateTime dt = DateTime.Parse(Console.ReadLine()!);

                Console.Write("Тривалість у хвилинах (за замовчуванням 30): ");
                string? durInput = Console.ReadLine();
                int duration = int.TryParse(durInput, out int dur) ? dur : 30;

                aptManager.Book(pId, dId, dt, duration);
                break;

            case "3":
                Console.Write("Введіть ID запису для скасування: ");
                if (int.TryParse(Console.ReadLine(), out int cId))
                {
                    Console.Write("Причина скасування: ");
                    string reason = Console.ReadLine()!;
                    aptManager.Cancel(cId, reason);
                }
                break;

            case "4":
                Console.Write("Введіть ID запису для завершення: ");
                if (int.TryParse(Console.ReadLine(), out int compId))
                {
                    aptManager.Complete(compId);
                }
                break;

            case "5":
                Console.Write("Введіть ID пацієнта: ");
                if (int.TryParse(Console.ReadLine(), out int patSearchId))
                {
                    aptManager.DisplayList(aptManager.GetByPatient(patSearchId));
                }
                break;

            case "6":
                Console.Write("Введіть дату (рррр-мм-дд): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                {
                    aptManager.DisplayList(aptManager.GetByDate(searchDate));
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

static void RunDoctorsMenu(DoctorManager manager)
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
                manager.DisplayAll();
                break;
            case "2":
                Console.Write("Введіть ім'я: ");
                string firstName = Console.ReadLine()!;
                Console.Write("Введіть прізвище: ");
                string lastName = Console.ReadLine()!;
                Console.Write("Введіть спеціальність: ");
                string speciality = Console.ReadLine()!;
                Console.Write("Введіть номер ліцензії: ");
                string license = Console.ReadLine()!;
                Console.Write("Введіть телефон: ");
                string phone = Console.ReadLine()!;

                int startHour = 8;
                Console.Write("Година початку (за замовчуванням 8): ");
                if (int.TryParse(Console.ReadLine(), out int sh) && sh >= 0 && sh <= 23) startHour = sh;

                int endHour = 17;
                Console.Write("Година кінця (за замовчуванням 17): ");
                if (int.TryParse(Console.ReadLine(), out int eh) && eh >= 0 && eh <= 23) endHour = eh;

                manager.Add(new Doctor(firstName, lastName, speciality, license, phone) { WorkStartHour = startHour, WorkEndHour = endHour });
                break;
            case "3":
                Console.Write("Введіть спеціальність: ");
                var found = manager.FindBySpeciality(Console.ReadLine()!);
                foreach (var d in found) Console.WriteLine(d);
                break;
            case "4":
                Console.Write("Введіть ID лікаря: ");
                if (int.TryParse(Console.ReadLine(), out int deleteId)) manager.Remove(deleteId);
                break;
            case "5":
                manager.DisplayStats();
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

static void RunPatientsMenu(PatientManager manager)
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
                manager.DisplayAll();
                break;
            case "2":
                Console.Write("Введіть ім'я: ");
                string firstName = Console.ReadLine()!;
                Console.Write("Введіть прізвище: ");
                string lastName = Console.ReadLine()!;
                Console.Write("Введіть дату народження (рррр-мм-дд): ");
                DateTime dob = DateTime.Parse(Console.ReadLine()!);
                Console.Write("Введіть групу крові: ");
                string blood = Console.ReadLine()!;
                Console.Write("Введіть телефон: ");
                string phone = Console.ReadLine()!;

                manager.Add(new Patient(firstName, lastName, dob, blood, phone));
                break;
            case "3":
                Console.Write("Введіть ім'я або прізвище: ");
                var found = manager.FindByName(Console.ReadLine()!);
                foreach (var p in found) Console.WriteLine(p);
                break;
            case "4":
                Console.Write("Введіть ID: ");
                if (int.TryParse(Console.ReadLine(), out int deleteId)) manager.Remove(deleteId);
                break;
            case "5":
                manager.DisplayStats();
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