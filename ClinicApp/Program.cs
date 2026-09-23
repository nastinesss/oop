using ClinicApp;

DisplayInitialDemo();

var patientManager = new PatientManager();
patientManager.Add(new Patient("Іван", "Петренко", DateTime.Today.AddYears(-41), "A+", "0501234567"));
patientManager.Add(new Patient("Олена", "Коваль", DateTime.Today.AddYears(-33), "B-", "0672345678"));
patientManager.Add(new Patient("Максим", "Бойко", DateTime.Today.AddYears(-16), "0+", "0933456789"));

RunPatientsMenu(patientManager);


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
        Console.WriteLine("0. Назад / Вихід");
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
                Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                break;
        }
    }
}

static void DisplayInitialDemo()
{
    Console.WriteLine("=== Пацієнти (Task 1) ===");
    var p1 = new Patient("Іван", "Петренко", DateTime.Today.AddYears(-41), "A+", "0501234567");
    var p2 = new Patient("Олена", "Коваль", DateTime.Today.AddYears(-33), "B-", "0672345678");
    var p3 = new Patient("Максим", "Бойко", DateTime.Today.AddYears(-16), "0+", "0933456789");
    var p4 = new Patient();
    var p5 = new Patient("Марія", "Ткач");
    Console.WriteLine(p1);
    Console.WriteLine(p2);
    Console.WriteLine(p3);
    Console.WriteLine(p4);
    Console.WriteLine(p5);

    Console.WriteLine("\n=== Лікарі (Task 2) ===");
    var doc1 = new Doctor("Олександр", "Мельник", "Кардіологія", "DOC-10293", "0509876543")
    {
        WorkStartHour = 8,
        WorkEndHour = 16
    };
    var doc2 = new Doctor("Наталія", "Шевченко", "Педіатрія");
    var doc3 = new Doctor("Сергій", "Бондар", "Неврологія", "DOC-44812", "0671122334")
    {
        WorkStartHour = 12,
        WorkEndHour = 20
    };
    var doc4 = new Doctor();
    Console.WriteLine(doc1);
    Console.WriteLine(doc2);
    Console.WriteLine(doc3);
    Console.WriteLine(doc4);
}