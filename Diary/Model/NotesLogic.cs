using Diary.DataBase;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using SQLite;
using System.Text.RegularExpressions;
using System;
using Diary.Another;

namespace Diary.Model
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

    class NotesLogic : ViewModelBase
    {
        private SQLiteConnection dataBase;
        private IEnumerable<Note> storageNotes;

        public NotesLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            Notes = this.dataBase.Table<Note>();
            BubbleSortForDates();
            RewriteDate();
            storageNotes = Notes;
            NumberOfNotes = this.dataBase.Table<Note>().Count();
        }

        public IEnumerable<Note> Notes { get; set; }

        public int NumberOfNotes { get; set; }

        public void AddNewNote(string content, string date)
        {
            Note note = new Note();
            note.NoteContent = content;
            note.NoteTitle = GetPreparedTitle(content);
            note.CreationOrEditingDate = date;
            dataBase.Insert(note);
            Notes = dataBase.Table<Note>();
            NumberOfNotes = dataBase.Table<Note>().Count();
            BubbleSortForDates();
            RewriteDate();
            storageNotes = Notes;
        }

        public void UpdateData(int id, string text, string date)
        {
            Note note = GetElementById(id);
            note.NoteTitle = GetPreparedTitle(text);
            note.NoteContent = text;
            note.CreationOrEditingDate = date;
            dataBase.Update(note);
            Notes = dataBase.Table<Note>();
            BubbleSortForDates();
            RewriteDate();
            storageNotes = Notes;
        }

        public void DeleteNote(int id)
        {
            dataBase.Delete<Note>(GetElementById(id).Id);
            Notes = dataBase.Table<Note>();
            NumberOfNotes = dataBase.Table<Note>().Count();
            BubbleSortForDates();
            RewriteDate();
            storageNotes = Notes;
        }

        public string GetNoteContent(int id)
        {
            return GetElementById(id).NoteContent;
        }

        public string GetDateInternal(int id)
        {
            return GetElementById(id).CreationOrEditingDate;
        }

        public int GetIndexOfList(int id)
        {
            int index = 0;
            IEnumerator<Note> iterator = Notes.GetEnumerator();
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

        public void Searching (string txt)
        {
            Notes = storageNotes;
            int index = 0;
            List<Note> notes = new List<Note>();
            Note note;
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

        private void RewriteDate()
        {
            IEnumerator<Note> iterator = Notes.GetEnumerator();
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
                else
                {
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
                        Months month = (Months)Convert.ToInt32(allDate.Split(new char[] { '.', ' ' })[1]);
                        if (Convert.ToInt32(allDate.Split(new char[] { '.', ' ' })[0]) == day.GetNumberOfDaysInThisMonth(month + "", Convert.ToInt32(DateTime.Now.ToString("yyyy"))))
                        {
                            allDate = allDate.Remove(0, 5);
                            return "Вчера в" + allDate;
                        }
                    }
                    Months months = (Months)Convert.ToInt32(allDate.Split(new char[] { '.', ' ' })[1]);
                    if (allDate.Split(new char[] { '.', ' ' })[0][0] == '0')
                    {
                        return allDate.Split(new char[] { '.', ' ' })[0][1] + " " + months + " в " + allDate.Split(new char[] { ' ' })[1];
                    }
                    else
                    {
                        return allDate.Split(new char[] { '.', ' ' })[0] + " " + months + " в " + allDate.Split(new char[] { ' ' })[1];
                    }
                }
            }
            else
            {
                if (DateTime.Now.ToString("dd.MM") == "01.01" && (allDate.Split(new char[] { '.', ' ' })[0] + "." + allDate.Split(new char[] { '.', ' ' })[1] == "31.12"))
                {
                    allDate = allDate.Remove(0, 10);
                    return "Вчера в" + allDate;
                }
                return allDate.Split(new char[] { ' ' })[0] + " в " + allDate.Split(new char[] { ' ' })[1];
            }
        }

        private void BubbleSortForDates()
        {
            List<Note> notes = new List<Note>();
            IEnumerator<Note> iterator = Notes.GetEnumerator();
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
                        Note swapper = notes[j];
                        notes[j] = notes[j + 1];
                        notes[j + 1] = swapper;
                    }
                    else if (Convertation(notes[j].CreationOrEditingDate, 2) == Convertation(notes[j + 1].CreationOrEditingDate, 2))
                    {
                        if (Convertation(notes[j].CreationOrEditingDate, 1) < Convertation(notes[j + 1].CreationOrEditingDate, 1))
                        {
                            Note swapper = notes[j];
                            notes[j] = notes[j + 1];
                            notes[j + 1] = swapper;
                        }
                        else if (Convertation(notes[j].CreationOrEditingDate, 1) == Convertation(notes[j + 1].CreationOrEditingDate, 1))
                        {
                            if (Convertation(notes[j].CreationOrEditingDate, 0) < Convertation(notes[j + 1].CreationOrEditingDate, 0))
                            {
                                Note swapper = notes[j];
                                notes[j] = notes[j + 1];
                                notes[j + 1] = swapper;
                            }
                            else if (Convertation(notes[j].CreationOrEditingDate, 0) == Convertation(notes[j + 1].CreationOrEditingDate, 0))
                            {
                                if (Convertation(notes[j].CreationOrEditingDate, 3) < Convertation(notes[j + 1].CreationOrEditingDate, 3))
                                {
                                    Note swapper = notes[j];
                                    notes[j] = notes[j + 1];
                                    notes[j + 1] = swapper;
                                }
                                else if (Convertation(notes[j].CreationOrEditingDate, 3) == Convertation(notes[j + 1].CreationOrEditingDate, 3))
                                {
                                    if (Convertation(notes[j].CreationOrEditingDate, 4) < Convertation(notes[j + 1].CreationOrEditingDate, 4))
                                    {
                                        Note swapper = notes[j];
                                        notes[j] = notes[j + 1];
                                        notes[j + 1] = swapper;
                                    }
                                    else if (Convertation(notes[j].CreationOrEditingDate, 4) == Convertation(notes[j + 1].CreationOrEditingDate, 4))
                                    {
                                        if (notes[j].Id < notes[j + 1].Id)
                                        {
                                            Note swapper = notes[j];
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

        private Note GetElementById(int id)
        {
            IEnumerator<Note> iterator = Notes.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (id == iterator.Current.Id)
                {
                    return iterator.Current;
                }
            }
            return null;
        }

        private Note GetElementByIndex(int index)
        {
            IEnumerator<Note> iterator = Notes.GetEnumerator();
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

        private string GetPreparedTitle(string stringWhichItNeedUpgrade)
        {
            int index = 0;
            while (stringWhichItNeedUpgrade.Split(new char[] { '\n' })[index].Replace("\r", "") == "")
            {
                index++;
            }
            return stringWhichItNeedUpgrade.Split(new char[] { '\n' })[index].Replace("\r", "");
        }
    }
}