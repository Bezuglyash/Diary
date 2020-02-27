using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Diary.DataBase;
using Diary.Model;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Diary.ViewModel
{
    class BasketViewModel : ViewModelBase
    {
        private NotesLogic notesLogic;
        private BasketLogic basketLogic;
        private SeeBasketViewModel seeBasketViewModel;
        private Dispatcher dispatcher;
        private string textSearch;

        public BasketViewModel() { }

        public BasketViewModel(NotesLogic notesLogic,BasketLogic basketLogic)
        {
            this.notesLogic = notesLogic;
            this.basketLogic = basketLogic;
            DeleteNote = new GalaSoft.MvvmLight.Command.RelayCommand<int>(Delete);
            RecoverNote = new GalaSoft.MvvmLight.Command.RelayCommand<int>(Recover);
            EditAndViewNote = new GalaSoft.MvvmLight.Command.RelayCommand<int>(EditAndView);
            dispatcher = Dispatcher.CurrentDispatcher;
            TextSearch = "";
            Notes = new ObservableCollection<Basket>(this.basketLogic.Notes);
            Count = Notes.Count;
        }

        public ObservableCollection<Basket> Notes { get; set; }

        public int Count { get; set; }

        public GalaSoft.MvvmLight.Command.RelayCommand<int> DeleteNote { get; }

        public GalaSoft.MvvmLight.Command.RelayCommand<int> RecoverNote { get; }

        public GalaSoft.MvvmLight.Command.RelayCommand<int> EditAndViewNote { get; }

        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                if (value != null)
                {
                    textSearch = value;
                    basketLogic.Searching(textSearch);
                    Notes = new ObservableCollection<Basket>(basketLogic.Notes);
                    Count = Notes.Count;
                }
            }
        }

        public FrameworkElement GoToViewNote { get; set; }

        public ICommand AllDelete
        {
            get
            {
                return  new RelayCommand(() =>
                {
                    if (new MessageBoxViewModel("Вы уверены, что хотите удалить все записи безвозвратно?", "Удаление").ShowMessage() == true)
                    {
                        Notes.Clear();
                        Count = Notes.Count;
                        basketLogic.AllDeleteAsync();
                    }
                });
            }
        }

        private void Recover(int idNoteWhichMustDelete)
        {
            Basket basket = basketLogic.GetBasket(idNoteWhichMustDelete);
            Notes.RemoveAt(GetIndexByTitle(basket.NoteTitle));
            Count = Notes.Count;
            RecoverUpdateAsync(basket.NoteContent, basketLogic.GetStandardDate(idNoteWhichMustDelete));
            basketLogic.Recover(idNoteWhichMustDelete);
        }

        private void EditAndView(int idNoteWhichMustDelete)
        {
            Basket basket = basketLogic.GetBasket(idNoteWhichMustDelete);
            seeBasketViewModel = new SeeBasketViewModel(basket.NoteContent, basket.CreationOrEditingDate);
            GoToViewNote = new SeeBasketView();
            TextSearch = "";
            GoToViewNote.DataContext = seeBasketViewModel;
        }

        private void Delete(int idNoteWhichMustDelete)
        {
            if (new MessageBoxViewModel("Вы уверены, что хотите удалить запись безвозвратно?", "Удаление").ShowMessage() == true)
            {
                int index = basketLogic.GetIndexOfList(idNoteWhichMustDelete);
                Notes.RemoveAt(index);
                Count = Notes.Count;
                Thread thread = new Thread(new ParameterizedThreadStart(UpdateNote));
                thread.Start(idNoteWhichMustDelete);
            }
        }

        private void UpdateNote(object id)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                basketLogic.DeleteNote(Convert.ToInt32(id));
            });
        }

        private async void RecoverUpdateAsync(string text, string date)
        {
            await Task.Run(() => { notesLogic.AddNewNote(text, date); });
        }

        private int GetIndexByTitle(string text)
        {
            IEnumerator<Basket> iterator = Notes.GetEnumerator();
            int index = 0;
            while (iterator.MoveNext())
            {
                if (iterator.Current.NoteTitle == text)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}