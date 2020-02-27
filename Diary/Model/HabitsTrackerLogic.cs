using Diary.Another;
using Diary.DataBase.HabitTracker;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Diary.Another.Tracker;

namespace Diary.Model
{
    class HabitsTrackerLogic : ViewModelBase
    {
        private static FileStream fileXML;
        private XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HabitTracker>));
        private List<HabitTracker> habitsTracker;
        private int countDeleteHabits;
        private const string NAME_FILE = "HabitsTracker.xml";

        public HabitsTrackerLogic ()
        {
            countDeleteHabits = 0;
            ReadFile();
            CheckMonths();
            CountDeleteHabits();
            NumberOfHabits = habitsTracker.Count - countDeleteHabits;
        }

        public int NumberOfHabits { get; set; }

        public ObservableCollection<DataHabit> GetDataHabits()
        {
            ObservableCollection<DataHabit> dataHabits = new ObservableCollection<DataHabit>();
            foreach (var habitTracker in habitsTracker)
            {
                if (habitTracker.DateDelete == "")
                {
                    DataHabit dataHabit = new DataHabit(habitTracker.Id, habitTracker.Habit, true);
                    dataHabits.Add(dataHabit);
                }
            }
            return dataHabits;
        }

        public void Distribution(List<DataHabit> dataHabits, string date)
        {
            int count = dataHabits.Count;
            bool isUpdate = false;
            for (int j = 0; j < habitsTracker.Count; j++)
            {
                for (int i = 0; i < dataHabits.Count; i++)
                {
                    if (dataHabits[i].Index == habitsTracker[j].Id && habitsTracker[j].DateDelete == "")
                    {
                        isUpdate = true;
                        if (dataHabits[i].Habit != habitsTracker[j].Habit)
                        {
                            habitsTracker[j].Habit = dataHabits[i].Habit;
                        }
                        dataHabits.Remove(dataHabits[i]);
                        break;
                    }
                    if (count != dataHabits.Count)
                    {
                        i--;
                        count = dataHabits.Count;
                    }
                }
                if (isUpdate == false && habitsTracker[j].DateDelete == "")
                {
                    habitsTracker[j].DateDelete = date;
                    countDeleteHabits++;
                }
                else
                {
                    isUpdate = false;
                }
            }
            if (dataHabits.Count > 0)
            {
                AddHabit(dataHabits, date);
            }
            WriteFile();
            NumberOfHabits = habitsTracker.Count - countDeleteHabits;
        }

        public ObservableCollection<WeekHabit> GetWeekHabits(List<string> dates)
        {
            CheckMonths(dates);
            ObservableCollection<WeekHabit> weekHabits = new ObservableCollection<WeekHabit>();
            foreach (var habitTracker in habitsTracker)
            {
                if (IsCreateInThePast(habitTracker, dates) && IsDeleteInTheFuture(habitTracker, dates))
                {
                    WeekHabit weekHabit = new WeekHabit();
                    weekHabit.Habit = habitTracker.Habit;
                    for (int i = 0; i < habitTracker.Dates.Count; i++)
                    {
                        for (int j = 0; j < dates.Count; j++)
                        {
                            if (habitTracker.Dates[i].Year == Convert.ToInt32(dates[j].Split(new char[] {'.'})[2]) &&
                                habitTracker.Dates[i].MonthsCheckList[Convert.ToInt32(dates[j].Split(new char[] {'.'})[1]) - 1].Count != 0)
                            {
                                foreach (var day in habitTracker.Dates[i].MonthsCheckList[Convert.ToInt32(dates[j].Split(new char[] { '.' })[1]) - 1])
                                {
                                    if (day == Convert.ToInt32(dates[j].Split(new char[] { '.' })[0]))
                                    {
                                        switch (j)
                                        {
                                            case 0:
                                                weekHabit.IsDoneOnMonday = true;
                                                break;
                                            case 1:
                                                weekHabit.IsDoneOnTuesday = true;
                                                break;
                                            case 2:
                                                weekHabit.IsDoneOnWednesday = true;
                                                break;
                                            case 3:
                                                weekHabit.IsDoneOnThursday = true;
                                                break;
                                            case 4:
                                                weekHabit.IsDoneOnFriday = true;
                                                break;
                                            case 5:
                                                weekHabit.IsDoneOnSaturday = true;
                                                break;
                                            case 6:
                                                weekHabit.IsDoneOnSunday = true;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    weekHabits.Add(weekHabit);
                }
            }
            return weekHabits;
        }

        public void UpdateHabit(string text, string date)
        {
            foreach (var habit in habitsTracker)
            {
                if (habit.Habit == text)
                {
                    foreach (var dateInList in habit.Dates)
                    {
                        if (dateInList.Year == Convert.ToInt32(date.Split(new char[] {'.'})[2]))
                        {
                            if (dateInList.MonthsCheckList != null)
                            {
                                if (dateInList.MonthsCheckList[Convert.ToInt32(date.Split(new char[] {'.'})[1]) - 1].IndexOf(Convert.ToInt32(date.Split(new char[] {'.'})[0])) == -1)
                                {
                                    dateInList.MonthsCheckList[Convert.ToInt32(date.Split(new char[] {'.'})[1]) - 1].Add(Convert.ToInt32(date.Split(new char[] {'.'})[0]));
                                }
                                else
                                {
                                    dateInList.MonthsCheckList[Convert.ToInt32(date.Split(new char[] {'.'})[1]) - 1].Remove(Convert.ToInt32(date.Split(new char[] {'.'})[0]));
                                }
                            }
                            else
                            {
                                dateInList.MonthsCheckList = new List<List<int>>( Convert.ToInt32(date.Split(new char[] { '.' })[0]));
                            }
                            break;
                        }
                    }
                }
            }
            WriteFile();
            NumberOfHabits = habitsTracker.Count - countDeleteHabits;
        }

        public static void CreateXmlFile()
        {
            fileXML = new FileStream(NAME_FILE, FileMode.Create);
            fileXML.Close();
        }

        private void AddHabit(List<DataHabit> data, string date)
        {
            int index = habitsTracker.Count == 0 ? 0 : habitsTracker[habitsTracker.Count - 1].Id;
            for (int i = 0; i < data.Count; i++)
            {
                HabitTracker habit = new HabitTracker(index + 1 + i);
                habit.Habit = data[i].Habit;
                habit.DateAdd = date;
                List<int> list = new List<int>();
                habit.Dates.Add(new MonthsYear()
                {
                    Year = Convert.ToInt32(date.Split(new char[] { '.' })[2]),
                    MonthsCheckList = new List<List<int>>()
                });
                habitsTracker.Add(habit);
            }
        }

        private void CountDeleteHabits()
        {
            foreach (var habit in habitsTracker)
            {
                if (habit.DateDelete != "")
                {
                    countDeleteHabits++;
                }
            }
        }

        private bool IsCreateInThePast(HabitTracker habit, List<string> list)
        {
            foreach (var date in list)
            {
                if (Convert.ToInt32(habit.DateAdd.Split(new char[] { '.' })[2]) < Convert.ToInt32(date.Split(new char[] { '.' })[2]))
                {
                    return true;
                }
                if (Convert.ToInt32(habit.DateAdd.Split(new char[] {'.'})[2]) == Convert.ToInt32(date.Split(new char[] {'.'})[2]) &&
                         Convert.ToInt32(habit.DateAdd.Split(new char[] {'.'})[1]) < Convert.ToInt32(date.Split(new char[] {'.'})[1]))
                {
                    return true;
                }
                if (Convert.ToInt32(habit.DateAdd.Split(new char[] {'.'})[2]) == Convert.ToInt32(date.Split(new char[] {'.'})[2]) &&
                         Convert.ToInt32(habit.DateAdd.Split(new char[] {'.'})[1]) == Convert.ToInt32(date.Split(new char[] {'.'})[1]) &&
                         Convert.ToInt32(habit.DateAdd.Split(new char[] {'.'})[0]) <= Convert.ToInt32(date.Split(new char[] {'.'})[0]))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsDeleteInTheFuture(HabitTracker habit, List<string> list)
        {
            foreach (var date in list)
            {
                if (habit.DateDelete == "")
                {
                    return true;
                }
                else if (Convert.ToInt32(habit.DateDelete.Split(new char[] { '.' })[2]) > Convert.ToInt32(date.Split(new char[] { '.' })[2]))
                {
                    return true;
                }
                if (Convert.ToInt32(habit.DateDelete.Split(new char[] { '.' })[2]) == Convert.ToInt32(date.Split(new char[] { '.' })[2]) &&
                    Convert.ToInt32(habit.DateDelete.Split(new char[] { '.' })[1]) > Convert.ToInt32(date.Split(new char[] { '.' })[1]))
                {
                    return true;
                }
                if (Convert.ToInt32(habit.DateDelete.Split(new char[] { '.' })[2]) == Convert.ToInt32(date.Split(new char[] { '.' })[2]) &&
                    Convert.ToInt32(habit.DateDelete.Split(new char[] { '.' })[1]) == Convert.ToInt32(date.Split(new char[] { '.' })[1]) &&
                    Convert.ToInt32(habit.DateDelete.Split(new char[] { '.' })[0]) >= Convert.ToInt32(date.Split(new char[] { '.' })[0]))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckMonths()
        {
            foreach (var tracker in habitsTracker)
            {
                foreach (var date in tracker.Dates)
                {
                    if (Convert.ToInt32(DateTime.Now.ToString("MM")) > date.MonthsCheckList.Count)
                    {
                        int size = date.MonthsCheckList.Count;
                        for (int i = 0; i < Convert.ToInt32(DateTime.Now.ToString("MM")) - size; i++)
                        {
                            date.MonthsCheckList.Add(new List<int>());
                        }
                    }
                }
            }
        }

        private void CheckMonths(List<string> dates)
        {
            foreach (var tracker in habitsTracker)
            {
                foreach (var date in tracker.Dates)
                {
                    foreach (var dateWeek in dates)
                    {
                        if (Convert.ToInt32(dateWeek.Split(new char[] { '.' })[2]) >= date.Year && Convert.ToInt32(dateWeek.Split(new char[] { '.' })[1]) > date.MonthsCheckList.Count)
                        {
                            int size = date.MonthsCheckList.Count;
                            for (int i = 0; i < Convert.ToInt32(dateWeek.Split(new char[] { '.' })[1]) - size; i++)
                            {
                                date.MonthsCheckList.Add(new List<int>());
                            }
                        }
                    }
                }
            }
        }

        private void ReadFile()
        {
            try
            {
                using (fileXML = new FileStream(NAME_FILE, FileMode.Open))
                {
                    try
                    {
                        habitsTracker = (List<HabitTracker>)xmlSerializer.Deserialize(fileXML);
                    }
                    catch (Exception)
                    {
                        habitsTracker = new List<HabitTracker>();
                    }
                }
            }
            catch (Exception)
            {
                CreateXmlFile();
                habitsTracker = new List<HabitTracker>();
            }
        }

        private void WriteFile()
        {
            using (fileXML = new FileStream(NAME_FILE, FileMode.Create))
            {
                xmlSerializer.Serialize(fileXML, habitsTracker);
            }
        }
    }
}