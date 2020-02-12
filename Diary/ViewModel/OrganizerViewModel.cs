using Diary.Model;
using Diary.Model.ImportantDatesLogic;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModel
{
    class OrganizerViewModel : ViewModelBase
    {
        private NotesLogic notesLogic;
        private ImportantDatesLogic importantDatesLogic;
        private TimetableForTheDaysLogic timetableForTheDaysLogic;
        private HabitsTrackerLogic habitsTrackerLogic;
        private MyNotesViewModel myNotesViewModel;
        private PlannerViewModel plannerViewModel;

        public OrganizerViewModel() { }

        public OrganizerViewModel((NotesLogic, ImportantDatesLogic, TimetableForTheDaysLogic, HabitsTrackerLogic) logics)
        {
            notesLogic = logics.Item1;
            importantDatesLogic = logics.Item2;
            timetableForTheDaysLogic = logics.Item3;
            habitsTrackerLogic = logics.Item4;
            IsEnabledNotes = false;
            IsEnabledPlanner = true;
            myNotesViewModel = new MyNotesViewModel(notesLogic, importantDatesLogic);
            OrganizerType = new MyNotesView();
            OrganizerType.DataContext = myNotesViewModel;
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
                    myNotesViewModel = new MyNotesViewModel(notesLogic, importantDatesLogic);
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
                    plannerViewModel = new PlannerViewModel(timetableForTheDaysLogic, habitsTrackerLogic);
                    OrganizerType = new PlannerView();
                    OrganizerType.DataContext = plannerViewModel;
                    IsEnabledNotes = true;
                    IsEnabledPlanner = false;
                });
            }
        }

        public FrameworkElement OrganizerType { get; set; }
    }
}