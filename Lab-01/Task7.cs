using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_01
{
    internal class Task7
    {
        public static void Run()
        {
            int N=int.Parse(Console.ReadLine());
            decimal[] prices= new decimal[N];
            for (int i = 0; i < N; i++)
            {
                prices[i] = decimal.Parse(Console.ReadLine());
            }
            decimal totalSum = 0;
            decimal min = prices[0];
            decimal max = prices[0];
            foreach (decimal price in prices)
            {
                totalSum += price;
                if(price< min) min = price;
                if(price>max) max = price; 
            }
            decimal avg = totalSum / N;
            string aboveAvg = " ";
            for (int i = 0; i < N; i++) 
            {
                if (prices[i]>avg)
                {
                    aboveAvg += (i + 1) + " ";
                }
            }
            aboveAvg = aboveAvg.Trim();
            int expensive = -1;
            int k = 0;
            while (k < N)
            {
                if (prices[k] > 1000)
                {
                    expensive = k;
                    break;
                }
                k++;
            }
            Console.WriteLine("=== Звіт по прийомах ===");
            Console.WriteLine($"Кількість:         {N}");
            Console.WriteLine($"Загальна сума:     {totalSum:F2} грн");
            Console.WriteLine($"Середня:           {avg:F2} грн");
            Console.WriteLine($"Мін / Макс:        {min:F2} / {max:F2} грн");
            Console.WriteLine($"Вище середнього:   {aboveAvg} грн");
            if(expensive!=-1)
            {
                Console.WriteLine($"Перший > 1000:    #{expensive + 1}-{prices[expensive]:F2} грн");
            }
            else
            {
                Console.WriteLine("Перший > 1000:    немає");
            }
            Console.WriteLine("=====================");
        }
    }
}
