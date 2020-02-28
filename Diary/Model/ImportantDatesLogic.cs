using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diary.Model.ImportantDatesLogic
{
    class ImportantDatesLogic : ViewModelBase
    {
        private SQLiteConnection dataBase;
        private List<ImportantDate> importantDates;

        public ImportantDatesLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            StandardActions();
            importantDates = new List<ImportantDate>();
            if (NumberOfDates == 0)
            {
                AutomaticFillingAsync();
            }
            else
            {
                ToList();
            }
        }

        public IEnumerable<ImportantDate> ImportantDates { get; set; }

        public int NumberOfDates { get; set; }

        public void AddNewEvent(string eventOfThisDate, string date, int isAnnually)
        {
            ImportantDate importantDate = new ImportantDate();
            importantDate.Event = eventOfThisDate;
            importantDate.Date = date;
            importantDate.IsAnnually = isAnnually;
            dataBase.Insert(importantDate);
            StandardActions();
        }

        public void UpdateData(int id, string eventOfThisDate, string date, int isAnnually)
        {
            ImportantDate importantDate = GetElementById(id);
            importantDate.Date = date;
            importantDate.Event = eventOfThisDate;
            importantDate.IsAnnually = isAnnually;
            dataBase.Update(importantDate);
            StandardActions();
        }

        public void DeleteEvent(string eventOfThisDate, string date)
        {
            dataBase.Delete<ImportantDate>(GetElementByEventAndDate(eventOfThisDate, date).Id);
            StandardActions();
        }

        public List<string> GetEvents (string date, int year, bool isLastSundayInThisNovember = false)
        {
            List<string> result = new List<string>();
            foreach(var importantDate in importantDates)
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
                else if (isLastSundayInThisNovember == true && importantDate.Event == "День матери")
                {
                    result.Add(importantDate.Event);
                }
            }
            return result;
        }

        public bool IsHaveEvents (string date, int year)
        {
            foreach (var importantDate in importantDates)
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
            return false;
        }

        public int GetAnnually (string date, string eventer)
        {
            foreach (var importantDate in importantDates)
            {
                if (importantDate.Date.Remove(5, 5) == date && importantDate.Event == eventer)
                {
                    return importantDate.IsAnnually;
                }
                else if (importantDate.Event == eventer)
                {
                    if (eventer == "День матери")
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

        public void UpdateToList(int id, string eventOfThisDate, string date, int isAnnually)
        {
            for (int i = 0; i < importantDates.Count; i++)
            {
                if (importantDates[i].Id == id)
                {
                    importantDates[i].Date = date;
                    importantDates[i].Event = eventOfThisDate;
                    importantDates[i].IsAnnually = isAnnually;
                }
            }
        }

        public void DeleteToList(string eventOfThisDate, string date)
        {
            for (int i = 0; i < importantDates.Count; i++)
            {
                if (importantDates[i].Date.Remove(5, 5) == date)
                {
                    if (importantDates[i].Event == eventOfThisDate)
                    {
                        importantDates.RemoveAt(i);
                        return;
                    }
                }
                else if (importantDates[i].Event == eventOfThisDate)
                {
                    if (eventOfThisDate == "День матери")
                    {
                        importantDates.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        public string[] GetEventDate(string eventer, string shirtDate) // Для дня матери автоматическое определение
        {
            foreach (var importantDate in importantDates)
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
            return null;
        }

        public int GetIdByEventAndDate(string eventOfThisDate, string date)
        {
            IEnumerator<ImportantDate> iterator = ImportantDates.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Date == date)
                {
                    if (iterator.Current.Event == eventOfThisDate)
                    {
                        return iterator.Current.Id;
                    }
                }
                else if (iterator.Current.Event == eventOfThisDate)
                {
                    if (eventOfThisDate == "День матери")
                    {
                        return iterator.Current.Id;
                    }
                }
            }
            return -1;
        }

        public void UpdateDataAndList()
        {
            StandardActions();
            importantDates.Clear();
            ToList();
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
                if (iterator.Current.Date.Remove(5, 5) == date)
                {
                    if (iterator.Current.Event == eventOfThisDate)
                    {
                        return iterator.Current;
                    }
                }
                else if (iterator.Current.Event == eventOfThisDate)
                {
                    if (eventOfThisDate == "День матери")
                    {
                        return iterator.Current;
                    }
                }
            }
            return null;
        }

        private ImportantDate GetElementById(int id)
        {
            IEnumerator<ImportantDate> iterator = ImportantDates.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Id == id)
                {
                    return iterator.Current;
                }
            }
            return null;
        }

        private void StandardActions()
        {
            ImportantDates = dataBase.Table<ImportantDate>();
            NumberOfDates = dataBase.Table<ImportantDate>().Count();
        }

        private async void AutomaticFillingAsync()
        {
            await Task.Run(() =>
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
                importantDate.Date = "24 25 26 27 28 29 30.11.1998";
                importantDate.IsAnnually = 1;
                dataBase.Insert(importantDate);
            });
        }
    }
}