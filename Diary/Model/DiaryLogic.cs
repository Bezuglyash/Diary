using Diary.DataBase;
using GalaSoft.MvvmLight;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;

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

        public void AddName(string text)
        {
            user.Name = text;
        }

        public void AddPassword(int? pin)
        {
            user.Password = pin;
            user.IsHavePassword = 1;
            try
            {
                if (dataBase.Table<User>().Any())
                {
                    UpdateDataAsync();
                }
            }
            catch (Exception) { }
        }

        public void AddIsHavePassword(int number)
        {
            user.IsHavePassword = number;
            if (dataBase.Table<User>().Any())
            {
                UpdateDataAsync();
            }
        }

        public string GetName()
        {
            return user.Name;
        }

        public int? GetPassword()
        {
            return user.Password;
        }

        public bool IsHavePassword()
        {
            return user.IsHavePassword == 1;
        }

        public void RewriteName(string name)
        {
            user.Name = name;
            UpdateDataAsync();
        }

        public void UpdatePassword(int? pin, int number)
        {
            user.Password = pin;
            user.IsHavePassword = number;
            UpdateDataAsync();
        }

        public async void CreateDataBaseAndTables()
        {
            await Task.Run(() =>
            {
                dataBase = new SQLiteConnection(NAME_DATA_BASE, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, true);
                dataBase.CreateTable<User>();
                dataBase.CreateTable<Note>();
                dataBase.CreateTable<ImportantDate>();
                new ImportantDatesLogic.ImportantDatesLogic(dataBase);
                dataBase.CreateTable<TimetableForTheDay>();
                HabitsTrackerLogic.CreateXmlFile();
                dataBase.CreateTable<Goal>();
                dataBase.CreateTable<Basket>();
                AddDataAsync();
            });
        }

        public SQLiteConnection GetDataBase() { return dataBase; }

        private async void AddDataAsync()
        {
            await Task.Run(() => { dataBase.Insert(user); });
        }

        private async void UpdateDataAsync()
        {
            await Task.Run(() => { dataBase.Update(user); });
        }
    }
}