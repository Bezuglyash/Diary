using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using Diary.Another;
using Diary.Another.Tracker;
using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Diary.ViewModel
{
    class AllHabitsTrackerViewModel : ViewModelBase
    {
        private HabitsTrackerLogic habitsTrackerLogic;
        private Day workOfDay;
        private int day;
        private int month;
        private int year;
        private string nowWeek;
        private Dispatcher dispatcher;

        public AllHabitsTrackerViewModel() { }

        public AllHabitsTrackerViewModel(HabitsTrackerLogic habitsTrackerLogic)
        {
            this.habitsTrackerLogic = habitsTrackerLogic;
            workOfDay = new Day();
            Condition = "Visible";
            day = Convert.ToInt32(DateTime.Now.ToString("dd"));
            month = Convert.ToInt32(DateTime.Now.ToString("MM"));
            year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            WeekHabits = new ObservableCollection<WeekHabit>();
            WeekHabit.ListDays.Clear();
            WeekHabit.SetWeek(DateTime.Now.ToString("dd"), DateTime.Now.ToString("MM"), year);
            DayMonth = WeekHabit.Week;
            Completion();
            MondayCheck = new RelayCommand<string>(MonCheck);
            TuesdayCheck = new RelayCommand<string>(TueCheck);
            WednesdayCheck = new RelayCommand<string>(WedCheck);
            ThursdayCheck = new RelayCommand<string>(ThuCheck);
            FridayCheck = new RelayCommand<string>(FriCheck);
            SaturdayCheck = new RelayCommand<string>(SatCheck);
            SundayCheck = new RelayCommand<string>(SunCheck);
            IsVisible = false;
            nowWeek = DayMonth;
            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public ObservableCollection<WeekHabit> WeekHabits { get; set; }

        public int Count { get; set; }

        public string Condition { get; set; }

        public string DayMonth { get; set; }

        public bool IsVisible { get; set; }

        public RelayCommand<string> MondayCheck { get; }

        public RelayCommand<string> TuesdayCheck { get; }

        public RelayCommand<string> WednesdayCheck { get; }

        public RelayCommand<string> ThursdayCheck { get; }

        public RelayCommand<string> FridayCheck { get; }

        public RelayCommand<string> SaturdayCheck { get; }

        public RelayCommand<string> SundayCheck { get; }

        public ICommand Previous
        {
            get
            {
                return new RelayCommand(() =>
                {
                    WeekHabit.ListDays.Clear();
                    if (day - 7 > 0)
                    {
                        string thisMonth = month < 10 ? "0" + month.ToString() : month.ToString();
                        WeekHabit.SetWeek((day - 7).ToString(), thisMonth, year);
                        day -= 7;
                    }
                    else
                    {
                        string previousMonth;
                        if (month - 1 != 0)
                        {
                            previousMonth = month - 1 < 10 ? "0" + (month - 1).ToString() : (month - 1).ToString();
                        }
                        else
                        {
                            previousMonth = "12";
                            year -= 1;
                        }
                        int countDays = workOfDay.GetNumberOfDaysInThisMonth(workOfDay.GetMonth(previousMonth), year);
                        day = countDays + (day - 7);
                        WeekHabit.SetWeek(day.ToString(), previousMonth, year);
                        month = Convert.ToInt32(previousMonth);
                    }
                    IsVisible = true;
                    DayMonth = WeekHabit.Week;
                    WeekHabits.Clear();
                    Completion();
                });
            }
        }

        public ICommand Next
        {
            get
            {
                return new RelayCommand(() =>
                {
                    WeekHabit.ListDays.Clear();
                    string thisMonth = month < 10 ? "0" + month.ToString() : month.ToString();
                    if (day + 7 < workOfDay.GetNumberOfDaysInThisMonth(workOfDay.GetMonth(thisMonth), year))
                    {
                        WeekHabit.SetWeek((day + 7).ToString(), thisMonth, year);
                        day += 7;
                    }
                    else
                    {
                        string nextMonth;
                        if (month + 1 != 13)
                        {
                            nextMonth = month + 1 < 10 ? "0" + (month + 1).ToString() : (month + 1).ToString();
                        }
                        else
                        {
                            nextMonth = "01";
                            year += 1;
                        }
                        int countDays = workOfDay.GetNumberOfDaysInThisMonth(workOfDay.GetMonth(thisMonth), year);
                        day = day + 7 - countDays;
                        WeekHabit.SetWeek(day.ToString(), nextMonth, year);
                        month = Convert.ToInt32(nextMonth);
                    }
                    DayMonth = WeekHabit.Week;
                    if (DayMonth == nowWeek)
                    {
                        IsVisible = false;
                    }
                    WeekHabits.Clear();
                    Completion();
                });
            }
        }

        public ICommand BackSpace
        {
            get
            {
                return new RelayCommand(() =>
                {
                    WeekHabit.ListDays.Clear();
                    Condition = "Collapsed";
                });
            }
        }

        private void MonCheck(string text)
        {
            Thread thread = new Thread(delegate () { UpdateTracker(text, WeekHabit.ListDays[0]); });
            thread.Start();
        }

        private void TueCheck(string text)
        {
            Thread thread = new Thread(delegate () { UpdateTracker(text, WeekHabit.ListDays[1]); });
            thread.Start();
        }

        private void WedCheck(string text)
        {
            Thread thread = new Thread(delegate () { UpdateTracker(text, WeekHabit.ListDays[2]); });
            thread.Start();
        }

        private void ThuCheck(string text)
        {
            Thread thread = new Thread(delegate () { UpdateTracker(text, WeekHabit.ListDays[3]); });
            thread.Start();
        }

        private void FriCheck(string text)
        {
            Thread thread = new Thread(delegate () { UpdateTracker(text, WeekHabit.ListDays[4]); });
            thread.Start();
        }

        private void SatCheck(string text)
        {
            Thread thread = new Thread(delegate () { UpdateTracker(text, WeekHabit.ListDays[5]); });
            thread.Start();
        }

        private void SunCheck(string text)
        {
            Thread thread = new Thread(delegate () { UpdateTracker(text, WeekHabit.ListDays[6]); });
            thread.Start();
        }

        private void Completion()
        {
            WeekHabits = habitsTrackerLogic.GetWeekHabits(WeekHabit.ListDays);
            Count = WeekHabits.Count;
        }

        private void UpdateTracker(string text, string date)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                habitsTrackerLogic.UpdateHabit(text, date);
            });
        }
    }
}