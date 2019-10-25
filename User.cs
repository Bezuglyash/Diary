using SQLite;

namespace Diary
{
    class User
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [MaxLength(30), NotNull]
        public string Name { get; set; }

        [MaxLength(4)]
        public int Password { get; set; }

        public bool IsHavePassword { get; set; }
    }
}