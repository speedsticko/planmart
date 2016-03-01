using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMart.Processors
{
    public class DateUtil
    {

        public static DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        public static bool IsABlackFriday(DateTime placed)
        {
            // Black Friday (partying), the last Friday before Christmas
            DateTime black_friday1 = LastDayOfWeekBefore(DayOfWeek.Friday, new DateTime(placed.Year, 12, 25));
            if ((placed - black_friday1).Days == 0)
            {
                return true;
            }

            // Black Friday(shopping), the Friday after U.S.Thanksgiving Day.
            DateTime thanksgiving = new DateTime(placed.Year, 11, 1);
            while (thanksgiving.DayOfWeek != DayOfWeek.Thursday)
            {
                thanksgiving = thanksgiving.AddDays(1);
            }
            thanksgiving = thanksgiving.AddDays(21);// jump 3 weeks to 4th Thursday

            DateTime blackfriday2 = thanksgiving.AddDays(1);

            if ((placed - blackfriday2).Days == 0)
            {
                return true;
            }

            // Good Friday or Black Friday, a Christian observance of Jesus' crucifixion
            if ((placed - EasterSunday(placed.Year).AddDays(-2)).Days == 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsVeteransDay(DateTime placed)
        {
            return placed.Day == 11 && placed.Month == 11;
        }

        private static DateTime LastDayOfWeekBefore(DayOfWeek day, DateTime before)
        {
            DateTime tmp_date = before;
            DayOfWeek dayOfWeek = tmp_date.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                tmp_date = tmp_date.AddDays(-1);
                dayOfWeek = tmp_date.DayOfWeek;
            }
            return tmp_date;
        }

        public static bool IsMemorialDay(DateTime placed)
        {
            // last monday in May 
            DateTime memorial_day = LastDayOfWeekBefore(DayOfWeek.Monday, new DateTime(placed.Year, 5, 31));
            return (memorial_day - placed).Days == 0;
        }

        public static int GetAgeToday(DateTime birth_date)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - birth_date.Year;
            if (now < birth_date.AddYears(age)) age--;
            return age;
        }
    }
}
