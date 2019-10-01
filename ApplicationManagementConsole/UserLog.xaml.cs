using Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for UserLog.xaml
    /// </summary>
    public partial class UserLog : UserControl
    {
        private readonly ObservableCollection<UserLogEvent> userEvents = new ObservableCollection<UserLogEvent>();
        public UserLog()
        {
            InitializeComponent();

            byUser.ItemsSource = DatabaseAccess.GetUserList();
            byEvent.ItemsSource = DatabaseAccess.GetUserEvents();
            eventLog.ItemsSource = userEvents;

            FiltersChanged(null, null);

        }

        private void ResetFiltersClick(object sender, RoutedEventArgs e)
        {
            byUser.ItemsSource = DatabaseAccess.GetUserList();
            byEvent.ItemsSource = DatabaseAccess.GetUserEvents();
            start.SelectedDate = null;
            end.SelectedDate = null;
        }

        private void FiltersChanged(object sender, SelectionChangedEventArgs e)
        {
            userEvents.Clear();
            var log = DatabaseAccess.GetUserEventsLog();

            if (byUser.SelectedItem != null)
                log = log.Where(l => l.User.Id == ((User)byUser.SelectedItem).Id).ToList();

            if (byEvent.SelectedItem != null)
                log = log.Where(l => l.UserEvent == ((UserEvents)((ComboBoxItem)byEvent.SelectedItem).Tag)).ToList();

            if (start.SelectedDate.HasValue)
                log = log.Where(l => l.Timestamp.CompareTo(start.SelectedDate.Value) > 0).ToList();

            if (end.SelectedDate.HasValue)
                log = log.Where(l => l.Timestamp.CompareTo(end.SelectedDate.Value) < 0).ToList();

            foreach (var logEvent in log)
                userEvents.Add(logEvent);
        }
    }

    public class LogEventsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userEvent = null;

            switch((int)value)
            {
                case 0:
                    userEvent = "Account Created";
                    break;

                case 1:
                    userEvent = "Sucessful Login";
                    break;

                case 2:
                    userEvent = "User Logout";
                    break;

                case 3:
                    userEvent = "Failed Login";
                    break;

                case 4:
                    userEvent = "Account Locked";
                    break;

                case 5:
                    userEvent = "Account Unlocked";
                    break;

                case 6:
                    userEvent = "Account Disabled";
                    break;

                case 7:
                    userEvent = "Password Changed";
                    break;
            }

            return userEvent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
