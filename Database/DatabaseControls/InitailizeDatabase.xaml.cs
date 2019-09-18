using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Database.DatabaseControls
{
    /// <summary>
    /// Interaction logic for InitailizeDatabase.xaml
    /// </summary>
    public partial class InitailizeDatabase : Window
    {
        public string Username {  get { return username.Text; } }
        public string Firstname {  get { return firstname.Text; } }
        public string Lastname {  get { return lastname.Text; } }
        public string Password { get { return password1.Password; } }

        public InitailizeDatabase()
        {
            InitializeComponent();
        }

        private void VallidateForInitialize(object sender, TextChangedEventArgs e)
        {
            ValidateInitializeButton();
        }

        private void ValidateInitializeButton()
        {
            init.IsEnabled =
                !string.IsNullOrEmpty(username.Text) &&
                username.Text.Length >= 5 &&
                !string.IsNullOrEmpty(firstname.Text) &&
                !string.IsNullOrEmpty(lastname.Text) &&
                password1.Password.Length >= 8 &&
                password1.Password.Equals(password2.Password);
        }

        private void VallidateForInitialize(object sender, RoutedEventArgs e)
        {
            ValidateInitializeButton();
        }

        private void InitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
