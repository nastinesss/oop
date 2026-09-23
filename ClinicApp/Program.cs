using ClinicApp;

var patientManager = new PatientManager();
patientManager.Add(new Patient("Іван", "Петренко", DateTime.Today.AddYears(-41), "A+", "0501234567"));
patientManager.Add(new Patient("Олена", "Коваль", DateTime.Today.AddYears(-33), "B-", "0672345678"));
patientManager.Add(new Patient("Максим", "Бойко", DateTime.Today.AddYears(-16), "0+", "0933456789"));

var doctorManager = new DoctorManager();
doctorManager.Add(new Doctor("Олександр", "Мельник", "Кардіологія", "DOC-10293", "0509876543") { WorkStartHour = 8, WorkEndHour = 16 });
doctorManager.Add(new Doctor("Наталія", "Шевченко", "Педіатрія", "DOC-20394", "0673344556") { WorkStartHour = 9, WorkEndHour = 18 });
doctorManager.Add(new Doctor("Сергій", "Бондар", "Неврологія", "DOC-44812", "0671122334") { WorkStartHour = 12, WorkEndHour = 20 });
doctorManager.Add(new Doctor("Ольга", "Ковальчук", "Кардіологія", "DOC-55123", "0935566778") { WorkStartHour = 8, WorkEndHour = 14 });

bool appRunning = true;
while (appRunning)
{
    Console.WriteLine("\n===== ГОЛОВНЕ МЕНЮ КЛІНІКИ =====");
    Console.WriteLine("1. Меню: Пацієнти");
    Console.WriteLine("2. Меню: Лікарі");
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
        case "0":
            appRunning = false;
            break;
        default:
            Console.WriteLine("Невірний вибір.");
            break;
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
                string? startInput = Console.ReadLine();
                if (int.TryParse(startInput, out int sh) && sh >= 0 && sh <= 23)
                {
                    startHour = sh;
                }

                int endHour = 17;
                Console.Write("Година кінця (за замовчуванням 17): ");
                string? endInput = Console.ReadLine();
                if (int.TryParse(endInput, out int eh) && eh >= 0 && eh <= 23)
                {
                    endHour = eh;
                }

                var doc = new Doctor(firstName, lastName, speciality, license, phone)
                {
                    WorkStartHour = startHour,
                    WorkEndHour = endHour
                };
                manager.Add(doc);
                break;

            case "3":
                Console.Write("Введіть спеціальність для пошуку: ");
                string query = Console.ReadLine()!;
                var found = manager.FindBySpeciality(query);
                if (found.Length == 0)
                {
                    Console.WriteLine("Лікарів з такою спеціальністю не знайдено.");
                }
                else
                {
                    Console.WriteLine($"Знайдено ({found.Length}):");
                    foreach (var d in found)
                    {
                        Console.WriteLine(d);
                    }
                }
                break;

            case "4":
                Console.Write("Введіть ID лікаря для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int deleteId))
                {
                    if (manager.Remove(deleteId))
                    {
                        Console.WriteLine($"Лікаря з ID {deleteId} успішно видалено.");
                    }
                    else
                    {
                        Console.WriteLine($"Лікаря з ID {deleteId} не знайдено.");
                    }
                }
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
        Console.WriteLine("3. Знайти пацієнта за ім'ям/прізвищем");
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
                Console.Write("Введіть запит для пошуку: ");
                string query = Console.ReadLine()!;
                var found = manager.FindByName(query);
                if (found.Length == 0)
                {
                    Console.WriteLine("Пацієнтів не знайдено.");
                }
                else
                {
                    Console.WriteLine($"Знайдено ({found.Length}):");
                    foreach (var p in found)
                    {
                        Console.WriteLine(p);
                    }
                }
                break;

            case "4":
                Console.Write("Введіть ID пацієнта для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int deleteId))
                {
                    if (manager.Remove(deleteId))
                    {
                        Console.WriteLine($"Пацієнта з ID {deleteId} успішно видалено.");
                    }
                    else
                    {
                        Console.WriteLine($"Пацієнта з ID {deleteId} не знайдено.");
                    }
                }
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