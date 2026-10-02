using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    public static class ClinicFormatter
    {
        public static string FormatBloodType(BloodType bt) => bt switch
        {
            BloodType.APositive => "A+",
            BloodType.ANegative => "A-",
            BloodType.BPositive => "B+",
            BloodType.BNegative => "B-",
            BloodType.ABPositive => "AB+",
            BloodType.ABNegative => "AB-",
            BloodType.OPositive => "0+",
            BloodType.ONegative => "0-",
            _ => "Невідомо"
        };

        public static string FormatSpeciality(Speciality s) => s switch
        {
            Speciality.General => "Загальна практика",
            Speciality.Cardiology => "Кардіологія",
            Speciality.Neurology => "Неврологія",
            Speciality.Pediatrics => "Педіатрія",
            Speciality.Surgery => "Хірургія",
            Speciality.Orthopedics => "Ортопедія",
            Speciality.Dermatology => "Дерматологія",
            Speciality.Emergency => "Невідкладна допомога",
            _ => s.ToString()
        };

        public static string FormatAge(int age)
        {
            int mod100 = age % 100;
            if (mod100 >= 11 && mod100 <= 19)
            {
                return $"{age} років";
            }

            int mod10 = age % 10;
            return mod10 switch
            {
                1 => $"{age} рік",
                2 or 3 or 4 => $"{age} роки",
                _ => $"{age} років"
            };
        }

        public static string FormatPhone(string phone)
        {
            if (phone != null && phone.Length == 10 && phone.All(char.IsDigit))
            {
                return $"({phone[..3]}) {phone[3..6]}-{phone[6..]}";
            }

            return phone ?? "";
        }
    }
}
