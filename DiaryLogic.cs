using System;
using SQLite;

namespace Diary
{
    class DiaryLogic
    {
        private const string NAME_DATA_BASE = "Diary.db";
        private SQLiteConnection dataBase;
        private User user;
        private bool itWasOpen;

        public DiaryLogic()
        {
            try
            {
                itWasOpen = true;
                dataBase = new SQLiteConnection(NAME_DATA_BASE, SQLiteOpenFlags.ReadWrite, true);
            }
            catch (Exception)
            {
                itWasOpen = false;
            }
            user = new User();
        }
    }
}