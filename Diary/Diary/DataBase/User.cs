using SQLite;

namespace Diary.DataBase
{
    class User
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [MaxLength(30), NotNull]
        public string Name { get; set; }

        [MaxLength(4)]
        public int? Password { get; set; }

        [NotNull]
        public int IsHavePassword { get; set; }
    }
}