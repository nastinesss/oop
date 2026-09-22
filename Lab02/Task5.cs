using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02
{
    internal class Task5
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);
            int[,] matrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                string[] parts = Console.ReadLine()!
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(parts[j]);
                }
            }

            List<int> mainDiag = new List<int>();
            List<int> sideDiag = new List<int>();

            int mainSum = 0;
            int sideSum = 0;

            for (int i = 0; i < n; i++)
            {
                int mainVal = matrix[i, i];
                mainDiag.Add(mainVal);
                mainSum += mainVal;

                int sideVal = matrix[i, n - 1 - i];
                sideDiag.Add(sideVal);
                sideSum += sideVal;
            }

            Console.WriteLine($"Головна діагональ: {string.Join(", ", mainDiag)} (сума = {mainSum})");
            Console.WriteLine($"Побічна діагональ: {string.Join(", ", sideDiag)} (сума = {sideSum})");
        }
    }
}
