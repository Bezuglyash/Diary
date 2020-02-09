using Diary.Another;
using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace Diary.ViewModel
{
    class NewTimetableViewModel : ViewModelBase
    {
        private TimetableForTheDaysLogic timetableForTheDaysLogic;
        private Day day;
        private int selectedDay;
        private string selectedMonth;
        private int selectedYear;
        private int counter;
        private TaskForTheDay taskForTheDay;
        private Dispatcher dispatcher;
        private int dispatcherHelp;

        public NewTimetableViewModel() { }

        public NewTimetableViewModel(TimetableForTheDaysLogic timetableForTheDaysLogic)
        {
            this.timetableForTheDaysLogic = timetableForTheDaysLogic;
            Condition = "Visible";
            ListOfTasks = new ObservableCollection<TaskForTheDay>();
            AddDescription = new RelayCommand<int>(DescriptionInThisIndex);
            DeleteTask = new RelayCommand<int>(DeleteAtThisIndex);
            day = new Day();
            Months = new ObservableCollection<string>()
            {
                "Января",
                "Февраля",
                "Марта",
                "Апреля",
                "Мая",
                "Июня",
                "Июля",
                "Августа",
                "Сентября",
                "Октября",
                "Ноября",
                "Декабря"
            };
            selectedMonth = day.GetMonthNow();
            ClippingMonths();
            selectedYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            Years = new LinkedList<int>();
            for (int i = selectedYear; i <= 2328; i++)
            {
                Years.AddLast(i);
            }
            selectedDay = Convert.ToInt32(DateTime.Now.ToString("dd"));
            Days = new ObservableCollection<int>();
            ClippingDays(selectedDay);
            counter = 0;
            SearchingExistingCases();
            dispatcher = Dispatcher.CurrentDispatcher;
            dispatcherHelp = 1;
        }

        public NewTimetableViewModel(TimetableForTheDaysLogic timetableForTheDaysLogic, int numberOfDay, string month, int year)
        {
            this.timetableForTheDaysLogic = timetableForTheDaysLogic;
            Condition = "Visible";
            ListOfTasks = new ObservableCollection<TaskForTheDay>();
            AddDescription = new RelayCommand<int>(DescriptionInThisIndex);
            DeleteTask = new RelayCommand<int>(DeleteAtThisIndex);
            day = new Day();
            Months = new ObservableCollection<string>()
            {
                "Января",
                "Февраля",
                "Марта",
                "Апреля",
                "Мая",
                "Июня",
                "Июля",
                "Августа",
                "Сентября",
                "Октября",
                "Ноября",
                "Декабря"
            };
            selectedMonth = month;
            ClippingMonths();
            selectedYear = year;
            Years = new LinkedList<int>();
            for (int i = Convert.ToInt32(DateTime.Now.ToString("yyyy")); i <= 2328; i++)
            {
                Years.AddLast(i);
            }
            selectedDay = numberOfDay;
            Days = new ObservableCollection<int>();
            ClippingDays(selectedDay);
            counter = 0;
            SearchingExistingCases();
            Count = ListOfTasks.Count;
            dispatcher = Dispatcher.CurrentDispatcher;
            dispatcherHelp = 0;
        }

        public string Condition { get; set; }

        public ObservableCollection<TaskForTheDay> ListOfTasks { get; set; }

        public int Count { get; set; }

        public ObservableCollection<int> Days { get; set; }

        public ObservableCollection<string> Months { get; set; }

        public LinkedList<int> Years { get; set; }

        public int SelectedDay
        {
            get { return selectedDay; }
            set
            {
                selectedDay = value;
                StandartOperations();
            }
        }

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                UpdateDays();
                StandartOperations();
            }
        }

        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                if (selectedYear == Convert.ToInt32(DateTime.Now.ToString("yyyy")))
                {
                    if (Convert.ToInt32(day.GetNumberOfMonth(SelectedMonth)) < Convert.ToInt32(DateTime.Now.ToString("MM")))
                    {
                        SelectedMonth = day.GetMonthNow();
                    }
                    else
                    {
                        UpdateDays();
                    }
                    ClippingMonths();
                }
                else
                {
                    Months = new ObservableCollection<string>()
                    {
                        "Января",
                        "Февраля",
                        "Марта",
                        "Апреля",
                        "Мая",
                        "Июня",
                        "Июля",
                        "Августа",
                        "Сентября",
                        "Октября",
                        "Ноября",
                        "Декабря"
                    };
                    StandartOperations();
                    UpdateDays();
                }
            }
        }

        public RelayCommand<int> AddDescription { get; }

        public RelayCommand<int> DeleteTask { get; }

        public ICommand AddNewTask
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ListOfTasks.Add(new TaskForTheDay(counter));
                    counter++;
                    Count = ListOfTasks.Count;
                });
            }
        }

        public bool IsNeed { get; set; }

        public string DescriptionByThisIndex { get; set; }

        public ICommand AddThisDescription
        {
            get
            {
                return new RelayCommand(() =>
                {
                    IsNeed = false;
                    taskForTheDay.IsNeedThis = false;
                    EqualTask();
                });
            }
        }

        public ICommand SaveTimetable
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (IsNotNull() == true)
                    {
                        if (dispatcherHelp == 1)
                        {
                            Thread thread = new Thread(new ThreadStart(RewriteData));
                            thread.Start();
                        }
                        else
                        {
                            timetableForTheDaysLogic.Distribution(ToList(), ZeroOrNull() + SelectedDay.ToString() + "." + day.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString());
                        }
                        Condition = "Collapsed";
                    }
                    else
                    {
                        new MessageBoxViewModel("Не все задачи заполнены! Сохранение невозможно!", "Предупреждение").ShowWarning();
                    }
                });
            }
        }

        public ICommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Condition = "Collapsed";
                });
            }
        }

        private void DescriptionInThisIndex (int index)
        {
            if (IsNeed == true)
            {
                IsNeed = false;
                taskForTheDay.IsNeedThis = false;
                EqualTask();
            }
            ElementByIndex(index);
            IsNeed = true;
            taskForTheDay.IsNeedThis = true;
            DescriptionByThisIndex = taskForTheDay.Description;
        }

        private void DeleteAtThisIndex(int index)
        {
            ElementByIndex(index);
            if (taskForTheDay.IsNeedThis == true)
            {
                IsNeed = false;
                taskForTheDay.IsNeedThis = false;
            }
            ListOfTasks.Remove(taskForTheDay);
        }

        private void SearchingExistingCases()
        {
            if (timetableForTheDaysLogic.NumberOfCases > 0)
            {
                ListOfTasks = timetableForTheDaysLogic.GetTasksInThisDate(ZeroOrNull() + SelectedDay.ToString() + "." + day.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString());
            }
        }

        private void UpdateDays()
        {
            int interimDay = SelectedDay;
            Days.Clear();
            if (SelectedYear == Convert.ToInt32(DateTime.Now.ToString("yyyy")) && SelectedMonth == day.GetMonthNow())
            {
                ClippingDays(Convert.ToInt32(DateTime.Now.ToString("dd")));
                if (interimDay < Convert.ToInt32(DateTime.Now.ToString("dd")) || interimDay > day.GetNumberOfDaysInThisMonth(SelectedMonth, SelectedYear))
                {
                    SelectedDay = Convert.ToInt32(DateTime.Now.ToString("dd"));
                }
                else
                {
                    UpdateDay(interimDay);
                }
            }
            else
            {
                ClippingDays();
                if (interimDay > Days.Count)
                {
                    SelectedDay = 1;
                }
                else
                {
                    UpdateDay(interimDay);
                }
            }
        }

        private void RewriteData()
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                timetableForTheDaysLogic.Distribution(ToList(), ZeroOrNull() + SelectedDay.ToString() + "." + day.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString());
            });
        }

        private string ZeroOrNull()
        {
            if (SelectedDay < 10)
            {
                return "0";
            }
            else
            {
                return "";
            }
        }

        private void UpdateDay(int day)
        {
            selectedDay = 1 + day;
            SelectedDay -= 1;
        }

        private void ClippingDays(int firstDay = 1)
        {
            for (int i = firstDay; i <= day.GetNumberOfDaysInThisMonth(SelectedMonth, SelectedYear); i++)
            {
                Days.Add(i);
            }
        }

        private void ClippingMonths()
        {
            for (int i = 0; i < Convert.ToInt32(DateTime.Now.ToString("MM")) - 1; i++)
            {
                Months.RemoveAt(Convert.ToInt32(DateTime.Now.ToString("MM")) - 2 - i);
            }
        }

        private void StandartOperations()
        {
            ListOfTasks.Clear();
            counter = 0;
            SearchingExistingCases();
            Count = ListOfTasks.Count;
        }

        private List<TaskForTheDay> ToList()
        {
            List<TaskForTheDay> tasksForTheDay = new List<TaskForTheDay>();
            IEnumerator<TaskForTheDay> iterator = ListOfTasks.GetEnumerator();
            while (iterator.MoveNext())
            {
                tasksForTheDay.Add(iterator.Current);
            }
            return tasksForTheDay;
        }

        private void ElementByIndex(int index)
        {
            IEnumerator<TaskForTheDay> iterator = ListOfTasks.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Index == index)
                {
                    taskForTheDay = iterator.Current;
                }
            }
        }

        private void EqualTask()
        {
            IEnumerator<TaskForTheDay> iterator = ListOfTasks.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Index == taskForTheDay.Index)
                {
                    iterator.Current.Description = DescriptionByThisIndex;
                }
            }
        }

        private bool IsNotNull()
        {
            IEnumerator<TaskForTheDay> iterator = ListOfTasks.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Title == "" || iterator.Current.IsOnlyOneGaps() == true)
                {
                    return false;
                }
            }
            return true;
        }
    }
}