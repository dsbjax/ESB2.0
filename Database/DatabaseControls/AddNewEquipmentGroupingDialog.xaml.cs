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
    /// Interaction logic for AddNewEquipmentGroupingDialog.xaml
    /// </summary>
    public partial class AddNewEquipmentGroupingDialog : Window
    {
        public delegate void NewEquipmentGroupRequestEventHandler(object sender, NewEquipmentGroupRequestEventArgs e);
        public event NewEquipmentGroupRequestEventHandler NewEquipmentGroupRequest;
        private EquipmentSystem system;

        public AddNewEquipmentGroupingDialog(EquipmentSystem selectedItem)
        {
            InitializeComponent();
            system = selectedItem;
            title.Focus();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            addNewGroup.IsEnabled =
                !string.IsNullOrEmpty(title.Text) &&
                !string.IsNullOrEmpty(description.Text);
        }

        private void AddNewGroupClick(object sender, RoutedEventArgs e)
        {
            NewEquipmentGroupRequest?.Invoke(this, new NewEquipmentGroupRequestEventArgs(title.Text, description.Text, system));

            title.Clear();
            description.Clear();
            title.Focus();
        }
    }

    public class NewEquipmentGroupRequestEventArgs
    {
        public readonly string Title;
        public readonly string Description;
        public readonly EquipmentSystem EquipmentSystem;

        public NewEquipmentGroupRequestEventArgs(string title, string description, EquipmentSystem system)
        {
            Title = title;
            Description = description;
            EquipmentSystem = system;
        }
    }
}
