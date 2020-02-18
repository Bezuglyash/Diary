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
        private TimetableForTheDaysLogic timetableForTheDaysLogic;
        private HabitsTrackerLogic habitsTrackerLogic;
        private GoalsLogic goalsLogic;
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
                CreateLogic();
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
                        SwitchView("Органайзер");
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
                    WaitingAsync();
                    break;
                case "Органайзер":
                    DiaryOperation = new OrganizerView();
                    DiaryOperation.DataContext = new OrganizerViewModel((notesLogic, importantDatesLogic, timetableForTheDaysLogic, habitsTrackerLogic));
                    break;
            }
        }

        private async void WaitingAsync()
        {
            await Task.Run(() =>
            {
                while (diaryLogic.NameUser == "") { }
                Greeting = diaryLogic.NameUser;
                diaryLogic.CreateDataBaseAndTables();
                CreateLogic();
            });
        }

        private void CreateLogic()
        {
            notesLogic = new NotesLogic(diaryLogic.GetDataBase());
            importantDatesLogic = new ImportantDatesLogic(diaryLogic.GetDataBase());
            timetableForTheDaysLogic = new TimetableForTheDaysLogic(diaryLogic.GetDataBase());
            habitsTrackerLogic = new HabitsTrackerLogic();
            goalsLogic = new GoalsLogic(diaryLogic.GetDataBase());
        }
    }
}