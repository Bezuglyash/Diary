﻿using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows;
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
        private ICommand passwordInput;

        public PasswordViewModel() { }

        public PasswordViewModel(DiaryLogic diaryLogic)
        {
            this.diaryLogic = diaryLogic;
            Condition = "Visible";
            IsCanBack = false;
            IsCanDelete = false;
            password = diaryLogic.GetPassword();
            PinVM = "Придумайте 4-значный PIN-код";
            countInputPassword = 0;
            IsEnabled = true;
        }

        public string Condition { get; set; }

        public bool IsCanBack { get; set; }

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

        public Brush ColorFirst { get; set; }

        public Brush ColorSecond { get; set; }

        public Brush ColorThird { get; set; }

        public Brush ColorFourth { get; set; }

        public bool IsEnabled { get; set; }

        public string Information { get; private set; }

        private async void InputPassword(object parameter)
        {
            if (parameter.ToString() == "back")
            {
                countInputPassword = 0;
                potentialPassword = "";
                UpdatePasswordView(newText: "Придумайте 4-значный PIN-код", canBack: false);
            }
            else if (parameter.ToString() != "delete")
            {
                potentialPassword += parameter;
                if (potentialPassword.Length == 1)
                {
                    IsCanDelete = true;
                    ColorFirst = (Brush)new BrushConverter().ConvertFromString("PaleGoldenrod");
                }
                else if (potentialPassword.Length == 2)
                {
                    ColorSecond = (Brush)new BrushConverter().ConvertFromString("PaleGoldenrod");
                }
                else if (potentialPassword.Length == 3)
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
                    if (countInputPassword == 1)
                    {
                        UpdatePasswordView();
                        password = Convert.ToInt32(potentialPassword);
                        potentialPassword = "";
                    }
                    else
                    {
                        if (password == Convert.ToInt32(potentialPassword))
                        {
                            diaryLogic.AddPassword(password);
                            Information = "Введён пароль";
                        }
                        else
                        {
                            UpdatePasswordView(color: "#FF7F50");
                            await Task.Delay(328);
                            potentialPassword = "";
                            countInputPassword = 1;
                            UpdatePasswordView();
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
        }

        private void UpdatePasswordView(string color = "White", string newText = "Повторите 4-значный PIN-код", bool canDelete = false, bool canBack = true)
        {
            ColorFirst = (Brush)new BrushConverter().ConvertFromString(color);
            ColorSecond = (Brush)new BrushConverter().ConvertFromString(color); ;
            ColorThird = (Brush)new BrushConverter().ConvertFromString(color); ;
            ColorFourth = (Brush)new BrushConverter().ConvertFromString(color); ;
            IsCanDelete = canDelete;
            PinVM = newText;
            IsCanBack = canBack;
        }
    }
}