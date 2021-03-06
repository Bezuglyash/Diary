﻿using System;
using System.Collections;

namespace Diary.Another
{
    class Day : IDate
    {
        enum Months
        {
            Января = 1,
            Февраля = 2,
            Марта = 3,
            Апреля = 4,
            Мая = 5,
            Июня = 6,
            Июля = 7,
            Августа = 8,
            Сентября = 9,
            Октября = 10,
            Ноября = 11,
            Декабря = 12
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
        private Hashtable monthsOfNumbers;

        public Day()
        {
            numberOfDays = new Hashtable()
            {
                { "Января", 31 },
                { "Февраля", 28 },
                { "Марта", 31 },
                { "Апреля", 30 },
                { "Мая", 31 },
                { "Июня", 30 },
                { "Июля", 31 },
                { "Августа", 31 },
                { "Сентября", 30 },
                { "Октября", 31 },
                { "Ноября", 30 },
                { "Декабря", 31 }
            };

            indexOfMonths = new Hashtable()
            {
                { "Января", 6 },
                { "Февраля", 2 },
                { "Марта", 2 },
                { "Апреля", 5 },
                { "Мая", 0 },
                { "Июня", 3 },
                { "Июля", 5 },
                { "Августа", 1 },
                { "Сентября", 4 },
                { "Октября", 6 },
                { "Ноября", 2 },
                { "Декабря", 4 }
            };

            numberOfMonths = new Hashtable()
            {
                { "Января", "01" },
                { "Февраля", "02" },
                { "Марта", "03" },
                { "Апреля", "04" },
                { "Мая", "05" },
                { "Июня", "06" },
                { "Июля", "07" },
                { "Августа", "08" },
                { "Сентября", "09" },
                { "Октября", "10" },
                { "Ноября", "11" },
                { "Декабря", "12" }
            };

            monthsOfNumbers = new Hashtable()
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
        }

        public static string GetDay(string date)
        {
            Hashtable daysWeek = new Hashtable()
            {
                { 1, "Пн" },
                { 2, "Вт" },
                { 3, "Ср" },
                { 4, "Чт" },
                { 5, "Пт" },
                { 6, "Сб" },
                { 0, "Вс" }
            };
            return daysWeek[new Day().GetNumberDayOfWeek(Convert.ToInt32(date.Split(new char[] {'.'})[0]),
                date.Split(new char[] {'.'})[1],
                Convert.ToInt32(date.Split(new char[] {'.'})[2]))].ToString();
        }

        public string GetMonthNow()
        {
            Months month = (Months)Convert.ToInt32(DateTime.Now.ToString("MM"));
            return month + "";
        }

        public int GetNumberOfDaysInThisMonth(string month, int year)
        {
            if (IsThisLeapYear(year) == true && month.ToLower() == "Февраля".ToLower())
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
            if ((month == "Января" || month == "Февраля") && IsThisLeapYear(year) == true)
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
            if (Int32.TryParse(month, out int result))
            {
                month = GetMonth(month);
            }
            if ((month == "Января" || month == "Февраля") && IsThisLeapYear(year) == true)
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

        public string GetMonth(string numberMonth)
        {
            return monthsOfNumbers[numberMonth].ToString();
        }

        public string GetPreviousMonth(string thisMonth)
        {
            string number = numberOfMonths[thisMonth].ToString();
            int previousNumber = Convert.ToInt32(number) - 1;
            number = previousNumber == 0 ? "12" : previousNumber.ToString();
            if (Convert.ToInt32(number) < 10)
            {
                return monthsOfNumbers["0" + number].ToString();
            }
            return monthsOfNumbers[number].ToString();
        }

        public string GetNextMonth(string thisMonth)
        {
            string number = numberOfMonths[thisMonth].ToString();
            int nextNumber = Convert.ToInt32(number) + 1;
            number = nextNumber == 13 ? "1" : nextNumber.ToString();
            if (Convert.ToInt32(number) < 10)
            {
                return monthsOfNumbers["0" + number].ToString();
            }
            return monthsOfNumbers[number].ToString();
        }
    }
}