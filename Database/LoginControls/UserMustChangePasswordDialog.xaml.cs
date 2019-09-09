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
    /// Interaction logic for UserMustChangePasswordDialog.xaml
    /// </summary>
    public partial class UserMustChangePasswordDialog : Window
    {
        public string OldPassword { get { return oldPassword.Password; } }
        public string NewPassword { get { return password1.Password; } }

        public UserMustChangePasswordDialog()
        {
            InitializeComponent();
            oldPassword.Focus();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            changePassword.Visibility =
                oldPassword.Password.Length >= 8 &&
                password1.Password.Length >= 8 &&
                password2.Password.Equals(password1.Password) ?
                Visibility.Visible : Visibility.Collapsed;

            passwordMatch.Visibility =
                oldPassword.Password.Length >= 8 &&
                oldPassword.Password.Equals(password1.Password) ?
                Visibility.Visible : Visibility.Collapsed;

            passwordSize.Visibility =
                passwordMatch.Visibility == Visibility.Collapsed ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private void ChangePasswordClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
