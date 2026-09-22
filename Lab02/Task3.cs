using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02
{
    internal class Task3
    {
        public static void Run()
        {
            string[] days =
            {
                "Понеділок",
                "Вівторок",
                "Середа",
                "Четвер",
                "Пʼятниця",
                "Субота",
                "Неділя"
            };

            int[] patients = new int[7];
            int total = 0;

            for (int i = 0; i < 7; i++)
            {
                patients[i] = int.Parse(Console.ReadLine()!);
                total += patients[i];
            }

            int maxIndex = 0;
            int minIndex = 0;

            for (int i = 1; i < 7; i++)
            {
                if (patients[i] > patients[maxIndex])
                {
                    maxIndex = i;
                }

                if (patients[i] < patients[minIndex])
                {
                    minIndex = i;
                }
            }
            
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine($"{days[i],-10} : {patients[i]} пацієнтів");
            }

            Console.WriteLine($"Всього     : {total} пацієнтів");
            Console.WriteLine($"Пік        : {days[maxIndex]} ({patients[maxIndex]} пацієнтів)");
            Console.WriteLine($"Мінімум    : {days[minIndex]} ({patients[minIndex]} пацієнтів)");
        }
    }
}
