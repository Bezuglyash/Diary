using System.Threading;
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
        private BasketLogic basketLogic;
        private StartUsingViewModel startUsingViewModel;
        private PasswordInputViewModel passwordInputViewModel;
        private OrganizerViewModel organizerViewModel;
        private SettingsViewModel settingsViewModel;
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
            else if (diaryLogic.IsHavePassword())
            {
                SwitchView("Пароль");
            }
            else
            {
                CreateLogicAsync();
            }
            Greeting = "Привет, " + diaryLogic.GetName() + "!";
            NowValue = SMALL_VALUE;
            UpdateValue = BIG_VALUE;
            WidthMenu = SMALL_VALUE;
        }

        public string Greeting { get; set; }

        public FrameworkElement DiaryStartUsing { get; set; }

        public FrameworkElement DiaryOperation { get; set; }

        public string WidthMenu { get; set; }

        public ICommand OpenMainPage
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
                    if (lastClick != "Главная")
                    {
                        SwitchView("Главная");
                        lastClick = "Главная";
                    }
                });
            }
        }

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

        public ICommand OpenSettings
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
                    if (lastClick != "Настройки")
                    {
                        SwitchView("Настройки");
                        lastClick = "Настройки";
                    }
                });
            }
        }

        public ICommand OpenBasket
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
                    if (lastClick != "Корзина")
                    {
                        SwitchView("Корзина");
                        lastClick = "Корзина";
                    }
                });
            }
        }

        public string NowValue { get; set; }

        public string UpdateValue { get; set; }

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
                    startUsingViewModel = new StartUsingViewModel(diaryLogic);
                    DiaryStartUsing = new StartUsingThisDiaryView();
                    DiaryStartUsing.DataContext = startUsingViewModel;
                    WaitingAsync();
                    break;
                case "Пароль":
                    passwordInputViewModel = new PasswordInputViewModel(diaryLogic);
                    DiaryStartUsing = new PasswordInputView();
                    DiaryStartUsing.DataContext = passwordInputViewModel;
                    WaitingDonePassword();
                    break;
                case "Главная":
                    StopProcess();
                    DiaryOperation = new MainPageView();
                    DiaryOperation.DataContext = new MainPageViewModel((importantDatesLogic, timetableForTheDaysLogic, habitsTrackerLogic, goalsLogic));
                    break;
                case "Органайзер":
                    StopProcess();
                    organizerViewModel = new OrganizerViewModel((notesLogic, importantDatesLogic, timetableForTheDaysLogic, habitsTrackerLogic, goalsLogic, basketLogic));
                    DiaryOperation = new OrganizerView();
                    DiaryOperation.DataContext = organizerViewModel;
                    break;
                case "Настройки":
                    StopProcess();
                    settingsViewModel = new SettingsViewModel(diaryLogic);
                    DiaryOperation = new SettingsView();
                    DiaryOperation.DataContext = settingsViewModel;
                    break;
                case "Корзина":
                    StopProcess();
                    DiaryOperation = new BasketView();
                    DiaryOperation.DataContext = new BasketViewModel(notesLogic, basketLogic);
                    break;
            }
        }

        private async void WaitingAsync()
        {
            await Task.Run(() =>
            {
                while (startUsingViewModel.Status == "") { }
                Greeting = "Привет, " + diaryLogic.GetName() + "!";
                CreateLogicAsync();
            });
        }

        private async void WaitingDonePassword()
        {
            await Task.Run(() =>
            {
                while (passwordInputViewModel.Condition == "Visible") { }
                CreateLogicAsync();
            });
        }

        private async void CreateLogicAsync()
        {
            await Task.Run(() =>
            {
                notesLogic = new NotesLogic(diaryLogic.GetDataBase());
                importantDatesLogic = new ImportantDatesLogic(diaryLogic.GetDataBase());
                timetableForTheDaysLogic = new TimetableForTheDaysLogic(diaryLogic.GetDataBase());
                habitsTrackerLogic = new HabitsTrackerLogic();
                goalsLogic = new GoalsLogic(diaryLogic.GetDataBase());
                basketLogic = new BasketLogic(diaryLogic.GetDataBase());
            });
        }

        private void StopProcess()
        {
            if (organizerViewModel != null)
            {
                organizerViewModel.IsClose = true;
            }
            if (settingsViewModel != null)
            {
                settingsViewModel.IsClose = true;
            }
        }
    }
}