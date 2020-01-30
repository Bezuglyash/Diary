using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;
using System;
using System.Collections.Generic;

namespace Diary.Model.ImportantDatesLogic
{
    class ImportantDatesLogic : ViewModelBase
    {
        private SQLiteConnection dataBase;
        private List<ImportantDate> importantDates;

        public ImportantDatesLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            StandartActions();
            if (NumberOfDates == "0")
            {
                AutomaticFilling();
            }
            else
            {
                importantDates = new List<ImportantDate>();
                ToList();
            }
        }

        public IEnumerable<ImportantDate> ImportantDates { get; set; }

        public string NumberOfDates { get; set; }

        public void AddNewEvent(string eventOfThisDate, string date, int isAnnually)
        {
            ImportantDate importantDate = new ImportantDate();
            importantDate.Event = eventOfThisDate;
            importantDate.Date = date;
            importantDate.IsAnnually = isAnnually;
            dataBase.Insert(importantDate);
            StandartActions();
        }

        public void DeleteEvent(string eventOfThisDate, string date)
        {
            dataBase.Delete<ImportantDate>(GetElementByEventAndDate(eventOfThisDate, date).Id);
            StandartActions();
        }

        public List<string> GetEvents (string date, int year)
        {
            List<string> result = new List<string>();
            foreach(var importantDate in importantDates)
            {
                if (importantDate.Date.Length == 10)
                {
                    if (importantDate.Date.Remove(5, 5) == date)
                    {
                        if (importantDate.IsAnnually == 1)
                        {
                            if (year - Convert.ToInt32(importantDate.Date.Remove(0, 6)) >= 0)
                            {
                                result.Add(importantDate.Event);
                            }
                        }
                        else
                        {
                            if (year - Convert.ToInt32(importantDate.Date.Remove(0, 6)) == 0)
                            {
                                result.Add(importantDate.Event);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool IsHaveEvents (string date, int year)
        {
            foreach (var importantDate in importantDates)
            {
                if (importantDate.Date.Length == 10)
                {
                    if (importantDate.Date.Remove(5, 5) == date)
                    {
                        if (importantDate.IsAnnually == 1)
                        {
                            if (year - Convert.ToInt32(importantDate.Date.Remove(0, 6)) >= 0)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (year - Convert.ToInt32(importantDate.Date.Remove(0, 6)) == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public int GetAnnually (string date, string eventer)
        {
            foreach (var importantDate in importantDates)
            {
                if (importantDate.Date.Length == 10)
                {
                    if (importantDate.Date.Remove(5, 5) == date && importantDate.Event == eventer)
                    {
                        return importantDate.IsAnnually;
                    }
                }
            }
            return -1;
        }

        public void AddToList(string eventOfThisDate, string date, int isAnnually)
        {
            ImportantDate importantDate = new ImportantDate();
            importantDate.Event = eventOfThisDate;
            importantDate.Date = date;
            importantDate.IsAnnually = isAnnually;
            importantDates.Add(importantDate);
        }

        public void DeleteToList(string eventOfThisDate, string date)
        {
            for (int i = 0; i < importantDates.Count; i++)
            {
                if (importantDates[i].Date.Length == 10)
                {
                    if (importantDates[i].Date.Remove(5, 5) == date)
                    {
                        if (importantDates[i].Event == eventOfThisDate)
                        {
                            importantDates.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }

        public string[] GetEventDate(string eventer, string shirtDate)
        {
            foreach (var importantDate in importantDates)
            {
                if (importantDate.Date.Length == 10)
                {
                    if (importantDate.Event == eventer && importantDate.Date.Remove(5, 5) == shirtDate)
                    {
                        return importantDate.Date.Split(new char[] { '.' });
                    }
                    else if (importantDate.Event == eventer && importantDate.Date.Remove(5, 5) == "29.02")
                    {
                        return importantDate.Date.Split(new char[] { '.' });
                    }
                }
            }
            return null;
        }

        private void ToList()
        {
            IEnumerator<ImportantDate> iterator = ImportantDates.GetEnumerator();
            while (iterator.MoveNext())
            {
                importantDates.Add(iterator.Current);
            }
        }

        private ImportantDate GetElementByEventAndDate(string eventOfThisDate, string date)
        {
            IEnumerator<ImportantDate> iterator = ImportantDates.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Date.Length == 10)
                {
                    if (iterator.Current.Date.Remove(5, 5) == date)
                    {
                        if (iterator.Current.Event == eventOfThisDate)
                        {
                            return iterator.Current;
                        }
                    }
                }
            }
            return null;
        }

        private void StandartActions()
        {
            ImportantDates = this.dataBase.Table<ImportantDate>();
            NumberOfDates = this.dataBase.Table<ImportantDate>().Count().ToString();
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