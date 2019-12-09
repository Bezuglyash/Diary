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
        private bool isClose;

        public MyNotesViewModel() { }

        public MyNotesViewModel(NotesLogic notesLogic, ImportantDatesLogic importantDatesLogic)
        {
            this.notesLogic = notesLogic;
            this.importantDatesLogic = importantDatesLogic;
            NotesLogic = notesLogic;
            ImportantDatesLogic = importantDatesLogic;
        }

        public NotesLogic NotesLogic
        {
            get { return notesLogic; }
            set
            {
                notesLogic = value;
                RaisePropertyChanged("NumberOfNotes");
            }
        }

        public ImportantDatesLogic ImportantDatesLogic
        {
            get { return importantDatesLogic; }
            set
            {
                importantDatesLogic = value;
                RaisePropertyChanged("NumberOfDates");
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
                    Add = new NewNoteView();
                    Add.DataContext = newNoteViewModel;
                    //notesLogic.AddNewNote("1", "2", "3");
                });
            }
        }

        public ICommand AddNewEvent
        {
            get
            {
                return new RelayCommand(() =>
                {
                    importantDatesLogic.AddNewEvent("1", "2", 1);
                });
            }
        }

        public FrameworkElement Add { get; set; }

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
            }
        }
    }
}