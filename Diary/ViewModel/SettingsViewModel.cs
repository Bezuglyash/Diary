using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Diary.Model;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Diary.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        private DiaryLogic diaryLogic;
        private string userName;
        private PasswordInputViewModel passwordInputViewModel;
        private PasswordViewModel passwordViewModel;

        public SettingsViewModel() { }

        public SettingsViewModel(DiaryLogic diaryLogic)
        {
            this.diaryLogic = diaryLogic;
            UserName = this.diaryLogic.GetName();
            Color = (Brush)new BrushConverter().ConvertFromString("LightGoldenrodYellow");
            Text = "Изменить";
            IsReadOnly = true;
            Password = this.diaryLogic.IsHavePassword() ? "****" : "-";
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                string checkString = value;
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

        public string Text { get; set; }

        public Brush Color { get; set; }

        public ICommand SendingUserName
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (IsReadOnly)
                    {
                        IsReadOnly = false;
                        Text = "Сохранить";
                        Color = (Brush)new BrushConverter().ConvertFromString("FloralWhite");
                    }
                    else if (UserName != null)
                    {
                        if (UserName.Length != 0)
                        {
                            if (UserName[UserName.Length - 1] == ' ')
                            {
                                UserName = UserName.Remove(UserName.Length - 1, 1);
                            }
                            diaryLogic.RewriteName(UserName);
                            Text = "Изменить";
                            IsReadOnly = true;
                            Color = (Brush)new BrushConverter().ConvertFromString("LightGoldenrodYellow");
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

        public bool IsReadOnly { get; set; }

        public string Password { get; set; }

        public ICommand AddOrDeletePassword
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (Password == "****")
                    {
                        passwordInputViewModel = new PasswordInputViewModel(diaryLogic);
                        PasswordViews = new PasswordInputView();
                        PasswordViews.DataContext = passwordInputViewModel;
                        WaitingUserPasswordAsync();
                    }
                    else
                    {
                        passwordViewModel = new PasswordViewModel(diaryLogic);
                        PasswordViews = new PasswordView();
                        PasswordViews.DataContext = passwordViewModel;
                        WaitingAddPasswordAsync();
                    }
                });
            }
        }

        public ICommand RewritePassword
        {
            get
            {
                return new RelayCommand(() =>
                {
                    passwordInputViewModel = new PasswordInputViewModel(diaryLogic);
                    PasswordViews = new PasswordInputView();
                    PasswordViews.DataContext = passwordInputViewModel;
                    WaitingAsync();
                });
            }
        }

        public FrameworkElement PasswordViews { get; set; }

        public bool IsClose { get; set; }

        private async void WaitingUserPasswordAsync()
        {
            await Task.Run(() =>
            {
                while (IsClose != true && passwordInputViewModel.Condition == "Visible") ;
                if (passwordInputViewModel.Information == "Введён пароль")
                {
                    diaryLogic.UpdatePassword(null, 0);
                    Password = "-";
                }
            });
        }

        private async void WaitingAddPasswordAsync()
        {
            await Task.Run(() =>
            {
                while (IsClose != true && passwordViewModel.Information == null) ;
                if (passwordViewModel.Information == "Введён пароль")
                {
                    Password = "****";
                }
                passwordViewModel.Condition = "Collapsed";
            });
        }

        private async void WaitingAsync()
        {
            await Task.Run(() =>
            {
                while (IsClose != true && passwordInputViewModel.Condition == "Visible") ;
            });
            if (passwordInputViewModel.Information == "Введён пароль")
            {
                passwordViewModel = new PasswordViewModel(diaryLogic);
                PasswordViews = new PasswordView();
                PasswordViews.DataContext = passwordViewModel;
                WaitingAddPasswordAsync();
            }
        }
    }
}