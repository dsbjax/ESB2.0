using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace ApplicationManagementConsole
{
    /// <summary>
    /// Interaction logic for AddNewPageDialog.xaml
    /// </summary>
    public partial class AddNewPageDialog : Window
    {
        public delegate void AddNewStatusPageEventHandler(object sender, AddNewStatusPageEventArgs e);
        public event AddNewStatusPageEventHandler NewStatusPage;

        public AddNewPageDialog()
        {
            InitializeComponent();
            title.Focus();
        }

        public AddNewPageDialog(string mainTitle, string buttonText)
        {
            InitializeComponent();
            windowTitle.Text = mainTitle;
            addStatusPage.Content = buttonText;

            title.Focus();
        }

        private void AddPageClick(object sender, RoutedEventArgs e)
        {
            NewStatusPage?.Invoke(this, new AddNewStatusPageEventArgs(title.Text, description.Text));
            ClearTextBoxes();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();
            Close();
        }

        private void ClearTextBoxes()
        {
            title.Clear();
            description.Clear();
            title.Focus();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            addStatusPage.IsEnabled =
                !string.IsNullOrEmpty(title.Text) && !string.IsNullOrEmpty(description.Text);
        }
    }

    public class AddNewStatusPageEventArgs : EventArgs
    {
        public readonly string Title;
        public readonly string Description;

        public AddNewStatusPageEventArgs(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
