using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_01
{
    internal class Task6
    {
        public static void Run()
        {
            int cardNum=int.Parse(Console.ReadLine());
            int lastDigit = cardNum % 10;
            string Department = lastDigit switch
            {
                0 or 1 => "загальна терапія",
                2 or 3 => "хірургія",
                4 or 5 => "кардіологія",
                6 or 7 => "неврологія",
                8 or 9 => "офтальмологія",
                _=>"невідомо"
            };
            string isPrivileged = (cardNum % 2 == 0) ? "так" : "ні";
            string needsCheckup = (cardNum % 3 == 0) ? "так" : "ні";
            Console.WriteLine($"Відділення: {Department}");
            Console.WriteLine($"Пільгова:   {isPrivileged}");
            Console.WriteLine($"Огляд:      {needsCheckup}");
        }
    }
}
