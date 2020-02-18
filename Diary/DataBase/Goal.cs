using SQLite;

namespace Diary.DataBase
{
    class Goal
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { set; get; }

        [MaxLength(40), NotNull]
        public string Title { get; set; }

        public string Discription { get; set; }

        [NotNull]
        public int IsDone { get; set; }
    }
}