using Diary.DataBase;
using Diary.Model;
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
        private MyNotesViewModel myNotesViewModel;

        public OrganizerViewModel() { }

        public OrganizerViewModel((NotesLogic, ImportantDatesLogic) logics)
        {
            notesLogic = logics.Item1;
            importantDatesLogic = logics.Item2;
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
                    OrganizerType = new PlannerView();
                    OrganizerType.DataContext = new PlannerViewModel();
                    IsEnabledNotes = true;
                    IsEnabledPlanner = false;
                });
            }
        }

        public FrameworkElement OrganizerType { get; set; }
    }
}