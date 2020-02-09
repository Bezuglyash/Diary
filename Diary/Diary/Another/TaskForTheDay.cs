using GalaSoft.MvvmLight;

namespace Diary.Another
{
    class TaskForTheDay : ViewModelBase
    {
        private string title;

        public TaskForTheDay(int index)
        {
            Index = index;
            Title = "";
            Description = "";
            IsDone = false;
            IsNeedThis = false;
        }

        public TaskForTheDay(int index, string title, string description, int isDone)
        {
            Index = index;
            Title = title;
            Description = description;
            if (isDone == 1)
            {
                IsDone = true;
            }
            else
            {
                IsDone = false;
            }
            IsNeedThis = false;
        }
        
        public int Index { get; private set; }

        public string Title
        {
            get { return title; }
            set
            {
                if (value.Length <= 28)
                {
                    title = value;
                }
            }
        }

        public string Description { get; set; }

        public bool IsDone { get; set; }
        
        public bool IsNeedThis { get; set; }

        public bool IsOnlyOneGaps ()
        {
            int countGaps = 0;
            for(int i = 0; i < Title.Length; i++)
            {
                if (Title[i] == ' ')
                {
                    countGaps++;
                }
            }
            if (countGaps == Title.Length)
            {
                return true;
            }
            return false;
        }
    }
}