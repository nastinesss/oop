using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02
{
    internal class Task1
    {
        public static void Run() {
            int n = int.Parse(Console.ReadLine()!);
            double[] weights = new double[n];
            double sum = 0;
            for (int i = 0; i<n; i++)
            {
                weights[i] = double.Parse(Console.ReadLine()!);
                sum += weights[i];
            }
            double average = sum / n;
            double min = weights[0];
            double max = weights[0];
            int aboveAverageCount = 0;

            for (int i = 0; i < n; i++)
            {
                if (weights[i] < min)
                {
                    min = weights[i];
                }

                if (weights[i] > max)
                {
                    max = weights[i];
                }

                if (weights[i] > average)
                {
                    aboveAverageCount++;
                }
            }

            Console.WriteLine($"Кількість: {n} / Середня вага: {average:F1} кг / Мін / Макс: {min:F1} / {max:F1} кг / Вище середнього: {aboveAverageCount} з {n}");
        }
}
}
