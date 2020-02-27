using System;
using System.IO.Packaging;
using System.Windows.Input;
using System.Windows.Media;
using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Diary.ViewModel
{
    class SeeBasketViewModel : ViewModelBase
    {
        public SeeBasketViewModel() { }

        public SeeBasketViewModel(string text, string date)
        {
            Condition = "Visible";
            Date = date;
            Text = text;
            CountBorder = 0;
        }

        public string Condition { get; set; }

        public string Date { get; set; }

        public string Text { get; set; }

        public ICommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Condition = "Collapsed";
                });
            }
        }

        public string ButtonEdit { get; set; }

        public int CountBorder { get; set; }

    }
}