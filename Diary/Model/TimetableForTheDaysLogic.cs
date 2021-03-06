﻿using Diary.Another;
using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Diary.Model
{
    class TimetableForTheDaysLogic : ViewModelBase
    {
        private SQLiteConnection dataBase;
        private List<TimetableForTheDay> timetablesForTheDay;

        public TimetableForTheDaysLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            StandartActions();
        }

        public IEnumerable<TimetableForTheDay> ListOfCases { get; set; }

        public int NumberOfCases { get; set; }

        public ObservableCollection<TaskForTheDay> GetTasksInThisDate (string date)
        {
            ObservableCollection<TaskForTheDay> tasksForTheDay = new ObservableCollection<TaskForTheDay>();
            foreach (var timetableForTheDay in timetablesForTheDay)
            {
                if (timetableForTheDay.DateTimetable == date)
                {
                    tasksForTheDay.Add(new TaskForTheDay(
                        timetableForTheDay.Id,
                        timetableForTheDay.Case,
                        timetableForTheDay.Description,
                        timetableForTheDay.IsItDone));
                }
            }
            return tasksForTheDay;
        }

        public void UpdateTask(TaskForTheDay task, string date)
        {
            TimetableForTheDay timetable = new TimetableForTheDay();
            timetable.Id = task.Index;
            timetable.DateTimetable = date;
            timetable.Case = task.Title;
            timetable.Description = task.Description;
            if (task.IsDone == true)
            {
                timetable.IsItDone = 1;
            }
            else
            {
                timetable.IsItDone = 0;
            }
            dataBase.Update(timetable);
            StandartActions();
        }

        public void Distribution (List<TaskForTheDay> tasksForTheDay, string date)
        {
            if (tasksForTheDay.Count != 0 && tasksForTheDay[0] != null)
            {
                int count = tasksForTheDay.Count;
                for (int i = 0; i < tasksForTheDay.Count; i++)
                {
                    for (int j = 0; j < timetablesForTheDay.Count; j++)
                    {
                        if (date == timetablesForTheDay[j].DateTimetable && tasksForTheDay[i].Index == timetablesForTheDay[j].Id)
                        {
                            if (tasksForTheDay[i].Title == timetablesForTheDay[j].Case)
                            {
                                if (tasksForTheDay[i].Description != timetablesForTheDay[j].Description)
                                {
                                    UpdateTask(timetablesForTheDay[j], tasksForTheDay[i]);
                                }
                            }
                            else
                            {
                                UpdateTask(timetablesForTheDay[j], tasksForTheDay[i]);
                            }
                            tasksForTheDay.Remove(tasksForTheDay[i]);
                            timetablesForTheDay.Remove(timetablesForTheDay[j]);
                            break;
                        }
                    }
                    if (count != tasksForTheDay.Count)
                    {
                        i--;
                        count = tasksForTheDay.Count;
                    }
                }
                if (tasksForTheDay.Count > 0)
                {
                    AddTask(tasksForTheDay, date);
                }
            }
            for (int i = 0; i < timetablesForTheDay.Count; i++)
            {
                if (date == timetablesForTheDay[i].DateTimetable)
                {
                    DeleteTask(timetablesForTheDay[i]);
                }
            }
            StandartActions();
        }

        public bool IsHaveTasks(string date)
        {
            foreach (var timetableForTheDay in timetablesForTheDay)
            {
                if (timetableForTheDay.DateTimetable == date)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddTask(List<TaskForTheDay> tasks, string date)
        {
            List<TimetableForTheDay> timetables = new List<TimetableForTheDay>();
            for (int i = 0; i < tasks.Count; i++)
            {
                TimetableForTheDay timetable = new TimetableForTheDay();
                timetable.DateTimetable = date;
                timetable.Case = tasks[i].Title;
                timetable.Description = tasks[i].Description;
                timetable.IsItDone = 0;
                timetables.Add(timetable);
            }
            dataBase.InsertAll(timetables);
        }

        private void DeleteTask(TimetableForTheDay timetable)
        {
            dataBase.Delete<TimetableForTheDay>(timetable.Id);
        }

        private void UpdateTask(TimetableForTheDay timetable, TaskForTheDay task)
        {
            timetable.Case = task.Title;
            timetable.Description = task.Description;
            dataBase.Update(timetable);
        }

        private void ToList()
        {
            IEnumerator<TimetableForTheDay> iterator = ListOfCases.GetEnumerator();
            while (iterator.MoveNext())
            {
                timetablesForTheDay.Add(iterator.Current);
            }
        }

        private void StandartActions()
        {
            ListOfCases = dataBase.Table<TimetableForTheDay>();
            NumberOfCases = dataBase.Table<TimetableForTheDay>().Count();
            timetablesForTheDay = new List<TimetableForTheDay>();
            if (NumberOfCases > 0)
            {
                ToList();
            }
        }
    }
}