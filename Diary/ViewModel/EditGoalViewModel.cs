using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Diary.ViewModel
{
    class EditGoalViewModel : ViewModelBase
    {
        private GoalsLogic goalsLogic;
        private string titleGoal;
        private int id;

        public EditGoalViewModel() { }

        public EditGoalViewModel(GoalsLogic goalsLogic, int id, string text, string description)
        {
            this.goalsLogic = goalsLogic;
            GoalsLogic = this.goalsLogic;
            Condition = "Visible";
            TitleGoal = text;
            DescriptionGoal = description;
            this.id = id;
            BackgroundTitle = (Brush)new BrushConverter().ConvertFromString("White");
            Status = "Закрытие";
        }

        public string Condition { get; set; }

        public GoalsLogic GoalsLogic { get; set; }

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
                        goalsLogic.UpdateGoal(id, TitleGoal, DescriptionGoal);
                        GoalsLogic = goalsLogic;
                        Status = "Сохранение и закрытие";
                        Condition = "Collapsed";
                    }
                    else
                    {
                        RewriteBackgroundAsync();
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

        public string Status { get; private set; }

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