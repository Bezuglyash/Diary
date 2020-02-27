using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using static System.Threading.Thread;

namespace Diary.ViewModel
{
    class NewGoalViewModel : ViewModelBase
    {
        private GoalsLogic goalsLogic;
        private string titleGoal;

        public NewGoalViewModel() { }

        public NewGoalViewModel(GoalsLogic goalsLogic)
        {
            this.goalsLogic = goalsLogic;
            Condition = "Visible";
            TitleGoal = "";
            DescriptionGoal = "";
            BackgroundTitle = (Brush)new BrushConverter().ConvertFromString("White");
        }

        public string Condition { get; set; }

        public string TitleGoal
        {
            get { return titleGoal; }
            set
            {
                if (value.Length <= 40)
                {
                    titleGoal = value;
                }
            }
        }

        public string DescriptionGoal { get; set; }

        public ICommand SaveGoal
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (TitleGoal.Length != 0 && IsNotOnlyGaps())
                    {
                        goalsLogic.AddNewGoal(TitleGoal, DescriptionGoal, DateTime.Now.ToString("dd.MM.yyyy"));
                        Condition = "Collapsed";
                    }
                    else
                    {
                        RewriteBackgroundAsync();
                        //TitleGoal = "Привет!";
                    }
                });
            }
        }

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

        public Brush BackgroundTitle { get; set; }

        private async void RewriteBackgroundAsync()
        {
            BackgroundTitle = (Brush)new BrushConverter().ConvertFromString("#FF7F50");
            await Task.Delay(1000);
            BackgroundTitle = (Brush)new BrushConverter().ConvertFromString("White");
        }

        private bool IsNotOnlyGaps()
        {
            int countGaps = 0;
            for (int i = 0; i < titleGoal.Length; i++)
            {
                if (titleGoal[i] == ' ')
                {
                        countGaps++;
                }
            }
            return countGaps != titleGoal.Length;
        }
    }
}