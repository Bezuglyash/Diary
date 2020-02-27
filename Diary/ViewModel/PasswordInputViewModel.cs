using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Diary.ViewModel
{
    class PasswordInputViewModel : ViewModelBase
    {
        private DiaryLogic diaryLogic;
        private string userPassword;
        private string password;
        private int countInputPassword;
        private ICommand passwordInput;
        private int count;

        public PasswordInputViewModel() { }

        public PasswordInputViewModel(DiaryLogic diaryLogic)
        {
            this.diaryLogic = diaryLogic;
            Condition = "Visible";
            IsCanDelete = false;
            userPassword = diaryLogic.GetPassword().ToString();
            PinVM = "Введите пароль";
            countInputPassword = 0;
            IsEnabled = true;
            IsCanClick = true;
        }

        public string Condition { get; set; }

        public bool IsCanDelete { get; set; }

        public string PinVM { get; set; }

        public ICommand PasswordInput
        {
            get
            {
                passwordInput = new RelayCommand<object>(InputPassword);
                return passwordInput;
            }
        }

        public ICommand SecretPassword
        {
            get 
            {
                return new RelayCommand(() =>
                {
                    count++;
                    if (count == 67)
                    {
                        PinVM = "Ваш пароль: " + userPassword;
                    }
                });
            }
        }

        public Brush ColorFirst { get; set; }

        public Brush ColorSecond { get; set; }

        public Brush ColorThird { get; set; }

        public Brush ColorFourth { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsCanClick { get; set; }

        public string Information { get; private set; }

        private async void InputPassword(object parameter)
        {
            if (parameter.ToString() != "delete")
            {
                password += parameter.ToString();
                if (password.Length == 1)
                {
                    IsCanDelete = true;
                    ColorFirst = (Brush)new BrushConverter().ConvertFromString("PaleGoldenrod");
                }
                else if (password.Length == 2)
                {
                    ColorSecond = (Brush)new BrushConverter().ConvertFromString("PaleGoldenrod");
                }
                else if (password.Length == 3)
                {
                    ColorThird = (Brush)new BrushConverter().ConvertFromString("PaleGoldenrod");
                }
                else
                {
                    ColorFourth = (Brush)new BrushConverter().ConvertFromString("PaleGoldenrod");
                    IsEnabled = false;
                    await Task.Delay(400);
                    IsEnabled = true;
                    countInputPassword++;
                    if (password == userPassword)
                    {
                        Information = "Введён пароль";
                        Condition = "Collapsed";
                    }
                    else
                    {
                        UpdatePasswordView(color: "#FF7F50");
                        password = "";
                        IsCanClick = false;
                        await Task.Delay(328);
                        countInputPassword = 1;
                        IsCanClick = true;
                        UpdatePasswordView();
                    }
                }
            }
            else
            {
                string rewriteAfterDelete = "";
                for (int i = 0; i < (password.Length - 1); i++)
                {
                    rewriteAfterDelete += password[i];
                }
                password = rewriteAfterDelete;
                if (password.Length == 0)
                {
                    ColorFirst = (Brush)new BrushConverter().ConvertFromString("White");
                    IsCanDelete = false;
                }
                else if (password.Length == 1)
                {
                    ColorSecond = (Brush)new BrushConverter().ConvertFromString("White");
                }
                else if (password.Length == 2)
                {
                    ColorThird = (Brush)new BrushConverter().ConvertFromString("White");
                }
                else
                {
                    ColorFourth = (Brush)new BrushConverter().ConvertFromString("White");
                }
            }
            Console.WriteLine(password);
        }

        private void UpdatePasswordView(string color = "White", bool canDelete = false)
        {
            ColorFirst = (Brush)new BrushConverter().ConvertFromString(color);
            ColorSecond = (Brush)new BrushConverter().ConvertFromString(color); ;
            ColorThird = (Brush)new BrushConverter().ConvertFromString(color); ;
            ColorFourth = (Brush)new BrushConverter().ConvertFromString(color); ;
            IsCanDelete = canDelete;
        }
    }
}