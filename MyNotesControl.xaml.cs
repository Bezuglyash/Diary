using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Diary
{
    public partial class MyNotesControl : UserControl
    {
        private DiaryController myNotesController;

        public MyNotesControl(DiaryController myNotesController)
        {
            InitializeComponent();
            this.myNotesController = myNotesController;
            countNotesOfImportantDates.Text = myNotesController.GetStringAboutCountRecordsOfImportantDates();
            countGoals.Text = myNotesController.GetStringAboutCountGoals();
            countTasksForDay.Text = myNotesController.GetStringAboutCountTasks();
            countLoginAndPassword.Text = myNotesController.GetStringAboutCountLoginAndPin();
            countOther.Text = myNotesController.GetStringAboutCountOtherNotes();
        }
    }
}