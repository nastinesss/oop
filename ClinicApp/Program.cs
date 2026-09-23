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