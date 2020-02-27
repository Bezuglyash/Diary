using SQLite;

namespace Diary.DataBase
{
    class Goal
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { set; get; }

        [MaxLength(40), NotNull]
        public string Title { get; set; }

        public string Description { get; set; }

        [MaxLength(10), NotNull]
        public string DateCreate { get; set; }

        [NotNull]
        public int IsDone { get; set; }
    }
}