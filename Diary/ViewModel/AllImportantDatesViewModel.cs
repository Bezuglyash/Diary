using Diary.Another;
using Diary.Model.ImportantDatesLogic;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Diary.ViewModel
{
    class AllImportantDatesViewModel : ViewModelBase
    {
        private ImportantDatesLogic importantDatesLogic;
        private Day day;
        private string selectedMonth;
        private int selectedYear;

        public AllImportantDatesViewModel() { }

        public AllImportantDatesViewModel(ImportantDatesLogic importantDatesLogic)
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
            SelectedMonth = day.GetMonthNow();
            SelectedYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            DaysOfMonth = new ObservableCollection<DayLogic>();
            for (int i = 1; i <= 31; i++)
            {
                DaysOfMonth.Add(new DayLogic(i));
            }
        }

        public List<string> Months { get; set; }

        public LinkedList<int> Years { get; set; }

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
            }
        }

        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
            }
        }

        public ObservableCollection<DayLogic> DaysOfMonth { get; set; } 
    }
}