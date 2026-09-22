using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02
{
    internal class Task4
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);
            int m = int.Parse(Console.ReadLine()!);

            int[,] matrix = new int[n, m];

            for (int i = 0; i < n; i++)
            {
                string[] parts = Console.ReadLine()!
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = int.Parse(parts[j]);
                }
            }

            for (int i = 0; i < n; i++)
            {
                int rowSum = 0;
                for (int j = 0; j < m; j++)
                {
                    rowSum += matrix[i, j];
                }
                Console.WriteLine($"Лікар {i + 1}: {rowSum} прийомів");
            }

            int[] colSums = new int[m];
            for (int j = 0; j < m; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    colSums[j] += matrix[i, j];
                }
            }
            Console.WriteLine($"По днях: {string.Join(", ", colSums)}");

            int maxVal = matrix[0, 0];
            int maxRow = 0;
            int maxCol = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (matrix[i, j] > maxVal)
                    {
                        maxVal = matrix[i, j];
                        maxRow = i;
                        maxCol = j;
                    }
                }
            }

            Console.WriteLine($"Максимум: {maxVal} (Лікар {maxRow + 1}, День {maxCol + 1})");
        }
    }
}
