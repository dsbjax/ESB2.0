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

namespace Database.LoginControls
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public string Username { get { return username.Text; } }
        public string Password { get { return password.Password; } }

        public LoginDialog()
        {
            InitializeComponent();
            username.Focus();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableLoginButton();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableLoginButton();
        }

        private void EnableLoginButton()
        {
            login.Visibility =
                !string.IsNullOrEmpty(username.Text) && !string.IsNullOrEmpty(password.Password) ?
                Visibility.Visible : Visibility.Collapsed;
        }
    }
}
