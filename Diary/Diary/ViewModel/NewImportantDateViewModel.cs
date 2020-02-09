using Diary.Another;
using Diary.Model.ImportantDatesLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;

namespace Diary.ViewModel
{
    class NewImportantDateViewModel : ViewModelBase
    {
        private ImportantDatesLogic importantDatesLogic;
        private Day day;
        private int selectedDay;
        private string selectedMonth;
        private int selectedYear;
        private string text;
        private Dispatcher dispatcher;
        private int editId;

        public NewImportantDateViewModel() { }

        public NewImportantDateViewModel(ImportantDatesLogic importantDatesLogic)
        {
            this.importantDatesLogic = importantDatesLogic;
            Overall();
            selectedMonth = day.GetMonthNow();
            selectedYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            int count = day.GetNumberOfDaysInThisMonth(selectedMonth, selectedYear);
            for (int i = 1; i <= count; i++)
            {
                Days.Add(i);
            }
            SelectedDay = Convert.ToInt32(DateTime.Now.ToString("dd"));
            editId = -1;
        }

        public NewImportantDateViewModel(ImportantDatesLogic importantDatesLogic, int numberOfDay, string month, int year)
        {
            this.importantDatesLogic = importantDatesLogic;
            Overall();
            selectedMonth = month;
            selectedYear = year;
            int count = day.GetNumberOfDaysInThisMonth(month, year);
            for (int i = 1; i <= count; i++)
            {
                Days.Add(i);
            }
            SelectedDay = numberOfDay;
            editId = -1;
        }

        public NewImportantDateViewModel(ImportantDatesLogic importantDatesLogic, int numberOfDay, string month, int year, string eventText, int annually)
        {
            this.importantDatesLogic = importantDatesLogic;
            Overall();
            selectedMonth = month;
            selectedYear = year;
            int count = day.GetNumberOfDaysInThisMonth(month, year);
            for (int i = 1; i <= count; i++)
            {
                Days.Add(i);
            }
            SelectedDay = numberOfDay;
            Text = eventText;
            if (annually == 1)
            {
                IsAnnually = true;
            }
            else
            {
                IsAnnually = false;
            }
            editId = importantDatesLogic.GetIdByEventAndDate(Text, ZeroOrNull() + SelectedDay.ToString() + "." + day.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString());
        }

        public string Condition { get; set; }

        public ObservableCollection<int> Days { get; set; }

        public List<string> Months { get; set; }

        public LinkedList<int> Years { get; set; }

        public int SelectedDay
        {
            get { return selectedDay; }
            set
            {
                selectedDay = value;
                NameOfThisDay = day.GetNameDayOfWeek(selectedDay, SelectedMonth, SelectedYear);
            }
        }

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                if (Days.Count != day.GetNumberOfDaysInThisMonth(selectedMonth, SelectedYear))
                {
                    UpdateDays();
                }
                else
                {
                    NameOfThisDay = day.GetNameDayOfWeek(SelectedDay, selectedMonth, SelectedYear);
                }
            }
        }

        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                if (SelectedMonth == "Февраля" && Days.Count != day.GetNumberOfDaysInThisMonth(SelectedMonth, selectedYear))
                {
                    UpdateDays();
                }
                else
                {
                    NameOfThisDay = day.GetNameDayOfWeek(SelectedDay, SelectedMonth, selectedYear);
                }
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (value.Length <= 41)
                {
                   
                    text = value;
                }
            }
        }

        public bool IsAnnually { get; set; }

        public string NameOfThisDay { get; private set; }

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

        public ICommand SaveImportantDate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (Text == null || Text.Length == 0 || Text.Length == CountGaps())
                    {
                        BackgroundErrorAsync();
                    }
                    else
                    {
                        int annually;
                        if (IsAnnually)
                        {
                            annually = 1;
                        }
                        else
                        {
                            annually = 0;
                        }
                        if (editId == -1)
                        {
                            Thread thread = new Thread(new ParameterizedThreadStart(AddNewDate));
                            importantDatesLogic.AddToList(Text, ZeroOrNull() + SelectedDay.ToString() + "." + day.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString(), annually);
                            Condition = "Collapsed";
                            thread.Start(annually);
                        }
                        else
                        {
                            Thread thread = new Thread(new ParameterizedThreadStart(UpdateDate));
                            importantDatesLogic.UpdateToList(editId, Text, ZeroOrNull() + SelectedDay.ToString() + "." + day.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString(), annually);
                            Condition = "Collapsed";
                            thread.Start(annually);
                        }
                    }
                });
            }
        }

        public Brush Color { get; set; }

        private void UpdateDays()
        {
            int interimDay = SelectedDay;
            Days = new ObservableCollection<int>();
            int count = day.GetNumberOfDaysInThisMonth(SelectedMonth, SelectedYear);
            for (int i = 1; i <= count; i++)
            {
                Days.Add(i);
            }
            if (interimDay > Days.Count)
            {
                SelectedDay = 1;
            }
            else
            {
                SelectedDay = 1 + interimDay;
                SelectedDay -= 1;
            }
        }

        private void AddNewDate(object annually)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                importantDatesLogic.AddNewEvent(Text, ZeroOrNull() + SelectedDay.ToString() + "." + day.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString(), Convert.ToInt32(annually));
            });
        }

        private void UpdateDate(object annually)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                importantDatesLogic.UpdateData(editId, Text, ZeroOrNull() + SelectedDay.ToString() + "." + day.GetNumberOfMonth(SelectedMonth) + "." + SelectedYear.ToString(), Convert.ToInt32(annually));
            });
        }

        async private void BackgroundErrorAsync()
        {
            Color = (Brush)new BrushConverter().ConvertFromString("#FF7F50");
            await Task.Delay(328);
            Color = (Brush)new BrushConverter().ConvertFromString("LightGoldenrodYellow");
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

        private void Overall()
        {
            Condition = "Visible";
            day = new Day();
            Months = new List<string>()
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
            Years = new LinkedList<int>();
            for (int i = 1900; i <= 2328; i++)
            {
                Years.AddLast(i);
            }
            Days = new ObservableCollection<int>();
            dispatcher = Dispatcher.CurrentDispatcher;
            Color = (Brush)new BrushConverter().ConvertFromString("LightGoldenrodYellow");
        }

        private int CountGaps ()
        {
            int countGaps = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] == ' ')
                {
                    countGaps++;
                }
            }
            return countGaps;
        }
    }
}