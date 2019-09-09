using Database;
using System;
using System.Collections.Generic;
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
using WPFLibrary;

namespace StatusBoard
{
    /// <summary>
    /// Interaction logic for EquipmentOutagesStatusBoard.xaml
    /// </summary>
    public partial class EquipmentOutagesStatusBoard : UserControl, IRefreshData, IAppTimer
    {
        public EquipmentOutagesStatusBoard()
        {
            InitializeComponent();

            AppTimer.Subscribe(this, TimerInterval.Minutes);
            AppTimer.Subscribe(this, TimerInterval.NewDayLocal);
            RefreshData();
        }

        public void RefreshData()
        {
            outages.ItemsSource = DatabaseAccess.GetCurrentOutages();
            activeScheduledOutages.ItemsSource = DatabaseAccess.GetActiveScheduleOutages();
            todayScheduledOutages.ItemsSource = DatabaseAccess.GetTodayScheduledOutages();
            tomorrowScheduledOutages.ItemsSource = DatabaseAccess.GetTomorrowScheduledOutages();
            next7DaysScheduledOutages.ItemsSource = DatabaseAccess.GetNext7DaysScheduledOutages();
        }

        public void Tick(TimerInterval interval)
        {
            switch(interval)
            {
                case TimerInterval.Minutes:
                    activeScheduledOutages.ItemsSource = DatabaseAccess.GetActiveScheduleOutages();
                    todayScheduledOutages.ItemsSource = DatabaseAccess.GetTodayScheduledOutages();
                    break;

                case TimerInterval.NewDayLocal:
                    tomorrowScheduledOutages.ItemsSource = DatabaseAccess.GetTomorrowScheduledOutages();
                    next7DaysScheduledOutages.ItemsSource = DatabaseAccess.GetNext7DaysScheduledOutages();
                    break;
            }
        }
    }
    

    public class NoneHidden : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListViewHidden : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PastEndTimeSceduledOutage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || ((DateTime)value).CompareTo(DateTime.UtcNow) > 0 ? Brushes.Blue : Brushes.Red;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
