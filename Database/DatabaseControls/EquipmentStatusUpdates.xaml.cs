using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for EquipmentStatusUpdates.xaml
    /// </summary>
    public partial class EquipmentStatusUpdates : UserControl , IRefreshData
    {
        public EquipmentStatusUpdates()
        {
            InitializeComponent();
        }

        

        public void RefreshData()
        {
            outages.ItemsSource = DatabaseAccess.GetOutages();
            scheduledOutages.ItemsSource = DatabaseAccess.GetScheduledOutages();
        }

        private void OutageUpdateClick(object sender, RoutedEventArgs e)
        {
            var selected = (Outage)outages.SelectedItem;
            outages.ItemsSource = null;

            selected.Updates.Add(new OutageUpdate()
            {
                Timestamp = DateTime.Now,
                UpdateBy = CurrentUser.currentUser,
                Update = newOutageUpdate.Text,
            });
            selected.Completed = (bool)completed.IsChecked;

            if (selected.Completed)
                selected.End = DateTime.Now;

            foreach (var equipment in selected.Equipment)
                equipment.EquipmentStatus = (EquipmentStatus)newEquipmentStatus.SelectedIndex;

            newOutageUpdate.Clear();
            completed.IsChecked = false;

            RefreshData();
            if (!selected.Completed)
                outages.SelectedItem = selected;
        }

        private void StartDateTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(startDate.Text, "\\d{2}/\\d{2}/\\d{4}"))
                startDatePrompt.Foreground = Brushes.Black;
            else
                startDatePrompt.Foreground = Brushes.Red;

            ModifyEventEnable();
        }

        private void ModifyEventEnable()
        {
            modifyEvent.IsEnabled =

                startDatePrompt.Foreground == Brushes.Black &&
                startTimePrompt.Foreground == Brushes.Black &&
                endDatePrompt.Foreground == Brushes.Black &&
                endTimePrompt.Foreground == Brushes.Black;
        }

        private void StartTimeTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(startTime.Text, "\\d{4}") &&
                int.Parse(startTime.Text)/100 < 24 &&
                int.Parse(startTime.Text)%100 < 60)

                startTimePrompt.Foreground = Brushes.Black;
            else
                startTimePrompt.Foreground = Brushes.Red;

            ModifyEventEnable();
        }


        private void EndDateTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(endDate.Text, "\\d{2}/\\d{2}/\\d{4}"))
                endDatePrompt.Foreground = Brushes.Black;
            else
                endDatePrompt.Foreground = Brushes.Red;

            ModifyEventEnable();
        }

        private void EndTimeTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(endTime.Text, "\\d{4}") &&
                    int.Parse(endTime.Text) / 100 < 24 &&
                    int.Parse(endTime.Text) % 100 < 60)

                endTimePrompt.Foreground = Brushes.Black;
            else
                endTimePrompt.Foreground = Brushes.Red;

            ModifyEventEnable();
        }

        private void CancelEventClick(object sender, RoutedEventArgs e)
        {
            ((Outage)scheduledOutages.SelectedItem).Canceled = true;
            ((Outage)scheduledOutages.SelectedItem).Updates.Add(new OutageUpdate()
            {
                Timestamp = DateTime.Now,
                UpdateBy = CurrentUser.currentUser,
                Update = "Event Canceled"
            });

            ESB2db.Save();
            RefreshData();
        }

        private void EventCompleteClick(object sender, RoutedEventArgs e)
        {
            ((Outage)scheduledOutages.SelectedItem).Completed = true;
            foreach (var equipment in ((Outage)scheduledOutages.SelectedItem).Equipment)
                equipment.EquipmentStatus = EquipmentStatus.Operational;

            ((Outage)scheduledOutages.SelectedItem).Updates.Add(new OutageUpdate()
            {
                Timestamp = DateTime.Now,
                UpdateBy = CurrentUser.currentUser,
                Update = "Event Completed"
            });

            ESB2db.Save();
            RefreshData();
        }

        private void ModifyEventClick(object sender, RoutedEventArgs e)
        {
            var outage = (Outage)scheduledOutages.SelectedItem;

            var startMonth = int.Parse(startDate.Text.Substring(0, 2));
            var startDay = int.Parse(startDate.Text.Substring(3, 2));
            var startYear = int.Parse(startDate.Text.Substring(6, 4));
            var startHour = int.Parse(startTime.Text.Substring(0, 2));
            var startMin = int.Parse(startTime.Text.Substring(2, 2));

            var endMonth = int.Parse(endDate.Text.Substring(0, 2));
            var endDay = int.Parse(endDate.Text.Substring(3, 2));
            var endYear = int.Parse(endDate.Text.Substring(6, 4));
            var endHour = int.Parse(endTime.Text.Substring(0, 2));
            var endMin = int.Parse(endTime.Text.Substring(2, 2));

            outage.Start = new DateTime(startYear, startMonth, startDay, startHour, startMin, 0, DateTimeKind.Utc);
            outage.End = new DateTime(endYear, endMonth, endDay, endHour, endMin, 0, DateTimeKind.Utc);

            outage.Updates.Add(new OutageUpdate()
            {
                Timestamp = DateTime.Now,
                UpdateBy = CurrentUser.currentUser,
                Update = "Event Modified."
            });

            ESB2db.Save();
            RefreshData();

        }

        private void Outages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(outages.SelectedItem != null)
                newEquipmentStatus.SelectedIndex = (int) ((Outage)outages.SelectedItem).Equipment.First().EquipmentStatus;
        }
    }

    public class DisableEndTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility) (value == null ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CreatedByUsername : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = (User)value;

            return user.Lastname + ", " + user.Firstname;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EmptyStringToFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CancelEventDisable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            var scheduledOutage = (Outage)value;

            return scheduledOutage.Start.CompareTo(DateTime.UtcNow) > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EventCompleteDisable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            var scheduledOutage = (Outage)value;

            return scheduledOutage.Start.CompareTo(DateTime.UtcNow) < 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var dateTime = (DateTime)value;

            return dateTime.ToString("MM/dd/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TimeToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var dateTime = (DateTime)value;

            return dateTime.ToString("HHmm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class NewEquipmentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var current  = ((IEnumerable<Equipment>)value).First().EquipmentStatus;

            return (int)current;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
