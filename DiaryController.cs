using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary
{
    public class DiaryController
    {
        private int countRecordsOfImportantDates;
        private int countGoals;
        private int countTasks;
        private int countLoginAndPin;
        private int countOtherNotes;

        public DiaryController()
        {
            countRecordsOfImportantDates = 9;
            countGoals = 0;
            countTasks = 0;
            countLoginAndPin = 0;
            countOtherNotes = 0;
        }

        public string GetStringAboutCountRecordsOfImportantDates()
        {
            return "Количество записей: " + countRecordsOfImportantDates;
        }

        public string GetStringAboutCountGoals()
        {
            return "Количество записей: " + countGoals;
        }

        public string GetStringAboutCountTasks()
        {
            return "Количество записей: " + countTasks;
        }

        public string GetStringAboutCountLoginAndPin()
        {
            return "Количество записей: " + countLoginAndPin;
        }

        public string GetStringAboutCountOtherNotes()
        {
            return "Количество записей: " + countOtherNotes;
        }
    }
}