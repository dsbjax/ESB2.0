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
using System.Windows.Shapes;

namespace ApplicationManagementConsole
{
    /// <summary>
    /// Interaction logic for CustomReportDatesDialog.xaml
    /// </summary>
    public partial class CustomReportDatesDialog : Window
    {
        public DateTime StartDate { get { return start.SelectedDate.Value; } }
        public DateTime EndDate { get { return end.SelectedDate.Value; } }

        public CustomReportDatesDialog()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = start.SelectedDate != null && end.SelectedDate != null && ((Button)sender).IsDefault;
            Close();
        }
    }
}
