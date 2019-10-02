using Reports;
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
            ReportSelectionClick(sender, e);
            reportViewer.LoadReport(MonthlyReport.GetReport(new PrintDialog()));
        }
    }
}
