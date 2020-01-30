using GalaSoft.MvvmLight;
using System.Windows.Media;

namespace Diary.Model.ImportantDatesLogic
{
    class DayViewModel : ViewModelBase
    {
        public static int daysCount;

        public DayViewModel(int numberDayOfWeek, string marking)
        {
            NumberDayOfWeek = numberDayOfWeek;
            Condition = "Visible";
            ColorBack = (Brush)new BrushConverter().ConvertFromString("LightGoldenrodYellow");
            ColorFore = (Brush)new BrushConverter().ConvertFromString("Black");
            MarkingThePresenceOfEvents = marking;
        }

        public DayViewModel(int numberDayOfWeek, string color, string marking)
        {
            NumberDayOfWeek = numberDayOfWeek;
            Condition = "Visible";
            ColorBack = (Brush)new BrushConverter().ConvertFromString("LightGoldenrodYellow");
            ColorFore = (Brush)new BrushConverter().ConvertFromString(color);
            MarkingThePresenceOfEvents = marking;
        }

        public DayViewModel(string condition)
        {
           Condition = condition;
        }

        public int NumberDayOfWeek { get; private set; }

        public string Condition { get; set; }

        public Brush ColorBack { get; set; }

        public Brush ColorFore { get; set; }

        public string MarkingThePresenceOfEvents { get; set; }
    }
}