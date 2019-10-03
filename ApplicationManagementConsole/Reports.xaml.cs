using Reports;
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

namespace ApplicationManagementConsole
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : UserControl
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void ReportSelectionClick(object sender, RoutedEventArgs e)
        {
            HideAllReports();

            ((UIElement)((Button)sender).Tag).Visibility = Visibility.Visible;
        }

        private void HideAllReports()
        {
            userLog.Visibility = Visibility.Collapsed;
            reportViewer.Visibility = Visibility.Collapsed;
        }

        private void MonthlyReportClick(object sender, RoutedEventArgs e)
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var start = new DateTime(lastMonth.Year, lastMonth.Month, 1);
            var end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var header = "Monthly Report for " + lastMonth.ToString("MMMM yyyy", CultureInfo.InvariantCulture);

            ReportSelectionClick(sender, e);
            reportViewer.LoadReport(MonthlyReports.GetReport(header, start, end, true));
        }

        private void CustomReportClick(object sender, RoutedEventArgs e)
        {
            var reportDates = new CustomReportDatesDialog();

            if((bool)reportDates.ShowDialog())
            {
                var header = "Custom Report " + reportDates.StartDate.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture) +
                    " to " + reportDates.EndDate.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture);

                ReportSelectionClick(sender, e);
                reportViewer.LoadReport(MonthlyReports.GetReport(header, reportDates.StartDate, reportDates.EndDate, false));
            }
        }
    }
}
