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
    /// Interaction logic for UserAccountStatusControl.xaml
    /// </summary>
    public partial class UserAccountStatusControl : UserControl
    {
        public static DependencyProperty AccountStatusProperty =
            DependencyProperty.Register("AccountStatus", typeof(AccountStatus), typeof(UserAccountStatusControl),
                new PropertyMetadata(AccountStatus.AccountValid, new PropertyChangedCallback(AccountStatusPropertyChanged)));

        private static void AccountStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UserAccountStatusControl)d).AccoutStatusChanged(e);
        }

        private void AccoutStatusChanged(DependencyPropertyChangedEventArgs e)
        {
            accountLocked.IsChecked = ((AccountStatus)e.NewValue & AccountStatus.AccountLocked) == AccountStatus.AccountLocked;
            accountDisabled.IsChecked = ((AccountStatus)e.NewValue & AccountStatus.AccountDisabled) == AccountStatus.AccountDisabled;
            userMustChangePassword.IsChecked = ((AccountStatus)e.NewValue & AccountStatus.UserMustChangePassword) == AccountStatus.UserMustChangePassword;

        }

        public AccountStatus AccountStatus
        {
            get { return (AccountStatus) GetValue(AccountStatusProperty); }

            set
            {
                SetValue(AccountStatusProperty, value);

            }
        }

        public UserAccountStatusControl()
        {
            InitializeComponent();
        }

        private void StatusBoxClicked(object sender, RoutedEventArgs e)
        {
            AccountStatus tempStatus = AccountStatus.AccountValid;

            if ((bool)accountDisabled.IsChecked)
                tempStatus |= AccountStatus.AccountDisabled;

            if ((bool)accountLocked.IsChecked)
                tempStatus |= AccountStatus.AccountLocked;

            if ((bool)userMustChangePassword.IsChecked)
                tempStatus |= AccountStatus.UserMustChangePassword;

            AccountStatus = tempStatus;
        }

        private void UserMustChangePassword_Checked(object sender, RoutedEventArgs e)
        {
            AccountStatus tempStatus = AccountStatus.AccountValid;

            if ((bool)accountDisabled.IsChecked)
                tempStatus |= AccountStatus.AccountDisabled;

            if ((bool)accountLocked.IsChecked)
                tempStatus |= AccountStatus.AccountLocked;

            if ((bool)userMustChangePassword.IsChecked)
                tempStatus |= AccountStatus.UserMustChangePassword;

            AccountStatus = tempStatus;
        }
    }
}
