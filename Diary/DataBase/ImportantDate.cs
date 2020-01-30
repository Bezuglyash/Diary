using SQLite;

namespace Diary.DataBase
{
    class ImportantDate
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { set; get; }

        [MaxLength(10), NotNull]
        public string Date { get; set; }

        [MaxLength(41), NotNull]
        public string Event { get; set; }

        [NotNull]
        public int IsAnnually { get; set; }
    }
}