using ClinicApp;

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

Console.WriteLine("\n=== Лікарі ===");
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

Console.WriteLine($"\nПоточна година: {DateTime.Now.Hour}:00");
Console.WriteLine($"Чи може {doc1.FullName} прийняти о 10:00: {(doc1.CanAcceptAt(10) ? "Так" : "Ні")}");
Console.WriteLine($"Чи може {doc1.FullName} прийняти о 18:00: {(doc1.CanAcceptAt(18) ? "Так" : "Ні")}");