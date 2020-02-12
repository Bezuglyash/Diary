using Diary.Another;
using Diary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace Diary.ViewModel
{
    class NewHabitTrackerViewModel : ViewModelBase
    {
        private HabitsTrackerLogic habitsTrackerLogic;
        private Dispatcher dispatcher;

        public NewHabitTrackerViewModel() { }

        public NewHabitTrackerViewModel (HabitsTrackerLogic habitsTrackerLogic)
        {
            this.habitsTrackerLogic = habitsTrackerLogic;
            EditHabit = new RelayCommand<int>(Edit);
            DeleteHabit = new RelayCommand<int>(Delete);
            Condition = "Visible";
            ListOfHabits = new ObservableCollection<DataHabit>();
            SearchingHabits();
            Count = ListOfHabits.Count;
            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public ObservableCollection<DataHabit> ListOfHabits { get; set; }

        public int Count { get; set; }

        public string Condition { get; set; }

        public ICommand AddNewHabit
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (ListOfHabits.Count > 0)
                    {
                        ListOfHabits.Add(new DataHabit(GetMaxIndex() + 1));
                    }
                    else
                    {
                        ListOfHabits.Add(new DataHabit(1));
                    }
                    Count = ListOfHabits.Count;
                });
            }
        }

        public RelayCommand<int> EditHabit { get; }

        public RelayCommand<int> DeleteHabit { get; }

        public ICommand SaveTracker
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (IsNotNull() == true)
                    {
                        Thread thread = new Thread(new ThreadStart(RewriteData));
                        Condition = "Collapsed";
                        thread.Start();
                    }
                    else
                    {
                        new MessageBoxViewModel("Не все поля заполнены! Сохранение невозможно!", "Предупреждение").ShowWarning();
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

        private void SearchingHabits()
        {
            ListOfHabits = habitsTrackerLogic.GetDataHabits();
        }

        private void Edit(int index)
        {
            DataHabit data = GetDataHabit(index);
            if (data.IsOnlyRead == false)
            {
                data.IsOnlyRead = true;
            }
            else
            {
                data.IsOnlyRead = false;
            }
        }

        private void Delete(int index)
        {
            ListOfHabits.Remove(GetDataHabit(index));
            Count = ListOfHabits.Count;
        }

        private void RewriteData()
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                habitsTrackerLogic.Distribution(ToList(), DateTime.Now.ToString("dd") + "." + DateTime.Now.ToString("MM") + "." + DateTime.Now.ToString("yyyy"));
            });
        }

        private int GetMaxIndex()
        {
            return ListOfHabits[ListOfHabits.Count - 1].Index;
        }

        private DataHabit GetDataHabit(int index)
        {
            IEnumerator<DataHabit> iterator = ListOfHabits.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Index == index)
                {
                    return iterator.Current;
                }
            }
            return null;
        }

        private bool IsNotNull()
        {
            IEnumerator<DataHabit> iterator = ListOfHabits.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Habit == "" || iterator.Current.IsOnlyOneGaps() == true)
                {
                    return false;
                }
            }
            return true;
        }

        private List<DataHabit> ToList()
        {
            List<DataHabit> dataHabits = new List<DataHabit>();
            IEnumerator<DataHabit> iterator = ListOfHabits.GetEnumerator();
            while (iterator.MoveNext())
            {
                dataHabits.Add(iterator.Current);
            }
            return dataHabits;
        }
    }
}