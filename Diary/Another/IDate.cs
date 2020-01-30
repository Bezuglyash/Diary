namespace Diary.Another
{
    interface IDate
    {
        string GetMonthNow();
        int GetNumberOfDaysInThisMonth(string month, int year);
        string GetNameDayOfWeek(int numberOfDay, string month, int year);
        string GetNumberOfMonth(string nameOfMonth);
        bool IsThisLeapYear(int year);
    }
}
