using Diary.Another;
using System;
using System.Collections.Generic;

namespace Diary.DataBase.HabitTracker
{
    [Serializable]
    public class MonthsYear
    {
        public int Year { get; set; }

        internal MyDictionary<int, List<int>> MonthsCheck { get; set; }
    }
}