using Diary.Another;
using Diary.Model;
using Diary.Model.ImportantDatesLogic;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Diary.ViewModel
{
    class AllTimetablesViewModel : ViewModelBase
    {
        private TimetableForTheDaysLogic timetableForTheDaysLogic;
        private NewTimetableViewModel newTimetableViewModel;
        private Calendar calendar;
        private string selectedMonth;
        private int selectedYear;
        private int pastSelect;
        private Dispatcher dispatcher;
        private int pastIndex;

        public AllTimetablesViewModel() { }

        public AllTimetablesViewModel(TimetableForTheDaysLogic timetableForTheDaysLogic)
        {
            this.timetableForTheDaysLogic = timetableForTheDaysLogic;
            Condition = "Visible";
            ListOfTasks = new ObservableCollection<TaskForTheDay>();
            calendar = new Calendar();
            SelectedDay = new RelayCommand<int>(ViewTasks);
            SeeDescription = new RelayCommand<int>(See);
            Check = new RelayCommand<int>(Done);
            DeleteTask = new RelayCommand<int>(Delete);
            Months = new List<string>()
            {
                "Январь",
                "Февраль",
                "Март",
                "Апрель",
                "Май",
                "Июнь",
                "Июль",
                "Август",
                "Сентябрь",
                "Октябрь",
                "Ноябрь",
                "Декабрь"
            };
            Years = new LinkedList<int>();
            for (int i = 1900; i <= 2328; i++)
            {
                Years.AddLast(i);
            }
            selectedMonth = calendar.GetMonthNow();
            selectedYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            pastSelect = 0;
            StandardActions(Convert.ToInt32(DateTime.Now.ToString("dd")));
            dispatcher = Dispatcher.CurrentDispatcher;
            pastIndex = -1;
            IsClose = false;
        }

        public string Condition { get; set; }

        public List<string> Months { get; set; }

        public LinkedList<int> Years { get; set; }

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                ViewCalendar();
                ViewTasks(1);
            }
        }

        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                ViewCalendar();
                ViewTasks(1);
            }
        }

        public ObservableCollection<DayViewModel> DaysOfMonth { get; set; }

        public ObservableCollection<Event> Events { get; set; }

        public RelayCommand<int> SelectedDay { get; }

        public ObservableCollection<TaskForTheDay> ListOfTasks { get; set; }

        public int Count { get; set; }

        public RelayCommand<int> SeeDescription { get; }

        public RelayCommand<int> Check { get; }

        public RelayCommand<int> DeleteTask { get; }

        public bool IsNeed { get; set; }

        public string DescriptionByThisIndex { get; set; }

        public ICommand Previous
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedMonth == "Январь")
                    {
                        SelectedMonth = "Декабрь";
                        SelectedYear -= 1;
                        ViewTasks(1);
                    }
                    else
                    {
                        int count = 0;
                        while (count != Months.Count)
                        {
                            if (Months[count] == SelectedMonth)
                            {
                                SelectedMonth = Months[count - 1];
                                ViewTasks(1);
                                break;
                            }
                            count++;
                        }
                    }
                });
            }
        }

        public ICommand Next
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedMonth == "Декабрь")
                    {
                        SelectedMonth = "Январь";
                        SelectedYear += 1;
                        ViewTasks(1);
                    }
                    else
                    {
                        int count = 0;
                        while (count != Months.Count)
                        {
                            if (Months[count] == SelectedMonth)
                            {
                                SelectedMonth = Months[count + 1];
                                ViewTasks(1);
                                break;
                            }
                            count++;
                        }
                    }
                });
            }
        }

        public ICommand BackSpace
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Condition = "Collapsed";
                });
            }
        }

        public ICommand EditTasks
        {
            get
            {
                return new RelayCommand(() =>
                {
                    newTimetableViewModel = new NewTimetableViewModel(timetableForTheDaysLogic, pastSelect, calendar.GetSecondVariantNameOfMonths(SelectedMonth), SelectedYear);
                    EditTimetable = new NewTimetableView();
                    EditTimetable.DataContext = newTimetableViewModel;
                    WaitingNextStepAsync();
                });
            }
        }

        public ICommand AllDelete
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (new MessageBoxViewModel("Вы действительно хотите очистить список задач?", "Удаление").ShowMessage() == true)
                    {
                        Thread thread = new Thread(new ThreadStart(DeleteTimetable));
                        thread.Start();
                    }
                });
            }
        }

        public bool CanAreEdit { get; set; }

        public FrameworkElement EditTimetable { get; set; }

        public bool IsClose { get; set; }

        private async void WaitingNextStepAsync()
        {
            await Task.Run( () =>
            {
                while (IsClose != true && newTimetableViewModel.Condition != "Collapsed") { }
                if (newTimetableViewModel.Condition == "Collapsed")
                {
                    StandardActions(pastSelect);
                }
            });
        }

        private void ViewCalendar()
        {
            DayViewModel.daysCount = calendar.GetNumberOfDaysInThisMonth(SelectedMonth, SelectedYear);
            int stepFirstDay = 6 - (7 - calendar.GetNumberDayOfWeek(1, SelectedMonth, SelectedYear));
            if (stepFirstDay == -1)
            {
                stepFirstDay = 6;
            }
            DaysOfMonth = new ObservableCollection<DayViewModel>();
            int index = 0;
            for (int i = 1; i <= DayViewModel.daysCount; i++)
            {
                if (stepFirstDay > 0)
                {
                    DaysOfMonth.Add(new DayViewModel("Collapsed"));
                    stepFirstDay--;
                    i--;
                }
                else
                {
                    if (calendar.GetNameDayOfWeek(i, SelectedMonth, SelectedYear) == "Суббота" || calendar.GetNameDayOfWeek(i, SelectedMonth, SelectedYear) == "Воскресенье")
                    {
                        if (timetableForTheDaysLogic.IsHaveTasks(ZeroOrNull(i) + i.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear))
                        {
                            DaysOfMonth.Add(new DayViewModel(i, "Red", "Visible"));
                        }
                        else if (i >= 24 && i <= 30 && SelectedMonth == "Ноябрь" && calendar.GetNameDayOfWeek(i, SelectedMonth, SelectedYear) == "Воскресенье")
                        {
                            DaysOfMonth.Add(new DayViewModel(i, "Red", "Visible"));
                        }
                        else
                        {
                            DaysOfMonth.Add(new DayViewModel(i, "Red", "Collapsed"));
                        }
                    }
                    else
                    {
                        if (timetableForTheDaysLogic.IsHaveTasks(ZeroOrNull(i) + i.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear))
                        {
                            DaysOfMonth.Add(new DayViewModel(i, "Visible"));
                        }
                        else
                        {
                            DaysOfMonth.Add(new DayViewModel(i, "Collapsed"));
                        }
                    }
                }
                index++;
            }
        }

        private void ViewTasks(int number)
        {
            IsNeed = false;
            if (timetableForTheDaysLogic.NumberOfCases > 0)
            {
                ListOfTasks = timetableForTheDaysLogic.GetTasksInThisDate(ZeroOrNull(number) + number.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString());
            }
            CanAreEdit = IsNowDate(number);
            Select(number);
            Count = ListOfTasks.Count;
        }

        private void See(int index)
        {
            if (pastIndex == index)
            {
                if (IsNeed)
                {
                    IsNeed = false;
                }
                else
                {
                    IsNeed = true;
                }
            }
            else
            {
                IsNeed = true;
            }
            DescriptionByThisIndex = ElementByIndex(index).Description;
            pastIndex = index;
        }

        private void Done(int index)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(RewriteData));
            thread.Start(index);
        }

        private void Delete(int index)
        {
            if (new MessageBoxViewModel("Вы действительно хотите удалить задачу?", "Удаление").ShowMessage() == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(DeleteData));
                thread.Start(index);
            }
        }

        private void RewriteData(object id)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                timetableForTheDaysLogic.UpdateTask(ElementByIndex((int)id), ZeroOrNull(pastSelect) + pastSelect.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString());
            });
        }

        private void DeleteData(object id)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                timetableForTheDaysLogic.Distribution(ToList((int)id), ZeroOrNull(pastSelect) + pastSelect.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString());
                StandardActions(pastSelect);
            });
        }

        private void DeleteTimetable()
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                timetableForTheDaysLogic.Distribution(new List<TaskForTheDay>() { ElementByIndex(0) }, ZeroOrNull(pastSelect) + pastSelect.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString());
                ListOfTasks.Clear();
                StandardActions(pastSelect);
            });
        }

        private void Select(int select)
        {
            if (pastSelect != 0)
            {
                IEnumerator<DayViewModel> iteratorPast = DaysOfMonth.GetEnumerator();
                while (iteratorPast.MoveNext())
                {
                    if (iteratorPast.Current.NumberDayOfWeek == pastSelect)
                    {
                        iteratorPast.Current.ColorBack = (Brush)new BrushConverter().ConvertFromString("LightGoldenrodYellow");
                        break;
                    }
                }
            }
            IEnumerator<DayViewModel> iterator = DaysOfMonth.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.NumberDayOfWeek == select)
                {
                    iterator.Current.ColorBack = (Brush)new BrushConverter().ConvertFromString("PaleTurquoise");
                    pastSelect = select;
                    break;
                }
            }
        }

        private string ZeroOrNull(int number)
        {
            if (number < 10)
            {
                return "0";
            }
            else
            {
                return "";
            }
        }

        private void StandardActions(int day)
        {
            ViewCalendar();
            ViewTasks(day);
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

        private List<TaskForTheDay> ToList(int index)
        {
            List<TaskForTheDay> tasksForTheDay = new List<TaskForTheDay>();
            TaskForTheDay deleteTask = new TaskForTheDay(-1);
            IEnumerator<TaskForTheDay> iterator = ListOfTasks.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Index == index)
                {
                    deleteTask = iterator.Current;
                }
                else
                {
                    tasksForTheDay.Add(iterator.Current);
                }
            }
            ListOfTasks.Remove(deleteTask);
            return tasksForTheDay;
        }

        private bool IsNowDate(int number)
        {
            if (Convert.ToInt32(DateTime.Now.ToString("yyyy")) < SelectedYear)
            {
                return true;
            }
            else if (Convert.ToInt32(DateTime.Now.ToString("MM")) < Convert.ToInt32(calendar.GetNumberOfMonth(SelectedMonth)) && Convert.ToInt32(DateTime.Now.ToString("yyyy")) == SelectedYear)
            {
                return true;
            }
            else if (Convert.ToInt32(DateTime.Now.ToString("MM")) == Convert.ToInt32(calendar.GetNumberOfMonth(SelectedMonth)) && 
                Convert.ToInt32(DateTime.Now.ToString("yyyy")) == SelectedYear &&
                Convert.ToInt32(DateTime.Now.ToString("dd")) <= number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}