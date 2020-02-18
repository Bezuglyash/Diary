using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;
using System;
using System.Collections.Generic;

namespace Diary.Model
{
    class DiaryLogic : ViewModelBase
    {
        private const string NAME_DATA_BASE = "Diary.db";
        private SQLiteConnection dataBase;
        private User user;

        public DiaryLogic()
        {
            user = new User();
            try
            {
                ItWasOpen = true;
                dataBase = new SQLiteConnection(NAME_DATA_BASE, SQLiteOpenFlags.ReadWrite, true);
                user = dataBase.Get<User>(1);
            }
            catch (Exception)
            {
                ItWasOpen = false;
            }
        }

        public bool ItWasOpen { get; }

        public string NameUser
        {
            get { return user.Name; }
            set
            {
                user.Name = value;
            }
        }

        public int? UserPassword
        {
            get { return user.Password; }
            set
            {
                user.Password = value;
                SaveUser(1);
            }
        }

        public void CreateDataBaseAndTables()
        {
            dataBase = new SQLiteConnection(NAME_DATA_BASE, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, true);
            dataBase.CreateTable<User>();
            dataBase.CreateTable<Note>();
            dataBase.CreateTable<ImportantDate>();
            dataBase.CreateTable<TimetableForTheDay>();
            HabitsTrackerLogic.CreateXmlFile();
            dataBase.CreateTable<Goal>();
        }

        public void SaveUser(int isHavePassword = 0)
        {
            user.IsHavePassword = isHavePassword;
            dataBase.Insert(user);
        }

        public SQLiteConnection GetDataBase() { return dataBase; }
    }
}