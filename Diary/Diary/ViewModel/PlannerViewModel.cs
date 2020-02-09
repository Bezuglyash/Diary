using Diary.Model;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModel
{
    class PlannerViewModel : ViewModelBase
    {
        private TimetableForTheDaysLogic timetableForTheDaysLogic;
        private AllTimetablesViewModel allTimetablesViewModel;
        private NewTimetableViewModel newTimetableViewModel;
        private bool isClose;

        public PlannerViewModel() { }

        public PlannerViewModel(TimetableForTheDaysLogic timetableForTheDaysLogic) 
        {
            this.timetableForTheDaysLogic = timetableForTheDaysLogic;
            TimetableForTheDaysLogic = this.timetableForTheDaysLogic;
        }

        public TimetableForTheDaysLogic TimetableForTheDaysLogic { get; set; }

        public ICommand ViewTimetables
        {
            get
            {
                return new RelayCommand(() =>
                {
                    allTimetablesViewModel = new AllTimetablesViewModel(timetableForTheDaysLogic);
                    AddOrViewExisting = new AllTimetablesView();
                    AddOrViewExisting.DataContext = allTimetablesViewModel;
                });
            }
        }

        public ICommand AddNewTimetable
        {
            get
            {
                return new RelayCommand(() =>
                {
                    newTimetableViewModel = new NewTimetableViewModel(timetableForTheDaysLogic);
                    AddOrViewExisting = new NewTimetableView();
                    AddOrViewExisting.DataContext = newTimetableViewModel;
                });
            }
        }

        public FrameworkElement AddOrViewExisting { get; set; }

        public bool IsClose
        {
            get { return isClose; }
            set
            {
                isClose = value;
                if (allTimetablesViewModel != null)
                {
                    allTimetablesViewModel.IsClose = isClose;
                }
            }
        }
    }
}