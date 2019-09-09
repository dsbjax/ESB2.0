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
    /// Interaction logic for AddNewEquipmentSystemDialog.xaml
    /// </summary>
    public partial class AddNewEquipmentSystemDialog : Window
    {
        public delegate void NewSystemRequestEventHandler(object sender, NewSystemRequestEventArgs e);
        public event NewSystemRequestEventHandler NewSystemRequest;

        public AddNewEquipmentSystemDialog()
        {
            InitializeComponent();
            nomenclature.Focus();
        }

        private void AddNewSystemClicked(object sender, RoutedEventArgs e)
        {
            NewSystemRequest?.Invoke(this, new NewSystemRequestEventArgs(nomenclature.Text, description.Text));

            nomenclature.Clear();
            description.Clear();
            nomenclature.Focus();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            addNewSystem.IsEnabled =
                !string.IsNullOrEmpty(nomenclature.Text) &&
                !string.IsNullOrEmpty(description.Text);
        }
    }

    public class NewSystemRequestEventArgs : EventArgs
    {
        public string Nomenclature;
        public string Description;

        public NewSystemRequestEventArgs(string nomenclature, string description)
        {
            this.Nomenclature = nomenclature;
            this.Description = description;
        }
    }
}
