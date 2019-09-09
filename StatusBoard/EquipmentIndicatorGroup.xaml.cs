using Database;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StatusBoard
{
    /// <summary>
    /// Interaction logic for EquipmentIndicatorGroup.xaml
    /// </summary>
    public partial class EquipmentIndicatorGroup : UserControl
    {
        public static DependencyProperty EquipmentIndicatorGroupTitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(EquipmentIndicatorGroup));

        public static DependencyProperty EquipmentIndicatorGroupDescriptioProperty =
                    DependencyProperty.Register("Description", typeof(string), typeof(EquipmentIndicatorGroup));

        public static DependencyProperty EquipmentIndicatorGroupEquipmentProperty =
                    DependencyProperty.Register("Equipment", typeof(ICollection<Equipment>), typeof(EquipmentIndicatorGroup));

        public string Title
        {
            get { return (string) GetValue(EquipmentIndicatorGroupTitleProperty); }
            set { SetValue(EquipmentIndicatorGroupTitleProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(EquipmentIndicatorGroupDescriptioProperty); }
            set { SetValue(EquipmentIndicatorGroupDescriptioProperty, value); }
        }

        public ICollection<Equipment> Equipment
        {
            get { return (ICollection<Equipment>)GetValue(EquipmentIndicatorGroupEquipmentProperty); }
            set { SetValue(EquipmentIndicatorGroupEquipmentProperty, value); }
        }

        public EquipmentIndicatorGroup()
        {
            InitializeComponent();
        }
    }
}
