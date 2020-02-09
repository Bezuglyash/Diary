using System.Windows;

namespace Diary.ViewModel
{
    class MessageBoxViewModel
    {
        string text;
        string caption;

        public MessageBoxViewModel(string text, string caption)
        {
            this.text = text;
            this.caption = caption;
        }

        public bool ShowMessage()
        {
            MessageBoxResult result = MessageBox.Show(text, caption, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ShowWarning()
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK);
        }
    }
}
