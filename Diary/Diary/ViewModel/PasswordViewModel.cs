using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Diary.ViewModel
{
    class PasswordViewModel : ViewModelBase
    {
        private DiaryLogic diaryLogic;
        private bool isCanBack;
        private bool isCanDelete;
        private string stringHelper;
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

        public bool IsCanBack
        {
            get { return isCanBack; }
            set
            {
                isCanBack = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCanDelete
        {
            get { return isCanDelete; }
            set
            {
                isCanDelete = value;
                RaisePropertyChanged();
            }
        }

        public string PinVM
        {
            get { return stringHelper; }
            set
            {
                stringHelper = value;
                RaisePropertyChanged();
            }
        }

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
                    ColorFirst = "Gray";
                }
                else if (potentialPassword.Length == 2)
                {
                    ColorSecond = "Gray";
                }
                else if (potentialPassword.Length == 3)
                {
                    ColorThird = "Gray";
                }
                else
                {
                    ColorFourth = "Gray";
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
                    ColorFirst = "White";
                    IsCanDelete = false;
                }
                else if (potentialPassword.Length == 1)
                {
                    ColorSecond = "White";
                }
                else if (potentialPassword.Length == 2)
                {
                    ColorThird = "White";
                }
                else
                {
                    ColorFourth = "White";
                }
            }
            Console.WriteLine(potentialPassword);
        }

        private void UdpatePasswordView(string color = "White", string newText = "Повторите 4-значный PIN-код", bool canDelete = false, bool canBack = true)
        {
            ColorFirst = color;
            ColorSecond = color;
            ColorThird = color;
            ColorFourth = color;
            IsCanDelete = canDelete;
            PinVM = newText;
            IsCanBack = canBack;
        }

        public string ColorFirst { get; set; }

        public string ColorSecond { get; set; }

        public string ColorThird { get; set; }

        public string ColorFourth { get; set; }

        public bool IsEnabled { get; set; }
    }
}