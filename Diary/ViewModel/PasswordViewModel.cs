using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Diary.ViewModel
{
    class PasswordViewModel : ViewModelBase
    {
        private DiaryLogic diaryLogic;
        private string potentialPassword;
        private int? password;
        private int countInputPassword;
        private ICommand _passwordInput;

        public PasswordViewModel() { }

        public PasswordViewModel(DiaryLogic diaryLogic)
        {
            this.diaryLogic = diaryLogic;
            IsCanBack = false;
            IsCanDelete = false;
            password = diaryLogic.UserPassword;
            PinVM = "Придумайте 4-значный PIN-код";
            countInputPassword = 0;
            IsEnabled = true;
        }

        public bool IsCanBack { get; set; }

        public bool IsCanDelete { get; set; }

        public string PinVM { get; set; }

        public ICommand PasswordInput
        {
            get
            {
                _passwordInput = new RelayCommand<object>(InputPassword);
                return _passwordInput;
            }
        }

        async private void InputPassword(object parameter)
        {
            if (parameter.ToString() == "back")
            {
                countInputPassword = 0;
                potentialPassword = "";
                UdpatePasswordView(newText: "Придумайте 4-значный PIN-код", canBack: false);
            }
            else if (parameter.ToString() != "delete")
            {
                potentialPassword += parameter;
                if (potentialPassword.Length == 1)
                {
                    IsCanDelete = true;
                    ColorFirst = (Brush)new BrushConverter().ConvertFromString("Gray");
                }
                else if (potentialPassword.Length == 2)
                {
                    ColorSecond = (Brush)new BrushConverter().ConvertFromString("Gray");
                }
                else if (potentialPassword.Length == 3)
                {
                    ColorThird = (Brush)new BrushConverter().ConvertFromString("Gray");
                }
                else
                {
                    ColorFourth = (Brush)new BrushConverter().ConvertFromString("Gray");
                    IsEnabled = false;
                    await Task.Delay(400);
                    IsEnabled = true;
                    countInputPassword++;
                    if (countInputPassword == 1)
                    {
                        UdpatePasswordView();
                        password = Convert.ToInt32(potentialPassword);
                        potentialPassword = "";
                    }
                    else
                    {
                        if (password == Convert.ToInt32(potentialPassword))
                        {
                            diaryLogic.UserPassword = password;
                        }
                        else
                        {
                            UdpatePasswordView(color: "#FF7F50");
                            await Task.Delay(328);
                            potentialPassword = "";
                            countInputPassword = 1;
                            UdpatePasswordView();
                        }
                    }
                }
            }
            else
            {
                string rewriteAfterDelete = "";
                for (int i = 0; i < (potentialPassword.Length - 1); i++)
                {
                    rewriteAfterDelete += potentialPassword[i];
                }
                potentialPassword = rewriteAfterDelete;
                if (potentialPassword.Length == 0)
                {
                    ColorFirst = (Brush)new BrushConverter().ConvertFromString("White");
                    IsCanDelete = false;
                }
                else if (potentialPassword.Length == 1)
                {
                    ColorSecond = (Brush)new BrushConverter().ConvertFromString("White");
                }
                else if (potentialPassword.Length == 2)
                {
                    ColorThird = (Brush)new BrushConverter().ConvertFromString("White");
                }
                else
                {
                    ColorFourth = (Brush)new BrushConverter().ConvertFromString("White");
                }
            }
            Console.WriteLine(potentialPassword);
        }

        private void UdpatePasswordView(string color = "White", string newText = "Повторите 4-значный PIN-код", bool canDelete = false, bool canBack = true)
        {
            ColorFirst = (Brush)new BrushConverter().ConvertFromString(color);
            ColorSecond = (Brush)new BrushConverter().ConvertFromString(color); ;
            ColorThird = (Brush)new BrushConverter().ConvertFromString(color); ;
            ColorFourth = (Brush)new BrushConverter().ConvertFromString(color); ;
            IsCanDelete = canDelete;
            PinVM = newText;
            IsCanBack = canBack;
        }

        public Brush ColorFirst { get; set; }

        public Brush ColorSecond { get; set; }

        public Brush ColorThird { get; set; }

        public Brush ColorFourth { get; set; }

        public bool IsEnabled { get; set; }
    }
}