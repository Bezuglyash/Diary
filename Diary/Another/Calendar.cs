using System;
using System.Collections;

namespace Diary.Another
{
    class Calendar : IDate
    {
        enum Months
        {
            Январь = 1,
            Февраль = 2,
            Март = 3,
            Апрель = 4,
            Май = 5,
            Июнь = 6,
            Июль = 7,
            Август = 8,
            Сентябрь = 9,
            Октябрь = 10,
            Ноябрь = 11,
            Декабрь = 12
        }

        enum DayOfTheWeek
        {
            Понедельник = 1,
            Вторник = 2,
            Среда = 3,
            Четверг = 4,
            Пятница = 5,
            Суббота = 6,
            Воскресенье = 0
        }

        private Hashtable numberOfDays;
        private Hashtable indexOfMonths;
        private Hashtable numberOfMonths;
        private Hashtable variantNameOfMonths;

        public Calendar()
        {
            numberOfDays = new Hashtable()
            {
                { "Январь", 31 },
                { "Февраль", 28 },
                { "Март", 31 },
                { "Апрель", 30 },
                { "Май", 31 },
                { "Июнь", 30 },
                { "Июль", 31 },
                { "Август", 31 },
                { "Сентябрь", 30 },
                { "Октябрь", 31 },
                { "Ноябрь", 30 },
                { "Декабрь", 31 }
            };

            indexOfMonths = new Hashtable()
            {
                { "Январь", 6 },
                { "Февраль", 2 },
                { "Март", 2 },
                { "Апрель", 5 },
                { "Май", 0 },
                { "Июнь", 3 },
                { "Июль", 5 },
                { "Август", 1 },
                { "Сентябрь", 4 },
                { "Октябрь", 6 },
                { "Ноябрь", 2 },
                { "Декабрь", 4 }
            };

            numberOfMonths = new Hashtable()
            {
                { "Январь", "01" },
                { "Февраль", "02" },
                { "Март", "03" },
                { "Апрель", "04" },
                { "Май", "05" },
                { "Июнь", "06" },
                { "Июль", "07" },
                { "Август", "08" },
                { "Сентябрь", "09" },
                { "Октябрь", "10" },
                { "Ноябрь", "11" },
                { "Декабрь", "12" }
            };

            variantNameOfMonths = new Hashtable()
            {
                { "Январь", "Января" },
                { "Февраль", "Февраля" },
                { "Март", "Марта" },
                { "Апрель", "Апреля" },
                { "Май", "Мая" },
                { "Июнь", "Июня" },
                { "Июль", "Июля" },
                { "Август", "Августа" },
                { "Сентябрь", "Сентября" },
                { "Октябрь", "Октября" },
                { "Ноябрь", "Ноября" },
                { "Декабрь", "Декабря" }
            };
        }

        public string GetMonthNow()
        {
            Months month = (Months)Convert.ToInt32(DateTime.Now.ToString("MM"));
            return month + "";
        }

        public string GetMonthNow(string monthNumber)
        {
            Months month = (Months)Convert.ToInt32(monthNumber);
            return month + "";
        }

        public int GetNumberOfDaysInThisMonth(string month, int year)
        {
            if (IsThisLeapYear(year) == true && month.ToLower() == "Февраль".ToLower())
            {
                return 29;
            }
            else
            {
                return (int)numberOfDays[month];
            }
        }

        public string GetNameDayOfWeek(int numberOfDay, string month, int year)
        {
            int indexCentury, indexYear, indexOfMonth, indexLeapYear;
            if (year >= 1900 && year < 2000)
            {
                indexCentury = 1;
            }
            else
            {
                indexCentury = 0;
            }
            if ((month == "Январь" || month == "Февраль") && IsThisLeapYear(year) == true)
            {
                indexLeapYear = -1;
            }
            else
            {
                indexLeapYear = 0;
            }
            indexOfMonth = (int)indexOfMonths[month];
            int interimNumber = Convert.ToInt32(year.ToString().Remove(0, 2));
            indexYear = interimNumber / 12 + interimNumber % 12 + interimNumber % 12 / 4;
            return (DayOfTheWeek)((indexYear + numberOfDay + indexOfMonth + indexCentury + indexLeapYear) % 7) + "";
        }

        public int GetNumberDayOfWeek(int numberOfDay, string month, int year)
        {
            int indexCentury, indexYear, indexOfMonth, indexLeapYear;
            if (year >= 1900 && year < 2000)
            {
                indexCentury = 1;
            }
            else
            {
                indexCentury = 0;
            }
            if ((month == "Январь" || month == "Февраль") && IsThisLeapYear(year) == true)
            {
                indexLeapYear = -1;
            }
            else
            {
                indexLeapYear = 0;
            }
            indexOfMonth = (int)indexOfMonths[month];
            int interimNumber = Convert.ToInt32(year.ToString().Remove(0, 2));
            indexYear = interimNumber / 12 + interimNumber % 12 + interimNumber % 12 / 4;
            return (indexYear + numberOfDay + indexOfMonth + indexCentury + indexLeapYear) % 7;
        }

        public string GetNumberOfMonth(string nameOfMonth)
        {
            return numberOfMonths[nameOfMonth].ToString();
        }

        public bool IsThisLeapYear(int year)
        {
            if (Convert.ToInt32(year.ToString().Remove(0, 2)) % 4 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetSecondVariantNameOfMonths(string firstVariantNameOfMonths)
        {
            return variantNameOfMonths[firstVariantNameOfMonths].ToString();
        }

        public string GetMonth(string numberOfMonth)
        {
            Hashtable monthsOfNumbers = new Hashtable()
            {
                { "01", "Января" },
                { "02", "Февраля" },
                { "03", "Марта" },
                { "04", "Апреля" },
                { "05", "Мая" },
                { "06", "Июня" },
                { "07", "Июля" },
                { "08", "Августа" },
                { "09", "Сентября" },
                { "10", "Октября" },
                { "11", "Ноября" },
                { "12", "Декабря" }
            };
            return monthsOfNumbers[numberOfMonth].ToString();
        }
    }
}