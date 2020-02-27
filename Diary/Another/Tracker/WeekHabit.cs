using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace Diary.Another.Tracker
{
    class WeekHabit : ViewModelBase
    {
        private static Day day;

        static WeekHabit()
        {
            day = new Day();
            ListDays = new List<string>();
        }

        public WeekHabit()
        {
            IsDoneOnMonday = false;
            IsDoneOnTuesday = false;
            IsDoneOnWednesday = false;
            IsDoneOnThursday = false;
            IsDoneOnFriday = false;
            IsDoneOnSaturday = false;
            IsDoneOnSunday = false;
        }

        public static string Week { get; private set; }

        public static List<string> ListDays { get; set; }

        public static void SetWeek(string dayThis, string month, int year)
        {
            string monthName = day.GetMonth(month);
            int numberOfWeek = day.GetNumberDayOfWeek(Convert.ToInt32(dayThis), monthName, year);
            int numberOfMonth = day.GetNumberOfDaysInThisMonth(monthName, year);
            if (numberOfWeek == 0)
            {
                numberOfWeek = 7;
            }

            Week = numberOfWeek == 1 ? dayThis + " " + monthName.ToLower() + " - " : "";
            for (int i = 1; i < numberOfWeek; i++)
            {
                if (Convert.ToInt32(dayThis) - numberOfWeek + i > 0)
                {
                    string dayString = IsSmallNumber(Convert.ToInt32(dayThis) - numberOfWeek + i) ? 
                        "0" + (Convert.ToInt32(dayThis) - numberOfWeek + i) :
                        (Convert.ToInt32(dayThis) - numberOfWeek + i).ToString();

                    ListDays.Add(dayString + "." + month + "." + year);
                    if (i == 1)
                    {
                        Week = (Convert.ToInt32(dayThis) - numberOfWeek + i) + " " + monthName.ToLower() + " - ";
                    }
                }
                else
                {
                    int numberPreviousMonth = day.GetNumberOfDaysInThisMonth(day.GetPreviousMonth(monthName), year);

                    string dayString = IsSmallNumber(numberPreviousMonth + (Convert.ToInt32(dayThis) - numberOfWeek) + i) ?
                        "0" + (numberPreviousMonth + (Convert.ToInt32(dayThis) - numberOfWeek) + i) :
                        (numberPreviousMonth + (Convert.ToInt32(dayThis) - numberOfWeek) + i).ToString();

                    if (day.GetPreviousMonth(monthName) == "Декабря")
                    {
                        ListDays.Add(dayString + "." + day.GetNumberOfMonth(day.GetPreviousMonth(monthName)) + "." + (year - 1));
                    }
                    else
                    {
                        ListDays.Add(dayString + "." + day.GetNumberOfMonth(day.GetPreviousMonth(monthName)) + "." + year);
                    }

                    if (i == 1)
                    {
                        Week = (numberPreviousMonth + (Convert.ToInt32(dayThis) - numberOfWeek) + i) + " " + day.GetPreviousMonth(monthName).ToLower() + " - ";
                    }
                }
            }
            if (numberOfWeek == 7)
            {
                Week += dayThis + " " + monthName.ToLower() + " " + year;
                string dayThisString = IsSmallNumber(Convert.ToInt32(dayThis)) ? "0" + Convert.ToInt32(dayThis) : Convert.ToInt32(dayThis).ToString();
                ListDays.Add(dayThisString + "." + month + "." + year);
            }
            else
            {
                for (int i = 0; i <= 7 - numberOfWeek; i++)
                {
                    if (Convert.ToInt32(dayThis) + i <= numberOfMonth)
                    {
                        string dayString = IsSmallNumber(Convert.ToInt32(dayThis) + i) ? "0" + (Convert.ToInt32(dayThis) + i) : (Convert.ToInt32(dayThis) + i).ToString();

                        ListDays.Add(dayString + "." + month + "." + year);
                        if (i == 7 - numberOfWeek)
                        {
                            Week += (Convert.ToInt32(dayThis) + i) + " " + monthName.ToLower() + " " + year;
                        }
                    }
                    else
                    {
                        string dayString = IsSmallNumber(Convert.ToInt32(dayThis) + i - numberOfMonth) ? 
                            "0" + (Convert.ToInt32(dayThis) + i - numberOfMonth) : (Convert.ToInt32(dayThis) + i - numberOfMonth).ToString();

                        int yearExactly = year;
                        if (day.GetNextMonth(monthName) == "Января")
                        {
                            yearExactly += 1;
                            ListDays.Add(dayString + "." + day.GetNumberOfMonth(day.GetNextMonth(monthName)) + "." + yearExactly);
                        }
                        else
                        {
                            ListDays.Add(dayString + "." + day.GetNumberOfMonth(day.GetNextMonth(monthName)) + "." + yearExactly);
                        }
                        if (i == 7 - numberOfWeek)
                        {
                            Week += (Convert.ToInt32(dayThis) + i - numberOfMonth) + " " + day.GetNextMonth(monthName).ToLower() + " " + yearExactly;
                        }
                    }
                }
            }
        }

        public string Habit { get; set; }

        public bool IsDoneOnMonday { get; set; }

        public bool IsDoneOnTuesday { get; set; }

        public bool IsDoneOnWednesday { get; set; }

        public bool IsDoneOnThursday { get; set; }

        public bool IsDoneOnFriday { get; set; }

        public bool IsDoneOnSaturday { get; set; }

        public bool IsDoneOnSunday { get; set; }

        private static bool IsSmallNumber(int number)
        {
            return number < 10;
        }
    }
}