using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lab02
{
    internal class Task7
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);
            string[] names = new string[n];
            double[] bmis = new double[n];

            for (int i = 0; i < n; i++)
            {
                names[i] = Console.ReadLine()!;
                bmis[i] = double.Parse(Console.ReadLine()!);
            }

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (bmis[j] < bmis[j + 1])
                    {
                        double tempBmi = bmis[j];
                        bmis[j] = bmis[j + 1];
                        bmis[j + 1] = tempBmi;

                        string tempName = names[j];
                        names[j] = names[j + 1];
                        names[j + 1] = tempName;
                    }
                }
            }

            List<string> results = new List<string>();
            for (int i = 0; i < n; i++)
            {
                results.Add($"#{i + 1} {names[i]}: {bmis[i]:F2}");
            }

            Console.WriteLine(string.Join(" / ", results));
        }
    }
}
