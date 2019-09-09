using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Database.DatabaseControls
{
    /// <summary>
    /// Interaction logic for EquipmentManagerControl.xaml
    /// </summary>
    public partial class EquipmentManagerControl : UserControl
    {
        public EquipmentManagerControl()
        {
            InitializeComponent();
            systemListView.ItemsSource = DatabaseAccess.GetEquipmentList();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            DatabaseAccess.RefreshSystemList();
            base.OnRender(drawingContext);
        }

        private void DeleteSystemClicked(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.DeleteEquipmentSystem((EquipmentSystem)systemListView.SelectedItem);
        }

        private void AddSystemClick(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.AddEquipmentSystem();
                
        }

        private void AddEquipmentGroupingClick(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.AddEquipmentGrouping((EquipmentSystem)systemListView.SelectedItem, systemListView);
        }

        private void DeleteEquipmentGroupingClicked(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.DeleteEquipmentGrouping((EquipmentGrouping)equipmentGroupsListView.SelectedItem, systemListView);
        }

        private void AddNewEquipmentClicked(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.AddNewEquipment((EquipmentGrouping)equipmentGroupsListView.SelectedItem, 
                systemListView, equipmentGroupsListView);
        }

        private void DeleteEquipmentClicked(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.DeleteEquipment((Equipment)equipmentListView.SelectedItem,
                equipmentGroupsListView, ((EquipmentGrouping)equipmentGroupsListView.SelectedItem).Id,
                systemListView, ((EquipmentSystem)systemListView.SelectedItem).Id);
        }

        private void EquipmentListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && equipmentListView.SelectedItem != null)
                DeleteEquipmentClicked(this, new RoutedEventArgs());
        }

        private void EquipmentGroupsListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && equipmentGroupsListView.SelectedItem != null)
                DeleteEquipmentGroupingClicked(this, new RoutedEventArgs());
        }

        private void SystemListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && systemListView.SelectedItem != null)
                DeleteSystemClicked(this, new RoutedEventArgs());
        }
    }
}
