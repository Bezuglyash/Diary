using Diary.DataBase;
using Diary.Model;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Diary.ViewModel
{
    class AllNotesViewModel : ViewModelBase
    {
        private NotesLogic notesLogic;
        private BasketLogic basketLogic;
        private EditNoteViewModel editNoteViewModel;
        private Dispatcher dispatcher;
        private bool isClose;
        private string textSearch;

        public AllNotesViewModel() { }

        public AllNotesViewModel(NotesLogic notesLogic, BasketLogic basketLogic)
        {
            this.notesLogic = notesLogic;
            this.basketLogic = basketLogic;
            DeleteNoteCommand = new RelayCommand<int>(DeleteNote);
            EditAndViewNoteCommand = new RelayCommand<int>(EditAndViewNote);
            Condition = "Visible";
            isClose = false;
            dispatcher = Dispatcher.CurrentDispatcher;
            TextSearch = "";
            Notes = new ObservableCollection<Note>(this.notesLogic.Notes);
            Count = Notes.Count;
        }

        public string Condition { get; set; }

        public ObservableCollection<Note> Notes { get; set; }

        public int Count { get; set; }

        public RelayCommand<int> DeleteNoteCommand { get; }

        public RelayCommand<int> EditAndViewNoteCommand { get; }

        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                if (value != null)
                {
                    textSearch = value;
                    notesLogic.Searching(textSearch);
                    Notes = new ObservableCollection<Note>(notesLogic.Notes);
                    Count = Notes.Count;
                }
            }
        }

        public ICommand BackSpace
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Condition = "Collapsed";
                });
            }
        }

        public FrameworkElement GoToViewAndEditNote { get; set; }

        public bool IsClose
        {
            get { return isClose; }
            set
            {
                isClose = value;
                if (editNoteViewModel != null)
                {
                    editNoteViewModel.IsClose = isClose;
                }
            }
        }

        private void DeleteNote(int idNoteWhichMustDelete)
        {
            if (new MessageBoxViewModel("Вы действительно хотите удалить запись в корзину?", "Удаление").ShowMessage() == true)
            {
                Note note = Notes[notesLogic.GetIndexOfList(idNoteWhichMustDelete)];
                Notes.Remove(note);
                Count = Notes.Count;
                Thread thread = new Thread(new ParameterizedThreadStart(UpdateNote));
                thread.Start(idNoteWhichMustDelete);
                basketLogic.AddNewNoteAsync(note.NoteTitle, note.NoteContent, notesLogic.GetStandardDate(idNoteWhichMustDelete));
            }
        }

        private void EditAndViewNote(int idNoteWhichMustDelete)
        {
            editNoteViewModel = new EditNoteViewModel(notesLogic, idNoteWhichMustDelete);
            GoToViewAndEditNote = new EditNoteView();
            TextSearch = "";
            IsClose = false;
            GoToViewAndEditNote.DataContext = editNoteViewModel;
            WaitUpdateAsync(idNoteWhichMustDelete);
        }

        private void UpdateNote(object id)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                notesLogic.DeleteNote(Convert.ToInt32(id));
            });
        }

        private async void WaitUpdateAsync(int id)
        {
            await Task.Run(() =>
            {
                while (editNoteViewModel.Condition != "Collapsed") { }
                Notes = new ObservableCollection<Note>(notesLogic.Notes);
            });
        }
    }
}