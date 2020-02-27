using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Diary.Another;
using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;

namespace Diary.Model
{
    class BasketLogic : ViewModelBase
    {
        enum Months
        {
            Января = 1,
            Февраля = 2,
            Марта = 3,
            Апреля = 4,
            Мая = 5,
            Июня = 6,
            Июля = 7,
            Августа = 8,
            Сентября = 9,
            Октября = 10,
            Ноября = 11,
            Декабря = 12
        }

        private SQLiteConnection dataBase;
        private IEnumerable<Basket> storageNotes;

        public BasketLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            Notes = this.dataBase.Table<Basket>();
            BubbleSortForDates();
            RewriteDate();
            storageNotes = Notes;
            NumberOfNotes = this.dataBase.Table<Basket>().Count();
        }

        public IEnumerable<Basket> Notes { get; set; }

        public int NumberOfNotes { get; set; }

        public async void AddNewNoteAsync(string title, string content, string date)
        {
            await Task.Run(() =>
            {
                Basket note = new Basket();
                note.NoteContent = content;
                note.NoteTitle = title;
                note.CreationOrEditingDate = date;
                dataBase.Insert(note);
                StandardSteps();
            });
        }

        public void Recover(int id)
        {
            RecoverAsync(GetElementById(id));
        }

        public void DeleteNote(int id)
        {
            dataBase.Delete<Basket>(GetElementById(id).Id);
            StandardSteps();
        }

        public async void AllDeleteAsync()
        {
            await Task.Run(() =>
            {
                dataBase.DeleteAll<Basket>();
                StandardSteps();
            });
        }

        public int GetIndexOfList(int id)
        {
            int index = 0;
            IEnumerator<Basket> iterator = Notes.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (id == iterator.Current.Id)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public void Searching(string txt)
        {
            Notes = storageNotes;
            int index = 0;
            List<Basket> notes = new List<Basket>();
            Basket note;
            note = GetElementByIndex(index);
            while (note != null)
            {
                notes.Add(note);
                index++;
                note = GetElementByIndex(index);
            }
            for (int i = 0; i < notes.Count; i++)
            {
                Regex regex = new Regex(txt.ToLower());
                MatchCollection matches = regex.Matches(notes[i].NoteTitle.ToLower());
                if (matches.Count == 0)
                {
                    notes.RemoveAt(i);
                    i -= 1;
                }
            }
            Notes = notes;
        }

        public Basket GetBasket(int id)
        {
            IEnumerator<Basket> iterator = Notes.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Id == id)
                {
                    return iterator.Current;
                }
            }
            return null;
        }

        public string GetStandardDate(int id)
        {
            IEnumerator<Basket> iterator = dataBase.Table<Basket>().GetEnumerator();
            while (iterator.MoveNext())
            {
                if (iterator.Current.Id == id)
                {
                    return iterator.Current.CreationOrEditingDate;
                }
            }
            return null;
        }

        private async void RecoverAsync(Basket basket)
        {
            await Task.Run(() =>
            {
                dataBase.Delete<Basket>(basket.Id);
                StandardSteps();
            });
        }

        private void RewriteDate()
        {
            IEnumerator<Basket> iterator = Notes.GetEnumerator();
            while (iterator.MoveNext())
            {
                iterator.Current.CreationOrEditingDate = GetDateExternal(iterator.Current.Id);
            }
        }

        private string GetDateExternal(int id)
        {
            string allDate = GetElementById(id).CreationOrEditingDate;
            Regex regex = new Regex(DateTime.Now.ToString("yyyy"));
            MatchCollection matches = regex.Matches(allDate);
            if (matches.Count == 1)
            {
                allDate = allDate.Remove(5, 5);
                regex = new Regex(DateTime.Now.ToString("dd.MM"));
                matches = regex.Matches(allDate);
                if (matches.Count == 1)
                {
                    allDate = allDate.Remove(0, 5);
                    return "Сегодня в" + allDate;
                }
                if (allDate.Split(new char[] { '.', ' ' })[1] == DateTime.Now.ToString("MM"))
                {
                    int number = Convert.ToInt32(allDate.Split(new char[] { '.' })[0]);
                    if (Convert.ToInt32(DateTime.Now.ToString("dd")) - number == 1)
                    {
                        allDate = allDate.Remove(0, 5);
                        return "Вчера в" + allDate;
                    }
                }
                else if (DateTime.Now.ToString("dd") == "01" && Convert.ToInt32(allDate.Split(new char[] { '.', ' ' })[1]) == Convert.ToInt32(DateTime.Now.ToString("MM")) - 1)
                {
                    Day day = new Day();
                    Model.Months month = (Model.Months)Convert.ToInt32(allDate.Split(new char[] { '.', ' ' })[1]);
                    if (Convert.ToInt32(allDate.Split(new char[] { '.', ' ' })[0]) == day.GetNumberOfDaysInThisMonth(month + "", Convert.ToInt32(DateTime.Now.ToString("yyyy"))))
                    {
                        allDate = allDate.Remove(0, 5);
                        return "Вчера в" + allDate;
                    }
                }
                Model.Months months = (Model.Months)Convert.ToInt32(allDate.Split(new char[] { '.', ' ' })[1]);
                if (allDate.Split(new char[] { '.', ' ' })[0][0] == '0')
                {
                    return allDate.Split(new char[] { '.', ' ' })[0][1] + " " + months + " в " + allDate.Split(new char[] { ' ' })[1];
                }
                return allDate.Split(new char[] { '.', ' ' })[0] + " " + months + " в " + allDate.Split(new char[] { ' ' })[1];
            }
            if (DateTime.Now.ToString("dd.MM") == "01.01" && (allDate.Split(new char[] { '.', ' ' })[0] + "." + allDate.Split(new char[] { '.', ' ' })[1] == "31.12"))
            {
                allDate = allDate.Remove(0, 10);
                return "Вчера в" + allDate;
            }
            return allDate.Split(new char[] { ' ' })[0] + " в " + allDate.Split(new char[] { ' ' })[1];
        }

        private void BubbleSortForDates()
        {
            List<Basket> notes = new List<Basket>();
            IEnumerator<Basket> iterator = Notes.GetEnumerator();
            while (iterator.MoveNext())
            {
                notes.Add(iterator.Current);
            }
            for (int i = 0; i < notes.Count; i++)
            {
                for (int j = 0; j < notes.Count - 1; j++)
                {
                    if (Convertation(notes[j].CreationOrEditingDate, 2) < Convertation(notes[j + 1].CreationOrEditingDate, 2))
                    {
                        Basket swapper = notes[j];
                        notes[j] = notes[j + 1];
                        notes[j + 1] = swapper;
                    }
                    else if (Convertation(notes[j].CreationOrEditingDate, 2) == Convertation(notes[j + 1].CreationOrEditingDate, 2))
                    {
                        if (Convertation(notes[j].CreationOrEditingDate, 1) < Convertation(notes[j + 1].CreationOrEditingDate, 1))
                        {
                            Basket swapper = notes[j];
                            notes[j] = notes[j + 1];
                            notes[j + 1] = swapper;
                        }
                        else if (Convertation(notes[j].CreationOrEditingDate, 1) == Convertation(notes[j + 1].CreationOrEditingDate, 1))
                        {
                            if (Convertation(notes[j].CreationOrEditingDate, 0) < Convertation(notes[j + 1].CreationOrEditingDate, 0))
                            {
                                Basket swapper = notes[j];
                                notes[j] = notes[j + 1];
                                notes[j + 1] = swapper;
                            }
                            else if (Convertation(notes[j].CreationOrEditingDate, 0) == Convertation(notes[j + 1].CreationOrEditingDate, 0))
                            {
                                if (Convertation(notes[j].CreationOrEditingDate, 3) < Convertation(notes[j + 1].CreationOrEditingDate, 3))
                                {
                                    Basket swapper = notes[j];
                                    notes[j] = notes[j + 1];
                                    notes[j + 1] = swapper;
                                }
                                else if (Convertation(notes[j].CreationOrEditingDate, 3) == Convertation(notes[j + 1].CreationOrEditingDate, 3))
                                {
                                    if (Convertation(notes[j].CreationOrEditingDate, 4) < Convertation(notes[j + 1].CreationOrEditingDate, 4))
                                    {
                                        Basket swapper = notes[j];
                                        notes[j] = notes[j + 1];
                                        notes[j + 1] = swapper;
                                    }
                                    else if (Convertation(notes[j].CreationOrEditingDate, 4) == Convertation(notes[j + 1].CreationOrEditingDate, 4))
                                    {
                                        if (notes[j].Id < notes[j + 1].Id)
                                        {
                                            Basket swapper = notes[j];
                                            notes[j] = notes[j + 1];
                                            notes[j + 1] = swapper;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Notes = notes;
        }

        private int Convertation(string date, int index)
        {
            return Convert.ToInt32(date.Split(new char[] { '.', ' ', ':' })[index]);
        }

        private Basket GetElementById(int id)
        {
            IEnumerator<Basket> iterator = Notes.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (id == iterator.Current.Id)
                {
                    return iterator.Current;
                }
            }
            return null;
        }

        private Basket GetElementByIndex(int index)
        {
            IEnumerator<Basket> iterator = Notes.GetEnumerator();
            int counter = 0;
            while (iterator.MoveNext())
            {
                if (index == counter)
                {
                    return iterator.Current;
                }
                counter++;
            }
            return null;
        }

        private void StandardSteps()
        {
            Notes = dataBase.Table<Basket>();
            NumberOfNotes = dataBase.Table<Basket>().Count();
            BubbleSortForDates();
            RewriteDate();
            storageNotes = Notes;
        }
    }
}