using System;
using System.Collections.Generic;

namespace Bank.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime AddBusinessDays(this DateTime current, int days, List<DateTime> hollidays = null)
        {
            hollidays = hollidays ?? GetBrazilianHolidays();

            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);

            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                } while (current.DayOfWeek == DayOfWeek.Saturday ||
                         current.DayOfWeek == DayOfWeek.Sunday ||
                         hollidays.Contains(current));
            }
            return current;
        }

        public static List<DateTime> GetBrazilianHolidays()
        {
            var currentYear = DateTime.Now.Year;
            var holidayList = new List<DateTime>();
            for (int i = 0; i < 2; i++)
            {
                holidayList.Add(new DateTime(currentYear, 1, 1));   // Ano novo 
                holidayList.Add(new DateTime(currentYear, 4, 21));  // Tiradentes
                holidayList.Add(new DateTime(currentYear, 5, 1));   // Dia do trabalho
                holidayList.Add(new DateTime(currentYear, 9, 7));   // Dia da Independência do Brasil
                holidayList.Add(new DateTime(currentYear, 10, 12)); // Nossa Senhora Aparecida
                holidayList.Add(new DateTime(currentYear, 11, 2));  // Finados
                holidayList.Add(new DateTime(currentYear, 11, 15)); // Proclamação da República
                holidayList.Add(new DateTime(currentYear, 12, 25)); // Natal

                #region Variant Holidays

                int x, y;
                int a, b, c, d, e;
                int day, month;

                if (currentYear >= 1900 & currentYear <= 2099)
                {
                    x = 24;
                    y = 5;
                }
                else if (currentYear >= 2100 & currentYear <= 2199)
                {
                    x = 24;
                    y = 6;
                }
                else if (currentYear >= 2200 & currentYear <= 2299)
                {
                    x = 25;
                    y = 7;
                }
                else
                {
                    x = 24;
                    y = 5;
                }

                a = currentYear % 19;
                b = currentYear % 4;
                c = currentYear % 7;
                d = (19 * a + x) % 30;
                e = (2 * b + 4 * c + 6 * d + y) % 7;

                if ((d + e) > 9)
                {
                    day = (d + e - 9);
                    month = 4;
                }

                else
                {
                    day = (d + e + 22);
                    month = 3;
                }

                var pascoa = new DateTime(currentYear, month, day);
                var sextaSanta = pascoa.AddDays(-2);
                var carnaval = pascoa.AddDays(-47);
                var corpusChristi = pascoa.AddDays(60);

                holidayList.Add(pascoa);
                holidayList.Add(sextaSanta);
                holidayList.Add(carnaval);
                holidayList.Add(corpusChristi);

                #endregion

                currentYear++;
            }

            return holidayList;
        }
    }
}
