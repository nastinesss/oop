using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02
{
    internal class Task2
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);
            int[] original = new int[n];
            int[] sorted = new int[n];

            for (int i = 0; i < n; i++)
            {
                original[i] = int.Parse(Console.ReadLine()!);
                sorted[i] = original[i];
            }

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (sorted[j] > sorted[j + 1])
                    {
                        int temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine($"Черга (до):     {string.Join(" ", original)}");
            Console.WriteLine($"Черга (після):  {string.Join(" ", sorted)}");
            Console.WriteLine($"Найдешевший:    {sorted[0]} грн");
            Console.WriteLine($"Найдорожчий:    {sorted[n - 1]} грн");
        }
    }
}
