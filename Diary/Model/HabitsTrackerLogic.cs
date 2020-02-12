using Diary.Another;
using Diary.DataBase.HabitTracker;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Diary.Model
{
    class HabitsTrackerLogic : ViewModelBase
    {
        private static FileStream fileXML;
        private XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HabitTracker>));
        private List<HabitTracker> habitsTracker;
        private const string NAME_FILE = "HabitsTracker.xml";

        public HabitsTrackerLogic ()
        {
            ReadFile();
            NumberOfHabits = habitsTracker.Count;
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
                    if (dataHabits[i].Index == habitsTracker[j].Id)
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
                if (isUpdate == false)
                {
                    habitsTracker.Remove(habitsTracker[j]);
                    j--;
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
            NumberOfHabits = habitsTracker.Count;
        }

        public static void CreateXMLFile()
        {
            fileXML = new FileStream(NAME_FILE, FileMode.Create);
            fileXML.Close();
        }

        private void AddHabit(List<DataHabit> data, string date)
        {
            int index;
            if (habitsTracker.Count == 0)
            {
                index = 0;
            }
            else
            {
                index = habitsTracker[habitsTracker.Count - 1].Id;
            }
            for (int i = 0; i < data.Count; i++)
            {
                HabitTracker habit = new HabitTracker(index + 1 + i);
                habit.Habit = data[i].Habit;
                habit.DateAdd = date;
                List<int> list = new List<int>();
                habit.Dates.Add(new MonthsYear()
                {
                    Year = Convert.ToInt32(date.Split(new char[] { '.' })[2]),
                    MonthsCheck = new MyDictionary<int, List<int>>(Convert.ToInt32(date.Split(new char[] { '.' })[1]), new List<int>() { })
                });
                habitsTracker.Add(habit);
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
                CreateXMLFile();
                habitsTracker = new List<HabitTracker>();
            }
        }

        private void WriteFile()
        {
            using (fileXML = new FileStream(NAME_FILE, FileMode.Open))
            {
                xmlSerializer.Serialize(fileXML, habitsTracker);
            }
        }
    }
}