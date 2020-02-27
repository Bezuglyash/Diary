using Diary.Another;
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
    class AllImportantDatesViewModel : ViewModelBase
    {
        private ImportantDatesLogic importantDatesLogic;
        private Calendar calendar;
        private string selectedMonth;
        private int selectedYear;
        private int selectedEvent;
        private int pastSelect;
        private Dispatcher dispatcher;
        private NewImportantDateViewModel newImportantDateViewModel;

        public AllImportantDatesViewModel() { }

        public AllImportantDatesViewModel(ImportantDatesLogic importantDatesLogic)
        {
            Condition = "Visible";
            this.importantDatesLogic = importantDatesLogic;
            calendar = new Calendar();
            SelectedDay = new RelayCommand<int>(ViewEvents);
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
                ViewEvents(1);
            }
        }

        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                ViewCalendar();
                ViewEvents(1);
            }
        }

        public ObservableCollection<DayViewModel> DaysOfMonth { get; set; }

        public ObservableCollection<Event> Events{ get; set; }

        public RelayCommand<int> SelectedDay { get; }

        public int SelectedEvent
        {
            get { return selectedEvent; }
            set
            {
                selectedEvent = value;
                if (selectedEvent != -1)
                {
                    EventText = GetEvent(Convert.ToInt32(selectedEvent));
                    if (pastSelect == 1 && SelectedMonth == "Март")
                    {
                        string[] partsDate = importantDatesLogic.GetEventDate(EventText, ZeroOrNull(pastSelect) + pastSelect.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth));
                        Annually = importantDatesLogic.GetAnnually(partsDate[0] + "." + partsDate[1], EventText);
                    }
                    else
                    {
                        Annually = importantDatesLogic.GetAnnually(ZeroOrNull(pastSelect) + pastSelect.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth), EventText);
                    }
                }
            }
        }

        public int Count { get; set; }

        public string EventText { get; set; }

        public int Annually { get; set; }

        public ICommand DeleteImportantDate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (new MessageBoxViewModel("Вы действительно желаете удалить данное событие?", "Удаление").ShowMessage() == true)
                    {
                        importantDatesLogic.DeleteToList(EventText, ZeroOrNull(pastSelect) + pastSelect.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth));
                        StandardActions(pastSelect);
                        Thread thread = new Thread(UpdateImportantDate);
                        thread.Start();
                    }
                });
            }
        }

        public ICommand EditImportantDate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (EventText == "День матери")
                    {
                        newImportantDateViewModel = new NewImportantDateViewModel(importantDatesLogic, 25, "Ноября", 1900, EventText, 1);
                    }
                    else
                    {
                        string[] partsDate = importantDatesLogic.GetEventDate(EventText, ZeroOrNull(pastSelect) + pastSelect.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth));
                        string nameMonth = calendar.GetMonthNow(partsDate[1]);
                        newImportantDateViewModel = new NewImportantDateViewModel(importantDatesLogic, Convert.ToInt32(partsDate[0]), calendar.GetSecondVariantNameOfMonths(nameMonth), Convert.ToInt32(partsDate[2]), EventText, Annually);
                    }
                    AddEvent = new NewImportantDateView();
                    AddEvent.DataContext = newImportantDateViewModel;
                    importantDatesLogic.UpdateDataAndList();
                    WaitingNextStepAsync();
                });
            }
        }

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
                        ViewEvents(1);
                    }
                    else
                    {
                        int count = 0;
                        while (count != Months.Count)
                        {
                            if (Months[count] == SelectedMonth)
                            {
                                SelectedMonth = Months[count - 1];
                                ViewEvents(1);
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
                        ViewEvents(1);
                    }
                    else
                    {
                        int count = 0;
                        while (count != Months.Count)
                        {
                            if (Months[count] == SelectedMonth)
                            {
                                SelectedMonth = Months[count + 1];
                                ViewEvents(1);
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

        public ICommand AddNewEvent
        {
            get
            {
                return new RelayCommand(() =>
                {
                    newImportantDateViewModel = new NewImportantDateViewModel(importantDatesLogic, pastSelect, calendar.GetSecondVariantNameOfMonths(SelectedMonth), SelectedYear);
                    AddEvent = new NewImportantDateView();
                    AddEvent.DataContext = newImportantDateViewModel;
                    WaitingNextStepAsync();
                });
            }
        }

        public FrameworkElement AddEvent { get; set; }

        public bool IsClose { get; set; }

        private async void WaitingNextStepAsync()
        {
            await Task.Run(() =>
            {
                while (IsClose != true && newImportantDateViewModel.Condition != "Collapsed") { }
                if (newImportantDateViewModel.Condition == "Collapsed")
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
                        if (importantDatesLogic.IsHaveEvents(ZeroOrNull(i) + i.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth), SelectedYear))
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
                        if (importantDatesLogic.IsHaveEvents(ZeroOrNull(i) + i.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth), SelectedYear))
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
                if (calendar.IsThisLeapYear(SelectedYear) == false && SelectedMonth == "Март" && i == 1)
                {
                    if (importantDatesLogic.IsHaveEvents(29.ToString() + "." + calendar.GetNumberOfMonth("Февраль"), SelectedYear))
                    {
                        DaysOfMonth[index - 1].MarkingThePresenceOfEvents = "Visible";
                    }
                }
            }
        }

        private void ViewEvents(int selectedDay)
        {
            Events = new ObservableCollection<Event>();
            List<string> events = new List<string>();

            events = importantDatesLogic.GetEvents(ZeroOrNull(selectedDay) + selectedDay.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth), SelectedYear);
            if (calendar.IsThisLeapYear(SelectedYear) == false && SelectedMonth == "Март" && selectedDay == 1 && importantDatesLogic.IsHaveEvents(ZeroOrNull(29) + 29.ToString() + "." + calendar.GetNumberOfMonth("Февраль"), SelectedYear))
            {
                events.AddRange(importantDatesLogic.GetEvents(29.ToString() + "." + calendar.GetNumberOfMonth("Февраль"), SelectedYear));
            }
            if (selectedDay >= 24 && selectedDay <= 30 && SelectedMonth == "Ноябрь")
            {
                events.Add("День матери");
            }

            foreach (var current in events)
            {
                Event eventer = new Event();
                eventer.TextEvent = current;
                Events.Add(eventer);
            }

            Count = Events.Count;
            SelectedEvent = -1;
            Select(selectedDay);
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

        private void UpdateImportantDate()
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                importantDatesLogic.DeleteEvent(EventText, ZeroOrNull(pastSelect) + pastSelect.ToString() + "." + calendar.GetNumberOfMonth(SelectedMonth));
            });
        }

        private string GetEvent (int index)
        {
            IEnumerator<Event> iterator = Events.GetEnumerator();
            int count = 0;
            while (iterator.MoveNext())
            {
                if (count == index)
                {
                    return iterator.Current.TextEvent;
                }
                count++;
            }
            return null;
        }

        private string ZeroOrNull (int number)
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
            ViewEvents(day);
        }
    }
}