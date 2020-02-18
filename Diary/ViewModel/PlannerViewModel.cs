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
        private HabitsTrackerLogic habitsTrackerLogic;
        private AllTimetablesViewModel allTimetablesViewModel;
        private AllHabitsTrackerViewModel allHabitsTrackerViewModel;
        private NewTimetableViewModel newTimetableViewModel;
        private NewHabitTrackerViewModel newHabitTrackerViewModel;
        private bool isClose;

        public PlannerViewModel() { }

        public PlannerViewModel(TimetableForTheDaysLogic timetableForTheDaysLogic, HabitsTrackerLogic habitsTrackerLogic) 
        {
            this.timetableForTheDaysLogic = timetableForTheDaysLogic;
            TimetableForTheDaysLogic = this.timetableForTheDaysLogic;
            this.habitsTrackerLogic = habitsTrackerLogic;
            HabitsTrackerLogic = this.habitsTrackerLogic;
        }

        public TimetableForTheDaysLogic TimetableForTheDaysLogic { get; set; }

        public HabitsTrackerLogic HabitsTrackerLogic { get; set; }

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

        public ICommand ViewHabits
        {
            get
            {
                return new RelayCommand(() =>
                {
                    allHabitsTrackerViewModel = new AllHabitsTrackerViewModel(habitsTrackerLogic);
                    AddOrViewExisting = new AllHabitsTrackerView();
                    AddOrViewExisting.DataContext = allHabitsTrackerViewModel;
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

        public ICommand AddNewHabit
        {
            get
            {
                return new RelayCommand(() =>
                {
                    newHabitTrackerViewModel = new NewHabitTrackerViewModel(habitsTrackerLogic);
                    AddOrViewExisting = new NewHabitTrackerView();
                    AddOrViewExisting.DataContext = newHabitTrackerViewModel;
                });
            }
        }

        public ICommand AddNewGoal
        {
            get
            {
                return new RelayCommand(() =>
                {
                    
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