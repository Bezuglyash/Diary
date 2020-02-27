using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diary.DataBase;
using Diary.Model;
using Diary.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Diary.ViewModel
{
    class AllGoalsViewModel : ViewModelBase
    {
        private GoalsLogic goalsLogic;
        private EditGoalViewModel editGoalViewModel;

        public AllGoalsViewModel() { }

        public AllGoalsViewModel(GoalsLogic goalsLogic)
        {
            this.goalsLogic = goalsLogic;
            Condition = "Visible";
            Goals = new ObservableCollection<Goal>(this.goalsLogic.GetAllGoals());
            EditAndViewGoal = new RelayCommand<int>(EditViewGoal);
            GoalCheck = new RelayCommand<int>(Check);
            DeleteGoal = new RelayCommand<int>(Delete);
            Count = Goals.Count;
        }

        public string Condition { get; set; }

        public ObservableCollection<Goal> Goals { get; set; }

        public int Count { get; set; }

        public RelayCommand<int> EditAndViewGoal { get; }

        public RelayCommand<int> GoalCheck { get; }

        public RelayCommand<int> DeleteGoal { get; }

        public ICommand BackSpace
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Condition = "Collapsed";
                });
            }
        }

        public FrameworkElement GoToViewAndEditGoal { get; set; }

        public bool IsClose { get; set; }

        private void EditViewGoal(int id)
        {
            int index = GetIndexById(id);
            editGoalViewModel = new EditGoalViewModel(goalsLogic, id, Goals[index].Title, Goals[index].Description);
            GoToViewAndEditGoal = new EditGoalView();
            GoToViewAndEditGoal.DataContext = editGoalViewModel;
            WaitingEdit();
        }

        private void Check(int id)
        {
            goalsLogic.UpdateGoal(id, Goals[GetIndexById(id)].IsDone);
        }

        private void Delete(int id)
        {
            if (new MessageBoxViewModel("Вы действительно хотите удалить цель?", "Удаление").ShowMessage() == true)
            {
                int index = GetIndexById(id);
                Goals.RemoveAt(index);
                goalsLogic.DeleteGoal(id);
                Count = Goals.Count;
            }
        }

        private async void WaitingEdit()
        {
            await Task.Run(() =>
            {
                while(IsClose != true && editGoalViewModel.Condition != "Collapsed") { }
                if (editGoalViewModel.Status == "Сохранение и закрытие")
                {
                    goalsLogic = editGoalViewModel.GoalsLogic;
                    Goals = new ObservableCollection<Goal>(this.goalsLogic.GetAllGoals());
                }
            });
        }

        private int GetIndexById(int id)
        {
            int index = 0;
            foreach (var goal in Goals)
            {
                if (goal.Id == id)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}