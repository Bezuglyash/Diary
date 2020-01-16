using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diary.Model;
using Diary.Model.ImportantDatesLogic;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Diary.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DiaryLogic diaryLogic;
        private NotesLogic notesLogic;
        private ImportantDatesLogic importantDatesLogic;
        private string userName;
        private string valueNow;
        private string valueUpdate;
        private const string SMALL_VALUE = "43";
        private const string BIG_VALUE = "228";
        private string lastClick;

        public MainViewModel()
        {
            diaryLogic = new DiaryLogic();
            if (diaryLogic.ItWasOpen == false)
            {
                SwitchView();
            }
            else
            {
                CreateLogics();
            }
            Greeting = diaryLogic.NameUser;
            NowValue = SMALL_VALUE;
            UpdateValue = BIG_VALUE;
            WidthMenu = SMALL_VALUE;
        }

        public string Greeting
        {
            get { return "Привет, " + userName + "!"; }
            set
            {
                userName = value;
                RaisePropertyChanged();
            }
        }

        public FrameworkElement DiaryStartUsing { get; set; }

        public FrameworkElement DiaryOperation { get; set; }

        public string WidthMenu { get; set; }

        public ICommand OpenMyNotesControl
        {
            get
            {
                return new RelayCommand(() =>
                {
                    WidthMenu = SMALL_VALUE;
                    if (NowValue != SMALL_VALUE)
                    {
                        NowValue = SMALL_VALUE;
                        UpdateValue = BIG_VALUE;
                    }
                    if (lastClick != "Органайзер")
                    {
                        SwitchView("MyNotes");
                        lastClick = "Органайзер";
                    }
                });
            }
        }

        public string NowValue
        {
            get { return valueNow; }
            set
            {
                valueNow = value;
                RaisePropertyChanged();
            }
        }

        public string UpdateValue
        {
            get { return valueUpdate; }
            set
            {
                valueUpdate = value;
                RaisePropertyChanged();
            }
        }

        public ICommand UpdateMenu
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string update = NowValue;
                    NowValue = UpdateValue;
                    UpdateValue = update;
                });
            }
        }

        public void SwitchView(string viewName = "")
        {
            switch (viewName)
            {
                case "":
                    DiaryStartUsing = new StartUsingThisDiaryView();
                    DiaryStartUsing.DataContext = new StartUsingViewModel(diaryLogic);
                    diaryLogic.CreateDataBaseAndTables();
                    CreateLogics();
                    WaitingAsync();
                    break;
                case "MyNotes":
                    DiaryOperation = new OrganizerView();
                    DiaryOperation.DataContext = new OrganizerViewModel((notesLogic, importantDatesLogic));
                    break;
                default:
                    break;
            }
        }

        async void WaitingAsync()
        {
            await Task.Run(() =>
            {
                while (diaryLogic.NameUser == "") { }
                Greeting = diaryLogic.NameUser;
            });
        }

        public void CreateLogics()
        {
            notesLogic = new NotesLogic(diaryLogic.GetDataBase());
            importantDatesLogic = new ImportantDatesLogic(diaryLogic.GetDataBase());
        }
    }
}