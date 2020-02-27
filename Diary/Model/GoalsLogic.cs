using System.Collections.Generic;
using System.Threading.Tasks;
using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;

namespace Diary.Model
{
    class GoalsLogic : ViewModelBase
    {
        private SQLiteConnection dataBase;
        private List<Goal> goals;

        public GoalsLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            NumberOfGoals = dataBase.Table<Goal>().Count();
            goals = new List<Goal>();
            if (NumberOfGoals > 0)
            {
                ToList();
                BubbleSortGoals();
            }
        }

        public int NumberOfGoals { get; set; }

        public void AddNewGoal(string title, string description, string date)
        {
            Goal goal = new Goal();
            goal.Title = title;
            goal.Description = description;
            goal.DateCreate = date;
            goal.IsDone = 0;
            if (NumberOfGoals > 0)
            {
                goal.Id = goals[0].Id + 1;
            }
            goals.Add(goal);
            NumberOfGoals = goals.Count;
            BubbleSortGoals();
            AddNewGoalAsync(goal);
        }

        public void UpdateGoal(int id, int isCheck)
        {
            Goal goal = SearchElementById(id);
            goal.IsDone = isCheck;
            UpdateGoalAsync(goal);
        }

        public void UpdateGoal(int id, string text, string description)
        {
            Goal goal = SearchElementById(id);
            goal.Title = text;
            goal.Description = description;
            UpdateGoalAsync(goal);
        }

        public void DeleteGoal(int id)
        {
            Goal deleteElement = SearchElementById(id);
            goals.Remove(deleteElement);
            NumberOfGoals = goals.Count;
            DeleteGoalAsync(deleteElement);
        }

        public List<Goal> GetAllGoals()
        {
            return goals;
        }

        private async void AddNewGoalAsync(Goal goal)
        {
            await Task.Run(() =>
            {
                dataBase.Insert(goal);
            });
        }

        private async void UpdateGoalAsync(Goal goal)
        {
            await Task.Run(() =>
            {
                dataBase.Update(goal);
            });
        }

        private async void DeleteGoalAsync(Goal goal)
        {
            await Task.Run(() =>
            {
                dataBase.Delete(goal);
            });
        }

        private void ToList()
        {
            IEnumerator<Goal> iterator = dataBase.Table<Goal>().GetEnumerator();
            while (iterator.MoveNext())
            {
                goals.Add(iterator.Current);
            }
        }

        private void BubbleSortGoals()
        {
            for (int i = 0; i < goals.Count; i++)
            {
                for (int j = 0; j < goals.Count - 1; j++)
                {
                    if (goals[j].Id < goals[j + 1].Id)
                    {
                        Goal swapper = goals[j];
                        goals[j] = goals[j + 1];
                        goals[j + 1] = swapper;
                    }
                }
            }
        }

        private Goal SearchElementById(int id)
        {
            foreach (var goal in goals)
            {
                if (goal.Id == id)
                {
                    return goal;
                }
            }
            return null;
        }
    }
}