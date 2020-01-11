using Diary.Another;
using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Diary.ViewModel
{
    class NewImportantDateViewModel : ViewModelBase
    {
        private ImportantDatesLogic importantDatesLogic;
        private Day day;
        private int selectedDay;
        private string selectedMonth;
        private int selectedYear;

        public NewImportantDateViewModel() { }

        public NewImportantDateViewModel(ImportantDatesLogic importantDatesLogic)
        {
            this.importantDatesLogic = importantDatesLogic;
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
            for (int i = 1900; i <= 3128; i++)
            {
                Years.AddLast(i);
            }
            selectedMonth = day.GetMonthNow();
            selectedYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            Days = new ObservableCollection<int>();
            int count = day.GetNumberOfDaysInThisMonth(selectedMonth, selectedYear);
            for (int i = 1; i <= count; i++)
            {
                Days.Add(i);
            }
            SelectedDay = Convert.ToInt32(DateTime.Now.ToString("dd"));
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
    }
}