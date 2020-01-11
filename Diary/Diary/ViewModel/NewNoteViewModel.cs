using Diary.Model;using GalaSoft.MvvmLight;using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;using System.Windows.Input;

namespace Diary.ViewModel{    class NewNoteViewModel : ViewModelBase    {        private NotesLogic notesLogic;        private bool isClose;        private string newText;        private string nameElementWhoClose;        public NewNoteViewModel() { }        public NewNoteViewModel(NotesLogic notesLogic)        {            this.notesLogic = notesLogic;            Condition = "Visible";            Date = "Сегодня " + DateTime.Now.ToString("H:mm");            IsClose = false;            nameElementWhoClose = "Сохранить";            ItCanSave = false;            UpdateDateAsync();        }
        public string Condition { get; set; }        public string Date { get; set; }        public string NewText        {
            get { return newText; }
            set
            {                newText = value;                if ((newText.Length == 1 && newText[0] == ' ') || (newText.Length == 2 && newText == "\r\n") || newText == "")
                {
                    nameElementWhoClose = "Отменить";
                    ItCanSave = false;
                }                else
                {
                    int countSpacing = 0;
                    int countEnterR = 0;
                    int countEnterN = 0;
                    for (int i = 0; i < newText.Length; i++)
                    {
                        if (newText[i] == ' ')
                        {
                            countSpacing++;
                        }
                        else if (newText[i] == '\r')
                        {
                            countEnterR++;
                        }
                        else if (newText[i] == '\n')
                        {
                            countEnterN++;
                        }
                    }
                    if (countSpacing == newText.Length || countSpacing + countEnterR + countEnterN == newText.Length)
                    {
                        nameElementWhoClose = "Отменить";
                        ItCanSave = false;
                    }
                    else
                    {
                        nameElementWhoClose = "Сохранить";
                        ItCanSave = true;
                    }
                }            }        }        public ICommand SaveNote
        {
            get
            {
                return new RelayCommand(() =>
                {
                    nameElementWhoClose = "Сохранить";
                    Condition = "Collapsed";
                    IsClose = true;
                });
            }
        }

        public ICommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    nameElementWhoClose = "Отменить";
                    Condition = "Collapsed";
                    IsClose = true;
                });
            }
        }        public bool ItCanSave { get; set; }        public bool IsClose         {             get { return isClose; }            set            {                isClose = value;                if (isClose == true && NewText != null && nameElementWhoClose == "Сохранить")                {                    notesLogic.AddNewNote(NewText, DateTime.Now.ToString("dd.MM.yyyy" + " H:mm"));                }            }        }        async public void UpdateDateAsync()        {            await Task.Run(() =>            {                while (IsClose != true)                {                    if (Date != "Сегодня " + DateTime.Now.ToString("H:mm"))                    {                        Date = "Сегодня " + DateTime.Now.ToString("H:mm");                    }                }            });        }    }}