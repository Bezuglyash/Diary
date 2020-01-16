using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;
using System.Collections.Generic;

namespace Diary.Model.ImportantDatesLogic
{
    class ImportantDatesLogic : ViewModelBase
    {
        private SQLiteConnection dataBase;

        public ImportantDatesLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            ImportantDates = this.dataBase.Table<ImportantDate>();
            NumberOfDates = this.dataBase.Table<ImportantDate>().Count().ToString();
            if (NumberOfDates == "0")
            {
                AutomaticFilling();
            }
        }

        public IEnumerable<ImportantDate> ImportantDates { get; }

        public string NumberOfDates { get; set; }

        public void AddNewEvent(string eventOfThisDate, string date, int isAnnually)
        {
            ImportantDate importantDate = new ImportantDate();
            importantDate.Event = eventOfThisDate;
            importantDate.Date = date;
            importantDate.IsAnnually = isAnnually;
            dataBase.Insert(importantDate);
            NumberOfDates = dataBase.Table<ImportantDate>().Count().ToString();
        }

        private void AutomaticFilling()
        {
            ImportantDate importantDate = new ImportantDate();
            importantDate.Event = "Новый год";
            importantDate.Date = "01.01.1900";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "Рождество";
            importantDate.Date = "07.01.1900";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "День всех влюблённых";
            importantDate.Date = "14.02.1900";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "День защитника Отечества";
            importantDate.Date = "23.02.1900";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "Международный женский день";
            importantDate.Date = "08.03.1900";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "Праздник Весны и Труда";
            importantDate.Date = "01.05.1900";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "День Победы";
            importantDate.Date = "09.05.1945";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "День России";
            importantDate.Date = "12.06.1992";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "День знаний";
            importantDate.Date = "01.09.1984";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "День народного единства";
            importantDate.Date = "04.11.2005";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
            importantDate.Event = "День матери";
            importantDate.Date = "11.1998";
            importantDate.IsAnnually = 1;
            dataBase.Insert(importantDate);
        }
    }
}