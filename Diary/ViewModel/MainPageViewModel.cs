using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using Diary.Another;
using Diary.Another.Tracker;
using Diary.DataBase;
using Diary.Model;
using Diary.Model.ImportantDatesLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Diary.ViewModel
{
    class MainPageViewModel : ViewModelBase
    {
        private readonly ImportantDatesLogic importantDatesLogic;
        private readonly TimetableForTheDaysLogic timetableForTheDaysLogic;
        private readonly HabitsTrackerLogic habitsTrackerLogic;
        private readonly GoalsLogic goalsLogic;
        private Calendar calendar;
        private Dispatcher dispatcher;

        public MainPageViewModel() { }

        public MainPageViewModel((ImportantDatesLogic, TimetableForTheDaysLogic, HabitsTrackerLogic, GoalsLogic) logics)
        {
            importantDatesLogic = logics.Item1;
            timetableForTheDaysLogic = logics.Item2;
            habitsTrackerLogic = logics.Item3;
            goalsLogic = logics.Item4;
            dispatcher = Dispatcher.CurrentDispatcher;

            Goals = new ObservableCollection<Goal>(this.goalsLogic.GetAllGoals());
            GoalCheck = new RelayCommand<int>(Check);
            CountGoals = Goals.Count;

            WeekHabits = new ObservableCollection<WeekHabit>();
            WeekHabit.ListDays.Clear();
            DayOfWeek = Another.Day.GetDay(DateTime.Now.ToString("dd.MM.yyyy"));
            Completion();
            CheckDay = new RelayCommand<string>(Checked);

            calendar = new Calendar();
            ViewEvents();

            ListOfTasks = new ObservableCollection<TaskForTheDay>();
            CheckTask = new RelayCommand<int>(Done);
            ViewTasks();

            Day = Convert.ToInt32(DateTime.Now.ToString("dd"));
            Month = calendar.GetSecondVariantNameOfMonths(calendar.GetMonthNow());
            Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
        }

        // Цели
        public ObservableCollection<Goal> Goals { get; set; }

        public int CountGoals { get; set; }

        public string DayOfWeek { get; set; }

        public RelayCommand<int> GoalCheck { get; }

        private void Check(int id)
        {
            goalsLogic.UpdateGoal(id, Goals[GetIndexById(id)].IsDone);
        }

        private int GetIndexById(int id)
        {
            int index = 0;
            foreach (var goal in Goals)
            {
                if (goal.Id == id)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        // Трекер привычек
        public ObservableCollection<WeekHabit> WeekHabits { get; set; }

        public int CountHabits { get; set; }

        public RelayCommand<string> CheckDay { get; }

        private void Checked(string text)
        {
            Thread thread = new Thread(delegate () { UpdateTracker(text, WeekHabit.ListDays[0]); });
            thread.Start();
        }

        private void Completion()
        {
            WeekHabit.ListDays.Add(DateTime.Now.ToString("dd.MM.yyyy"));
            WeekHabits = habitsTrackerLogic.GetWeekHabits(WeekHabit.ListDays);
            CountHabits = WeekHabits.Count;
        }

        private void UpdateTracker(string text, string date)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                habitsTrackerLogic.UpdateHabit(text, date);
            });
        }

        // События
        public ObservableCollection<Event> Events { get; set; }

        public int CountEvents { get; set; }

        private void ViewEvents()
        {
            Events = new ObservableCollection<Event>();
            List<string> events = new List<string>();

            string day = DateTime.Now.ToString("dd");
            string month = calendar.GetMonth(DateTime.Now.ToString("MM"));
            int year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

            events = importantDatesLogic.GetEvents(day + "." + DateTime.Now.ToString("MM"), year);
            if (calendar.IsThisLeapYear(year) == false && month == "Март" && Convert.ToInt32(day) == 1 && importantDatesLogic.IsHaveEvents(29.ToString() + "." + calendar.GetNumberOfMonth("Февраль"), year))
            {
                events.AddRange(importantDatesLogic.GetEvents(29.ToString() + "." + calendar.GetNumberOfMonth("Февраль"), year));
            }
            if (Convert.ToInt32(day) >= 24 && Convert.ToInt32(day) <= 30 && month == "Ноябрь")
            {
                events.Add("День матери");
            }

            foreach (var current in events)
            {
                Event eventer = new Event();
                eventer.TextEvent = current;
                Events.Add(eventer);
            }

            CountEvents = Events.Count;
        }

        // Задачи
        public ObservableCollection<TaskForTheDay> ListOfTasks { get; set; }

        public int CountTasks { get; set; }

        public RelayCommand<int> CheckTask { get; }

        private void ViewTasks()
        {
            if (timetableForTheDaysLogic.NumberOfCases > 0)
            {
                ListOfTasks = timetableForTheDaysLogic.GetTasksInThisDate(DateTime.Now.ToString("dd") + "." + DateTime.Now.ToString("MM") + "." + DateTime.Now.ToString("yyyy"));
            }
            CountTasks = ListOfTasks.Count;
        }

        private void Done(int index)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(RewriteData));
            thread.Start(index);
        }

        private void RewriteData(object id)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                timetableForTheDaysLogic.UpdateTask(ElementByIndex((int)id), DateTime.Now.ToString("dd") + "." + DateTime.Now.ToString("MM") + "." + DateTime.Now.ToString("yyyy"));
            });
        }

        private TaskForTheDay ElementByIndex(int index)
        {
            IEnumerator<TaskForTheDay> iterator = ListOfTasks.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Index == index)
                {
                    return iterator.Current;
                }
            }
            return null;
        }

        // Дата
        public int Day { get; set; }

        public string Month { get; set; }

        public int Year { get; set; }
    }
}