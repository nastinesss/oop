using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_01
{
    internal class Task2
    {
        public static void Run()
        {
            double price=double.Parse(Console.ReadLine());
            int number=int.Parse(Console.ReadLine());
            int discount = int.Parse(Console.ReadLine());
            double total = price * number * (1 - discount / 100.0);
            Console.WriteLine($"Сума: { total:F2} грн");
        }
    }
}
