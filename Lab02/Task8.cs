using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02
{
    internal class Task8
    {
        public static void Run()
        {
            int d = int.Parse(Console.ReadLine()!);
            int w = int.Parse(Console.ReadLine()!);

            int[,,] data = new int[d, w, 2];

            for (int dep = 0; dep < d; dep++)
            {
                for (int week = 0; week < w; week++)
                {
                    for (int shift = 0; shift < 2; shift++)
                    {
                        data[dep, week, shift] = int.Parse(Console.ReadLine()!);
                    }
                }
            }

            int maxTotal = -1;
            int busiestDept = 0;

            for (int dep = 0; dep < d; dep++)
            {
                int morningTotal = 0;
                int eveningTotal = 0;

                for (int week = 0; week < w; week++)
                {
                    morningTotal += data[dep, week, 0];
                    eveningTotal += data[dep, week, 1];
                }

                int deptTotal = morningTotal + eveningTotal;

                Console.WriteLine($"Відділення {dep + 1}: всього {deptTotal}, ранок: {morningTotal}, вечір: {eveningTotal}");

                if (deptTotal > maxTotal)
                {
                    maxTotal = deptTotal;
                    busiestDept = dep;
                }
            }
            Console.WriteLine($"Найзавантаженіше: Відділення {busiestDept + 1} ({maxTotal} пацієнтів)");
        }
    }
}
