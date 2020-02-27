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
        private GoalsLogic goalsLogic;
        private AllTimetablesViewModel allTimetablesViewModel;
        private AllHabitsTrackerViewModel allHabitsTrackerViewModel;
        private AllGoalsViewModel allGoalsViewModel;
        private NewTimetableViewModel newTimetableViewModel;
        private NewHabitTrackerViewModel newHabitTrackerViewModel;
        private NewGoalViewModel newGoalViewModel;
        private bool isClose;

        public PlannerViewModel() { }

        public PlannerViewModel(TimetableForTheDaysLogic timetableForTheDaysLogic, HabitsTrackerLogic habitsTrackerLogic, GoalsLogic goalsLogic) 
        {
            this.timetableForTheDaysLogic = timetableForTheDaysLogic;
            TimetableForTheDaysLogic = this.timetableForTheDaysLogic;
            this.habitsTrackerLogic = habitsTrackerLogic;
            HabitsTrackerLogic = this.habitsTrackerLogic;
            this.goalsLogic = goalsLogic;
            GoalsLogic = this.goalsLogic;
        }

        public TimetableForTheDaysLogic TimetableForTheDaysLogic { get; set; }

        public HabitsTrackerLogic HabitsTrackerLogic { get; set; }

        public GoalsLogic GoalsLogic { get; set; }

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

        public ICommand ViewGoals
        {
            get
            {
                return new RelayCommand(() =>
                {
                    allGoalsViewModel = new AllGoalsViewModel(goalsLogic);
                    AddOrViewExisting = new AllGoalsView();
                    AddOrViewExisting.DataContext = allGoalsViewModel;
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
                    newGoalViewModel = new NewGoalViewModel(goalsLogic);
                    AddOrViewExisting = new NewGoalView();
                    AddOrViewExisting.DataContext = newGoalViewModel;
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
                else if (allGoalsViewModel != null)
                {
                    allGoalsViewModel.IsClose = isClose;
                }
            }
        }
    }
}