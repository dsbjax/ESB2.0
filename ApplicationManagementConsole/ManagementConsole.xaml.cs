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

namespace ApplicationManagementConsole
{
    /// <summary>
    /// Interaction logic for ManagementConsole.xaml
    /// </summary>
    public partial class ManagementConsole : UserControl, ICurrentUserSubscriber
    {
        public ManagementConsole()
        {
            InitializeComponent();
            CurrentUser.Subscribe(this);
        }

        public void SetCurrentUser(CurrentUser newCurrentUser)
        {
            Visibility = newCurrentUser != null ? Visibility.Visible : Visibility.Collapsed;

            if (newCurrentUser != null)
            {
                reportsControl.Visibility =
                    newEquipmentOutageControl.Visibility =
                    userManagerControl.Visibility =
                    equipmentManagerControl.Visibility =
                    statusBoardManagerControl.Visibility =
                    databaseManagerControl.Visibility = Visibility.Collapsed;

                if (newCurrentUser.UserPermissions == UserPermissions.ReportViewer)
                {
                    menu.Visibility = Visibility.Collapsed;
                    reportsControl.Visibility = Visibility.Visible;
                }
                else
                {
                    menu.Visibility = Visibility.Visible;

                    userManagerButton.Visibility =
                        (newCurrentUser.UserPermissions & UserPermissions.UserManager) == UserPermissions.UserManager ||
                        (newCurrentUser.UserPermissions & UserPermissions.DatabaseAdmin) == UserPermissions.DatabaseAdmin ?
                        Visibility.Visible : Visibility.Collapsed;

                    newEquipmentOutageButton.Visibility =
                        (newCurrentUser.UserPermissions & UserPermissions.StatusUpdates) == UserPermissions.StatusUpdates ||
                        (newCurrentUser.UserPermissions & UserPermissions.DatabaseAdmin) == UserPermissions.DatabaseAdmin ?
                        Visibility.Visible : Visibility.Collapsed;

                    equipmentOutageUpdateButton.Visibility =
                        (newCurrentUser.UserPermissions & UserPermissions.StatusUpdates) == UserPermissions.StatusUpdates ||
                        (newCurrentUser.UserPermissions & UserPermissions.DatabaseAdmin) == UserPermissions.DatabaseAdmin ?
                        Visibility.Visible : Visibility.Collapsed;

                    equipmentManagerButton.Visibility =
                        (newCurrentUser.UserPermissions & UserPermissions.EquipmentManager) == UserPermissions.EquipmentManager ||
                        (newCurrentUser.UserPermissions & UserPermissions.DatabaseAdmin) == UserPermissions.DatabaseAdmin ?
                        Visibility.Visible : Visibility.Collapsed;

                    statusBoardLayoutButton.Visibility =
                        (newCurrentUser.UserPermissions & UserPermissions.StatusBoardManager) == UserPermissions.StatusBoardManager ||
                        (newCurrentUser.UserPermissions & UserPermissions.DatabaseAdmin) == UserPermissions.DatabaseAdmin ?
                        Visibility.Visible : Visibility.Collapsed;

                    databaseAdministratorButton.Visibility =
                        (newCurrentUser.UserPermissions & UserPermissions.DatabaseAdmin) == UserPermissions.DatabaseAdmin ?
                        Visibility.Visible : Visibility.Collapsed;

                }

            }
        }

        private void ControlSelectClick(object sender, RoutedEventArgs e)
        {
            reportsControl.Visibility =
                newEquipmentOutageControl.Visibility =
                userManagerControl.Visibility =
                equipmentManagerControl.Visibility =
                statusBoardManagerControl.Visibility =
                databaseManagerControl.Visibility =
                equipmentStatusUpdatesControl.Visibility = Visibility.Collapsed;

            ((UserControl)((Button)sender).Tag).Visibility = Visibility.Visible;

            if ((UserControl)((Button)sender).Tag is IRefreshData refreshData)
                refreshData.RefreshData();
        }
    }
}
