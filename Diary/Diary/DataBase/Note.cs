
using SQLite;
using System.Collections.Specialized;

namespace Diary.DataBase
{
    class Note
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { set; get; }

        [NotNull]
        public string NoteTitle { get; set; }

        [NotNull]
        public string NoteContent { get; set; }

        [MaxLength(10), NotNull]
        public string CreationOrEditingDate { get; set; }
    }
}