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

namespace Database.DatabaseControls
{
    /// <summary>
    /// Interaction logic for UserPermissionsControl.xaml
    /// </summary>
    public partial class UserPermissionsControl : UserControl
    {
        public static DependencyProperty UserPermissionsProperty =
            DependencyProperty.Register("UserPermissions", typeof(UserPermissions), typeof(UserPermissionsControl),
                new PropertyMetadata(UserPermissions.ReportViewer, new PropertyChangedCallback(UserPermissionsPropertyChanged)));

        private static void UserPermissionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UserPermissionsControl)d).UpdateUserPermission(e);
        }

        private void UpdateUserPermission(DependencyPropertyChangedEventArgs e)
        {
            updates.IsChecked = ((UserPermissions)e.NewValue & UserPermissions.StatusUpdates) == UserPermissions.StatusUpdates;
            users.IsChecked = ((UserPermissions)e.NewValue & UserPermissions.UserManager) == UserPermissions.UserManager;
            equipment.IsChecked = ((UserPermissions)e.NewValue & UserPermissions.EquipmentManager) == UserPermissions.EquipmentManager;
            statusBoard.IsChecked = ((UserPermissions)e.NewValue & UserPermissions.StatusBoardManager) == UserPermissions.StatusBoardManager;
            admin.IsChecked = ((UserPermissions)e.NewValue & UserPermissions.DatabaseAdmin) == UserPermissions.DatabaseAdmin;

        }

        public UserPermissions UserPermissions
        {
            get { return (UserPermissions)GetValue(UserPermissionsProperty); }

            set
            {
                SetValue(UserPermissionsProperty, value);

            }
        }

        public UserPermissionsControl()
        {
            InitializeComponent();
        }

        private void UserPermissionsClicked(object sender, RoutedEventArgs e)
        {
            UserPermissions temp = UserPermissions.ReportViewer;

            if ((bool)updates.IsChecked) temp |= UserPermissions.StatusUpdates;
            if ((bool)users.IsChecked) temp |= UserPermissions.UserManager;
            if ((bool)equipment.IsChecked) temp |= UserPermissions.EquipmentManager;
            if ((bool)statusBoard.IsChecked) temp |= UserPermissions.StatusBoardManager;
            if ((bool)admin.IsChecked) temp |= UserPermissions.DatabaseAdmin;

            UserPermissions = temp;
        }
    }
}
