using System;
using System.Collections.Generic;

namespace Diary.DataBase.HabitTracker
{
    [Serializable]
    public class MonthsYear
    {
        public int Year { get; set; }

        public List<List<int>> MonthsCheckList { get; set; }
    }
}