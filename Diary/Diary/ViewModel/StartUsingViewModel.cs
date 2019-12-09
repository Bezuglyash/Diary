using Diary.Model;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModel
{
    class StartUsingViewModel : ViewModelBase
    {
        private string textHello;
        private DiaryLogic diaryLogic;
        private FrameworkElement passwordControl;
        private string userName;
        private string helloUser;
        private string color;
        private string checkString;
        private string content;
        private bool isEnabled;
        private string notClosePassword;

        public StartUsingViewModel() { }
        public StartUsingViewModel(DiaryLogic diaryLogic)
        {
            this.diaryLogic = diaryLogic;
            TextHello = "Приветствуем Вас в приложении \"Мой ежедневник\"!\nПредставьтесь, пожалуйста!";
            Color = "FloralWhite";
            diaryLogic.NameUser = "";
            Content = "...";
            IsEnabled = false;
            NotClosePassword = "Visible";
        }

        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                RaisePropertyChanged();
            }
        }

        public string TextHello
        {
            get { return textHello; }
            set
            {
                textHello = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                checkString = value;
                if (checkString.Contains(" ") == true)
                {
                    bool isTwoSpace = false;
                    for (int i = 0; i < checkString.Length; i++)
                    {
                        if (i + 1 < checkString.Length)
                        {
                            if (checkString[i] == ' ' && checkString[i + 1] == ' ')
                            {
                                isTwoSpace = true;
                            }
                        }
                    }
                    if (isTwoSpace == false)
                    {
                        if (checkString[0] == ' ')
                        {
                            userName = checkString.Remove(0, 1);
                        }
                        else
                        {
                            userName = value;
                        }
                        RaisePropertyChanged();
                    }
                }
                else
                {
                    userName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string HelloUser
        {
            get { return helloUser; }
            set
            {
                helloUser = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SendingUserName
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (UserName.Length != 0)
                    {
                        if (UserName[UserName.Length - 1] == ' ')
                        {
                            UserName = UserName.Remove(UserName.Length - 1, 1);
                        }
                        TextHello = "Привет, " + UserName + "!\nВы можете установить пароль (рекомендуется) или продолжить без пароля!";
                        Content = "Продолжить без пароля";
                        diaryLogic.NameUser = UserName;
                        IsEnabled = true;
                        PasswordControl = new PasswordView();
                        PasswordControl.DataContext = new PasswordViewModel(diaryLogic);
                        CheckAsync();
                    }
                    else
                    {
                        Color = "#FF7F50";
                    }
                });
            }
        }


        async public void CheckAsync()
        {
            await Task.Run(() => 
            {
                while (diaryLogic.UserPassword == null) { }
                NotClosePassword = "Collapsed";
            });
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                RaisePropertyChanged();
            }
        }

        public FrameworkElement PasswordControl
        {
            get { return passwordControl; }
            set
            {
                passwordControl = value;
                RaisePropertyChanged();
            }
        }

        public ICommand EndWithoutPassword
        {
            get
            {
                return new RelayCommand(() =>
                {
                    diaryLogic.SaveUser();
                    NotClosePassword = "Collapsed";
                });
            }
        }

        public string NotClosePassword
        {
            get { return notClosePassword; }
            set
            {
                notClosePassword = value;
                RaisePropertyChanged();
            }
        }
    }
}
