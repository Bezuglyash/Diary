using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Diary
{
    public partial class MainWindow : Window
    {
        private DiaryController diaryController;
        public MainWindow()
        {
            InitializeComponent();
            diaryController = new DiaryController();
            this.contentControl.Content = new MyNotesControl(diaryController);
        }

        private void ClickOpenMenu(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.9;
        }

        private void MyNotesClick(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new MyNotesControl(diaryController);
            popupMenu.IsOpen = false;
            this.Opacity = 1;
        }

        private void AddNoteClick(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new AddNoteControl();
            popupMenu.IsOpen = false;
            this.Opacity = 1;
        }

        private void MyFavouritesClick(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new MyNotesControl(diaryController);
            popupMenu.IsOpen = false;
            this.Opacity = 1;
        }

        private void MouseLeaveEventPopup(object sender, RoutedEventArgs e)
        {
            popupMenu.IsOpen = false;
            this.Opacity = 1;
        }
    }
}