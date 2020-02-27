using Diary.Model;
using Diary.Model.ImportantDatesLogic;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModel
{
    class OrganizerViewModel : ViewModelBase
    {
        private readonly NotesLogic notesLogic;
        private readonly ImportantDatesLogic importantDatesLogic;
        private readonly TimetableForTheDaysLogic timetableForTheDaysLogic;
        private readonly HabitsTrackerLogic habitsTrackerLogic;
        private readonly GoalsLogic goalsLogic;
        private readonly BasketLogic basketLogic;
        private MyNotesViewModel myNotesViewModel;
        private PlannerViewModel plannerViewModel;

        public OrganizerViewModel() { }

        public OrganizerViewModel((NotesLogic, ImportantDatesLogic, TimetableForTheDaysLogic, HabitsTrackerLogic, GoalsLogic, BasketLogic) logics)
        {
            notesLogic = logics.Item1;
            importantDatesLogic = logics.Item2;
            timetableForTheDaysLogic = logics.Item3;
            habitsTrackerLogic = logics.Item4;
            goalsLogic = logics.Item5;
            basketLogic = logics.Item6;
            IsEnabledNotes = false;
            IsEnabledPlanner = true;
            myNotesViewModel = new MyNotesViewModel(notesLogic, importantDatesLogic, basketLogic);
            OrganizerType = new MyNotesView();
            OrganizerType.DataContext = myNotesViewModel;
            IsClose = false;
        }

        public bool IsEnabledNotes { get; set; }

        public bool IsEnabledPlanner { get; set; }

        public ICommand Notes
        {
            get
            {
                return new RelayCommand(() =>
                {
                    plannerViewModel.IsClose = true;
                    myNotesViewModel = new MyNotesViewModel(notesLogic, importantDatesLogic, basketLogic);
                    OrganizerType = new MyNotesView();
                    OrganizerType.DataContext = myNotesViewModel;
                    IsEnabledNotes = false;
                    IsEnabledPlanner = true;
                });
            }
        }

        public ICommand Planner
        {
            get
            {
                return new RelayCommand(() =>
                {
                    myNotesViewModel.IsClose = true;
                    plannerViewModel = new PlannerViewModel(timetableForTheDaysLogic, habitsTrackerLogic, goalsLogic);
                    OrganizerType = new PlannerView();
                    OrganizerType.DataContext = plannerViewModel;
                    IsEnabledNotes = true;
                    IsEnabledPlanner = false;
                });
            }
        }

        public FrameworkElement OrganizerType { get; set; }

        public bool IsClose
        {
            set
            {
                if (myNotesViewModel != null)
                {
                    myNotesViewModel.IsClose = value;
                }
                if (plannerViewModel != null)
                {
                    plannerViewModel.IsClose = value;
                }
            }
        }
    }
}