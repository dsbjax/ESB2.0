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
    /// Interaction logic for AddNewEquipmentDialog.xaml
    /// </summary>
    public partial class AddNewEquipmentDialog : Window
    {
        public delegate void NewEquipmentRequestEventHandler(object sender, NewEquipmentRequestEventArgs e);
        public event NewEquipmentRequestEventHandler NewEquipmentRequest;

        private ListView systemListView;
        private ListView equipmentGroupsListView;
        private EquipmentGrouping equipmentGrouping;

        public AddNewEquipmentDialog(EquipmentGrouping equipmentGrouping, ListView systemListView, ListView equipmentGroupsListView)
        {
            InitializeComponent();

            this.equipmentGrouping = equipmentGrouping;
            this.systemListView = systemListView;
            this.equipmentGroupsListView = equipmentGroupsListView;

            nomenclature.Focus();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            addNewEquipment.IsEnabled =
                !string.IsNullOrEmpty(nomenclature.Text) &&
                !string.IsNullOrEmpty(description.Text);
        }

        private void AddNewEquipmentClicked(object sender, RoutedEventArgs e)
        {
            NewEquipmentRequest?.Invoke(this, new NewEquipmentRequestEventArgs(nomenclature.Text, description.Text,
                equipmentGrouping, systemListView, equipmentGroupsListView));

            nomenclature.Clear();
            description.Clear();
            nomenclature.Focus();
        }
    }

    public class NewEquipmentRequestEventArgs
    {
        public string Nomenclature;
        public string Description;
        public ListView SystemListView;
        public ListView EquipmentGroupsListView;
        public EquipmentGrouping EquipmentGrouping;

        public NewEquipmentRequestEventArgs(string nomenclature, string description, 
            EquipmentGrouping equipmentGrouping, ListView systemListView, ListView equipmentGroupsListView)
        {
            Nomenclature = nomenclature;
            Description = description;
            SystemListView = systemListView;
            EquipmentGroupsListView = equipmentGroupsListView;
            EquipmentGrouping = equipmentGrouping;
        }
    }
}
