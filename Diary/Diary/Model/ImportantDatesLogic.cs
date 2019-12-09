using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;
using System.Collections.Generic;

namespace Diary.Model
{
    class ImportantDatesLogic : ViewModelBase
    {
        private IEnumerable<ImportantDate> importantDates;
        private SQLiteConnection dataBase;

        public ImportantDatesLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            importantDates = this.dataBase.Table<ImportantDate>();
            NumberOfDates = this.dataBase.Table<ImportantDate>().Count().ToString();
        }

        public IEnumerable<ImportantDate> Notes { get; }

        public string NumberOfDates { get; set; }

        public void AddNewEvent(string a, string b, int c)
        {
            ImportantDate importantDate = new ImportantDate();
            importantDate.Event = a;
            importantDate.Date = b;
            importantDate.IsAnnually = c;
            dataBase.Insert(importantDate);
            NumberOfDates = dataBase.Table<ImportantDate>().Count().ToString();
        }
    }
}