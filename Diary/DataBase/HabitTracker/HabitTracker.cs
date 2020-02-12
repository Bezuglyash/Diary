using System;
using System.Collections.Generic;

namespace Diary.DataBase.HabitTracker
{
    [Serializable]
    public class HabitTracker
    {
        public HabitTracker() { }

        public HabitTracker(int id)
        {
            Id = id;
            Dates = new List<MonthsYear>();
            DateDelete = "";
        }

        public int Id { get; set; }

        public string Habit { get; set; }

        public string DateAdd { get; set; }

        public string DateDelete { get; set; }

        public List<MonthsYear> Dates { get; set; }
    }
}