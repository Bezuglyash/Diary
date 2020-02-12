using GalaSoft.MvvmLight;

namespace Diary.Another
{
    class DataHabit : ViewModelBase
    {
        private string habit;

        public DataHabit(int index)
        {
            Index = index;
            Habit = "";
            IsOnlyRead = false;
        }

        public DataHabit(int index, string habit, bool isOnlyRead)
        {
            Index = index;
            Habit = habit;
            IsOnlyRead = isOnlyRead;
        }

        public int Index { get; set; }

        public string Habit
        {
            get { return habit; }
            set
            {
                if (value.Length <= 28)
                {
                    habit = value;
                    DeleteGaps();
                }
            }
        }

        public bool IsOnlyRead { get; set; }

        public bool IsOnlyOneGaps()
        {
            int countGaps = 0;
            for (int i = 0; i < Habit.Length; i++)
            {
                if (Habit[i] == ' ')
                {
                    countGaps++;
                }
            }
            if (countGaps == Habit.Length)
            {
                return true;
            }
            return false;
        }

        private void DeleteGaps()
        {
            if (Habit.Length > 0)
            {
                if (Habit[0] == ' ')
                {
                    int count = 1;
                    int countDeleteSymbols = 1;
                    while (count != Habit.Length && Habit[count] == ' ')
                    {
                        countDeleteSymbols++;
                        count++;
                    }
                    Habit = Habit.Remove(0, countDeleteSymbols);
                }
            }
        }
    }
}