using Diary.Model;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModel
{
    class MyNotesViewModel : ViewModelBase
    {
        private NotesLogic notesLogic;
        private ImportantDatesLogic importantDatesLogic;
        private NewNoteViewModel newNoteViewModel;
        private AllNotesViewModel allNotesViewModel;
        private NewImportantDateViewModel newImportantDateViewModel;
        private bool isClose;

        public MyNotesViewModel() { }

        public MyNotesViewModel(NotesLogic notesLogic, ImportantDatesLogic importantDatesLogic)
        {
            this.notesLogic = notesLogic;
            this.importantDatesLogic = importantDatesLogic;
            NotesLogic = this.notesLogic;
            ImportantDatesLogic = this.importantDatesLogic;
        }

        public NotesLogic NotesLogic { get; set; }

        public ImportantDatesLogic ImportantDatesLogic { get; set; }

        public ICommand ViewExisting
        {
            get
            {
                return new RelayCommand(() =>
                {
                    allNotesViewModel = new AllNotesViewModel(notesLogic);
                    AddOrViewExisting = new AllNotesView();
                    IsClose = false;
                    AddOrViewExisting.DataContext = allNotesViewModel;
                });
            }
        }

        public ICommand AddNewNote
        {
            get
            {
                return new RelayCommand(() =>
                {
                    newNoteViewModel = new NewNoteViewModel(notesLogic);
                    IsClose = false;
                    AddOrViewExisting = new NewNoteView();
                    AddOrViewExisting.DataContext = newNoteViewModel;
                });
            }
        }

        public ICommand AddNewEvent
        {
            get
            {
                return new RelayCommand(() =>
                {
                    newImportantDateViewModel = new NewImportantDateViewModel(importantDatesLogic);
                    IsClose = false;
                    AddOrViewExisting = new NewImportantDateView();
                    AddOrViewExisting.DataContext = newImportantDateViewModel;
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
                if (newNoteViewModel != null)
                {
                    newNoteViewModel.IsClose = isClose;
                }
                else if (allNotesViewModel != null)
                {
                    allNotesViewModel.IsClose = isClose;
                }
            }
        }
    }
}