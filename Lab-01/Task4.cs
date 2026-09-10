using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_01
{
    internal class Task4
    {
        public static void Run()
        {
            int systol=int.Parse(Console.ReadLine());
            int dyastol=int.Parse(Console.ReadLine());
            if(systol<120 && dyastol < 80)
            {
                Console.WriteLine($"Тиск: {systol}/{dyastol} - норма");
            }
            else if (systol<130 && dyastol < 80)
            {
                Console.WriteLine($"Тиск: {systol}/{dyastol} - підвищений");
            }
            else if (systol < 140 || dyastol < 90)
            {
                Console.WriteLine($"Тиск: {systol}/{dyastol} - гіпертонія 1 ступеня");
            }
            else
            {
                Console.WriteLine($"Тиск: {systol}/{dyastol} - гіпертонія 2 ступеня");
            }
        }
    }
}
