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
    /// Interaction logic for StatusUpdatesControl.xaml
    /// </summary>
    public partial class StatusUpdatesControl : UserControl
    {
        private ESB2DatabaseContainer database = ESB2db.GetDatabase();

        public StatusUpdatesControl()
        {
            InitializeComponent();
            systemComboBox.ItemsSource = DatabaseAccess.GetEquipmentList();
        }

        private void AvailableEquipmentMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if(!selectedEquipmentListView.Items.Contains(availableEquipmentListView.SelectedItem))
                    selectedEquipmentListView.Items.Add(availableEquipmentListView.SelectedItem);
            }

            SaveUpdateEnabled();
        }

        private void SelectedEquipmentMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                selectedEquipmentListView.Items.Remove(selectedEquipmentListView.SelectedItem);
            }

            SaveUpdateEnabled();
        }

        private void ReoccuringStartDateChanged(object sender, TextChangedEventArgs e)
        {
            var match = Regex.Match(reoccuranceStartDate.Text, "(?<Month>\\d{2})/(?<Day>\\d{2})/(?<Year>\\d{4})");

            reoccurandeStartDatePrompt.Foreground =
                match.Success &&
                int.Parse(match.Groups["Month"].Value) <= 12 &&
                int.Parse(match.Groups["Month"].Value) > 0 &&
                int.Parse(match.Groups["Day"].Value) <= 31 &&
                int.Parse(match.Groups["Day"].Value) > 0 ?
                Brushes.Black : Brushes.Red;

            SaveUpdateEnabled();
        }

        private void ReoccuringEndDateChanged(object sender, TextChangedEventArgs e)
        {
            var match = Regex.Match(reoccuranceEndDate.Text, "(?<Month>\\d{2})/(?<Day>\\d{2})/(?<Year>\\d{4})");

            reoccurandeEndDatePrompt.Foreground =
                match.Success &&
                int.Parse(match.Groups["Month"].Value) <= 12 &&
                int.Parse(match.Groups["Month"].Value) > 0 &&
                int.Parse(match.Groups["Day"].Value) <= 31 &&
                int.Parse(match.Groups["Day"].Value) > 0 ?
                Brushes.Black : Brushes.Red;

            SaveUpdateEnabled();
        }

        private void StartDateChanged(object sender, TextChangedEventArgs e)
        {
            var match = Regex.Match(startDate.Text, "(?<Month>\\d{2})/(?<Day>\\d{2})/(?<Year>\\d{4})");

            startDatePrompt.Foreground =
                match.Success &&
                int.Parse(match.Groups["Month"].Value) <= 12 && 
                int.Parse(match.Groups["Month"].Value) > 0 &&
                int.Parse(match.Groups["Day"].Value) <= 31 &&
                int.Parse(match.Groups["Day"].Value) > 0 ?
                Brushes.Black : Brushes.Red;

            SaveUpdateEnabled();
        }

        private void EndDateChanged(object sender, TextChangedEventArgs e)
        {
            var match = Regex.Match(endDate.Text, "(?<Month>\\d{2})/(?<Day>\\d{2})/(?<Year>\\d{4})");

            endDatePrompt.Foreground =
                match.Success &&
                int.Parse(match.Groups["Month"].Value) <= 12 &&
                int.Parse(match.Groups["Month"].Value) > 0 &&
                int.Parse(match.Groups["Day"].Value) <= 31 &&
                int.Parse(match.Groups["Day"].Value) > 0 ?
                Brushes.Black : Brushes.Red;

            SaveUpdateEnabled();
        }

        private void StartTimeTextChanged(object sender, TextChangedEventArgs e)
        {
            var match = Regex.Match(startTime.Text, "(?<Hour>\\d{2})(?<Min>\\d{2})");

            startTimePrompt.Foreground =
                match.Success &&
                int.Parse(match.Groups["Hour"].Value) < 24 &&
                int.Parse(match.Groups["Hour"].Value) >= 0 &&
                int.Parse(match.Groups["Min"].Value) < 60 &&
                int.Parse(match.Groups["Min"].Value) >= 0 ?
                Brushes.Black : Brushes.Red;

            SaveUpdateEnabled();
        }

        private void EndTimeTextChanged(object sender, TextChangedEventArgs e)
        {
            var match = Regex.Match(endTime.Text, "(?<Hour>\\d{2})(?<Min>\\d{2})");

            endTimePrompt.Foreground =
                match.Success &&
                int.Parse(match.Groups["Hour"].Value) < 24 &&
                int.Parse(match.Groups["Hour"].Value) >= 0 &&
                int.Parse(match.Groups["Min"].Value) < 60 &&
                int.Parse(match.Groups["Min"].Value) >= 0 ?
                Brushes.Black : Brushes.Red;

            SaveUpdateEnabled();
        }

        private void StartDateLostFocus(object sender, RoutedEventArgs e)
        {
            if (outageType.SelectedIndex == 0)
                endDate.Text = startDate.Text;
        }

        private void OutageTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timePrompt.Visibility = outageType.SelectedIndex == -1 ? Visibility.Collapsed : Visibility.Visible;
            local.Visibility = outageType.SelectedIndex == 0 ? Visibility.Collapsed : Visibility.Visible;
            utc.Visibility = outageType.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;

            SaveUpdateEnabled();
        }

        private void SaveUpdateEnabled()
        {
            saveUpdateButton.IsEnabled =
                outageType.SelectedIndex != -1 &&
                !string.IsNullOrEmpty(startDate.Text) &&
                !string.IsNullOrEmpty(startTime.Text) &&
                startDatePrompt.Foreground == Brushes.Black &&
                startTimePrompt.Foreground == Brushes.Black &&
                (outageType.SelectedIndex == 1 ? true :
                !string.IsNullOrEmpty(endDate.Text) &&
                !string.IsNullOrEmpty(endTime.Text) &&
                endDatePrompt.Foreground == Brushes.Black &&
                endTimePrompt.Foreground == Brushes.Black) &&
                selectedEquipmentListView.Items.Count > 0 &&
                (!(bool)reoccuring.IsChecked ? true :
                (bool)dailyPeriod.IsChecked || (bool)weeklyPeriod.IsChecked &&
                !string.IsNullOrEmpty(reoccuranceStartDate.Text) &&
                reoccurandeStartDatePrompt.Foreground == Brushes.Black &&
                !string.IsNullOrEmpty(reoccuranceEndDate.Text) &&
                reoccurandeEndDatePrompt.Foreground == Brushes.Black);
        }

        private void SaveUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            var newOutage =
                new Outage()
                {
                    Title = title.Text,
                    Description = description.Text,
                    OutageType = (outageType.SelectedIndex == 0) ? OutageType.ScheduledMaintenance : OutageType.CorrectiveMaintenance,
                    Start = CreateTimestamp(startDate.Text, startTime.Text),
                    Completed = false,
                    Canceled = false,
                    CreatedBy = database.Users.First(u=> u.Id == CurrentUser.DatabaseUser.Id)
                };

            if (newOutage.OutageType == OutageType.ScheduledMaintenance)
            {
                newOutage.End = CreateTimestamp(endDate.Text, endTime.Text);
                newOutage.Updates.Add(new OutageUpdate()
                {
                    Timestamp = DateTime.Now,
                    UpdateBy = newOutage.CreatedBy,
                    Update = "Initial Coordination."
                });
            }
            else
                newOutage.Updates.Add(new OutageUpdate()
                {
                    Timestamp = DateTime.Now,
                    UpdateBy = newOutage.CreatedBy,
                    Update = newOutage.Description
                });

            foreach (var selectedEquipment in selectedEquipmentListView.Items)
            {
                if (newOutage.OutageType == OutageType.CorrectiveMaintenance)
                    ((Equipment)selectedEquipment).EquipmentStatus = SelectedStatus();

                newOutage.Equipment.Add((Equipment)selectedEquipment);
            }
            

            database.Outages.Add(newOutage);

            if((bool)reoccuring.IsChecked)
            {
                var reoccuringEndDate = CreateTimestamp(reoccuranceEndDate.Text, "2359");
                while(reoccuringEndDate.CompareTo(newOutage.Start.AddDays((bool)dailyPeriod.IsChecked ? 1 : 7)) > 0)
                {
                    newOutage = new Outage()
                    {
                        Title = newOutage.Title,
                        Description = newOutage.Description,
                        OutageType = newOutage.OutageType,
                        Start = newOutage.Start.AddDays((bool)dailyPeriod.IsChecked ? 1 : 7),
                        End = newOutage.End.Value.AddDays((bool)dailyPeriod.IsChecked ? 1 : 7),
                        Completed = false,
                        Canceled = false,
                        CreatedBy = newOutage.CreatedBy
                    };

                    newOutage.Updates.Add(new OutageUpdate()
                    {
                        Timestamp = DateTime.Now,
                        UpdateBy = newOutage.CreatedBy,
                        Update = "Initial Coordination."
                    });

                    foreach (var selectedEquipment in selectedEquipmentListView.Items)
                        newOutage.Equipment.Add((Equipment)selectedEquipment);

                    database.Outages.Add(newOutage);
                }
            }
            Clear();
        }

        private EquipmentStatus SelectedStatus()
        {
            EquipmentStatus status = EquipmentStatus.Operational;

            switch(equipmentStatus.SelectedIndex)
            {
                case 1:
                    status = EquipmentStatus.Degraded;
                    break;

                case 2:
                    status = EquipmentStatus.Down;
                    break;

                case 3:
                    status = EquipmentStatus.OffLine;
                    break;
            }

            return status;

        }

        private void Clear()
        {
            outageType.SelectedIndex = -1;

            title.Text =
                description.Text =
                startDate.Text =
                startTime.Text =
                endDate.Text =
                endTime.Text = 
                reoccuranceStartDate.Text =
                reoccuranceEndDate.Text = "";

            startDatePrompt.Foreground =
                startTimePrompt.Foreground =
                endDatePrompt.Foreground =
                endTimePrompt.Foreground = 
                reoccurandeEndDatePrompt.Foreground =
                reoccurandeStartDatePrompt.Foreground = Brushes.Black;

            selectedEquipmentListView.Items.Clear();

            systemComboBox.SelectedIndex = -1;

            dailyPeriod.IsChecked =
                weeklyPeriod.IsChecked = false;

            reoccuring.IsChecked = false;
        }

        private DateTime CreateTimestamp(string date, string time)
        {
            var dateArray = date.Split('/');

            return new DateTime(int.Parse(dateArray[2]), int.Parse(dateArray[0]), int.Parse(dateArray[1]),
                int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(2, 2)), 0);
        }

        private void ReoccuringClick(object sender, RoutedEventArgs e)
        {
            if ((bool)reoccuring.IsChecked)
                reoccuranceStartDate.Text = startDate.Text;
            else
            {
                dailyPeriod.IsChecked =
                    weeklyPeriod.IsChecked = false;

                reoccuranceStartDate.Text =
                    reoccuranceEndDate.Text = "";

                reoccurandeEndDatePrompt.Foreground =
                    reoccurandeStartDatePrompt.Foreground = Brushes.Black;
            }
        }
    }

    public class DisableEndDateTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HideEquipmentStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 1 ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
