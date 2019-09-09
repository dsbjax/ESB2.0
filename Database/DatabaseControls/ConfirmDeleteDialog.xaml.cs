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
    /// Interaction logic for ConfirmDeleteDialog.xaml
    /// </summary>
    public partial class ConfirmDeleteDialog : Window
    {
        public ConfirmDeleteDialog()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = ((Button)sender).Content.ToString().Equals("Yes");
            Close();
        }
    }
}
