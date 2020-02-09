using SQLite;

namespace Diary.DataBase
{
    class TimetableForTheDay
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { set; get; }

        [NotNull]
        public string DateTimetable { get; set; }

        [NotNull]
        public string Case { get; set; }

        public string Description { get; set; }

        [NotNull]
        public int IsItDone { get; set; }
    }
}