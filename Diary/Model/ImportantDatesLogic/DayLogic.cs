using GalaSoft.MvvmLight;

namespace Diary.Model.ImportantDatesLogic
{
    class DayLogic : ViewModelBase
    {
        public DayLogic(int numberDayOfWeek)
        {
            NumberDayOfWeek = numberDayOfWeek;
        }

        public int NumberDayOfWeek { get; private set; }
    }
}