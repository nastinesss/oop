using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_01
{
    internal class Task3
    {
        public static void Run()
        {
            int year = int.Parse(Console.ReadLine());
            int age = 2026 - year;
            Console.WriteLine($"Вік: {age} р.");
            if (age >= 0 && age <= 17)
            {
                Console.WriteLine("Категорія: дитина.");
            }
            else if (age >=18 && age<= 59)
            {
                Console.WriteLine("Категорія: дорослий.");
            }
            else if (age >= 60)
            {
                Console.WriteLine("Категорія: пенсіонер.");
            }

        }
    }
}
