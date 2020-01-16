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
        private IEnumerable<ImportantDate> importantDates;

        public DiaryLogic()
        {
            user = new User();
            try
            {
                ItWasOpen = true;
                dataBase = new SQLiteConnection(NAME_DATA_BASE, SQLiteOpenFlags.ReadWrite, true);
                user = dataBase.Get<User>(1);
                importantDates = dataBase.Table<ImportantDate>();
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
                RaisePropertyChanged(() => NameUser);
            }
        }

        public int? UserPassword
        {
            get { return user.Password; }
            set
            {
                user.Password = value;
                SaveUser(1);
                RaisePropertyChanged(() => UserPassword);
            }
        }

        public void CreateDataBaseAndTables()
        {
            dataBase = new SQLiteConnection(NAME_DATA_BASE, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, true);
            dataBase.CreateTable<User>();
            dataBase.CreateTable<Note>();
            dataBase.CreateTable<ImportantDate>();
        }

        public void SaveUser(int isHavePassword = 0)
        {
            user.IsHavePassword = isHavePassword;
            dataBase.Insert(user);
        }

        public SQLiteConnection GetDataBase() { return dataBase; }
    }
}