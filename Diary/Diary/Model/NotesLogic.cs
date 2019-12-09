using Diary.DataBase;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using SQLite;
using System.Collections.Specialized;

namespace Diary.Model
{
    class NotesLogic : ViewModelBase
    {
        private IEnumerable<Note> notes;
        private SQLiteConnection dataBase;

        public NotesLogic(SQLiteConnection dataBase)
        {
            this.dataBase = dataBase;
            notes = this.dataBase.Table<Note>();
            NumberOfNotes = this.dataBase.Table<Note>().Count().ToString();
        }

        public IEnumerable<Note> GetNotes()
        {
            return notes;
        }

        public string NumberOfNotes { get; set; }

        public void AddNewNote(string content, string date)
        {
            Note note = new Note();
            note.NoteContent = content;
            note.NoteTitle = GetPreparedTitle(content);
            note.CreationOrEditingDate = date;
            dataBase.Insert(note);
            NumberOfNotes = dataBase.Table<Note>().Count().ToString();
        }

        public string GetPreparedTitle(string stringWhichItNeedUpgrade)
        {
            return stringWhichItNeedUpgrade.Split(new char[] { '\n' })[0].Replace("\r", "");
        }
    }
}
