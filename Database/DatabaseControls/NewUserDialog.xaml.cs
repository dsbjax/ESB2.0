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
using System.Windows.Shapes;

namespace Database.DatabaseControls
{
    /// <summary>
    /// Interaction logic for NewUserDialog.xaml
    /// </summary>
    public partial class NewUserDialog : Window
    {
        public delegate void NewUserRequestEventHandler(object sender, NewUSerEventArgs e);
        public event NewUserRequestEventHandler NewUserRequest;

        public NewUserDialog()
        {
            InitializeComponent();
            username.Focus();
        }

        private void TextChangeded(object sender, TextChangedEventArgs e)
        {
            addUser.IsEnabled =
                username.Text.Length >= 5 &&
                !string.IsNullOrEmpty(firstname.Text) &&
                !string.IsNullOrEmpty(lastname.Text);
        }

        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            NewUserRequest?.Invoke(this, new NewUSerEventArgs(username.Text, firstname.Text, lastname.Text));

            username.Clear();
            firstname.Clear();
            lastname.Clear();
            username.Focus();

        }

        private void DoneClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class NewUSerEventArgs : EventArgs
    {
        public readonly string Username;
        public readonly string Firstname;
        public readonly string Lastname;


        public NewUSerEventArgs(string username, string firstname, string lastname)
        {
            Username = username;
            Firstname = firstname;
            Lastname = lastname;
        }
    }
}
