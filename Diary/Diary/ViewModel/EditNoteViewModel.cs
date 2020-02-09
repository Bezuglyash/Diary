using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Diary.ViewModel
{
    class EditNoteViewModel : ViewModelBase
    {
        private NotesLogic notesLogic;
        private int id;
        private int countClickOnButton;
        private string nameElementWhoClose;
        private string startText;
        private bool isClose;
        private string text;

        public EditNoteViewModel() { }

        public EditNoteViewModel(NotesLogic notesLogic, int id)
        {
            this.notesLogic = notesLogic;
            this.id = id;
            countClickOnButton = 0;
            nameElementWhoClose = "Сохранить";
            startText = this.notesLogic.GetNoteContent(this.id);
            Condition = "Visible";
            Date = this.notesLogic.GetDateInternal(this.id);
            Text = startText;
            ButtonEdit = "Редактировать";
            IsClose = false;
            Color = (Brush)new BrushConverter().ConvertFromString("#FAFAD2");
            CountBorder = 0;
            Fill = (Brush)new BrushConverter().ConvertFromString("Gray");
        }

        public string Condition { get; set; }

        public string Date { get; set; }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                if (countClickOnButton == 0)
                {
                    text = startText;
                }
                if ((text.Length == 1 && text[0] == ' ') || (text.Length == 2 && text == "\r\n") || text == "")
                {
                    nameElementWhoClose = "Отменить";
                }
                else
                {
                    int countSpacing = 0;
                    int countEnterR = 0;
                    int countEnterN = 0;
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i] == ' ')
                        {
                            countSpacing++;
                        }
                        else if (text[i] == '\r')
                        {
                            countEnterR++;
                        }
                        else if (text[i] == '\n')
                        {
                            countEnterN++;
                        }
                    }
                    if (countSpacing == text.Length || countSpacing + countEnterR + countEnterN == text.Length || startText == text)
                    {
                        nameElementWhoClose = "Отменить";
                    }
                    else
                    {
                        nameElementWhoClose = "Сохранить";
                    }
                }
            }
        }

        public ICommand Edit
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (countClickOnButton == 0)
                    {
                        countClickOnButton++;
                        Color = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                        Fill = (Brush)new BrushConverter().ConvertFromString("LightCyan");
                        ButtonEdit = "Сохранить";
                        CountBorder = 1;
                    }
                    else 
                    {
                        IsClose = true;
                    }
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
        }

        public string ButtonEdit { get; set; }

        public bool IsClose
        {
            get { return isClose; }
            set
            {
                isClose = value;
                if (isClose == true && Text != null && Text != startText && nameElementWhoClose == "Сохранить")
                {
                    notesLogic.UpdateData(Convert.ToInt32(id), Text, DateTime.Now.ToString("dd.MM.yyyy" + " H:mm"));
                }
                Condition = "Collapsed";
            }
        }

        public Brush Color { get; set; }

        public int CountBorder { get; set; }

        public Brush Fill { get; set; }
    }
}