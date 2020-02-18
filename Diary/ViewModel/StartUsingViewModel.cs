using Diary.DataBase;
using Diary.Model;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Diary.ViewModel
{
    class StartUsingViewModel : ViewModelBase
    {
        private string textHello;
        private DiaryLogic diaryLogic;
        private string userName;
        private string checkString;

        public StartUsingViewModel() { }
        public StartUsingViewModel(DiaryLogic diaryLogic)
        {
            this.diaryLogic = diaryLogic;
            TextHello = "Приветствуем Вас в приложении \"Мой ежедневник\"!\nПредставьтесь, пожалуйста!";
            Color = (Brush)new BrushConverter().ConvertFromString("FloralWhite");
            diaryLogic.NameUser = "";
            Content = "...";
            IsEnabled = false;
            NotClosePassword = "Visible";
        }

        public string Content { get; set; }

        public string TextHello { get; set; }

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
                    }
                }
                else
                {
                    userName = value;
                }
            }
        }

        public string HelloUser { get; set; }

        public ICommand SendingUserName
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (UserName != null)
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
                            Color = (Brush)new BrushConverter().ConvertFromString("#FF7F50");
                        }
                    }
                    else
                    {
                        Color = (Brush)new BrushConverter().ConvertFromString("#FF7F50");
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

        public bool IsEnabled { get; set; }

        public Brush Color { get; set; }

        public FrameworkElement PasswordControl { get; set; }

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

        public string NotClosePassword { get; set; }
    }
}