using System.Collections.Generic;
using Diary.DataBase;
using SQLite;

namespace Diary.Model
{
    class GoalsLogic
    {
        private SQLiteConnection dataBase;
        private List<Goal> goals;

        public GoalsLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            goals = new List<Goal>();
        }
    }
}