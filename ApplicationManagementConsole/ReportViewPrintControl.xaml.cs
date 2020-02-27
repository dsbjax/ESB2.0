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
    /// Interaction logic for ReportViewPrintControl.xaml
    /// </summary>
    public partial class ReportViewPrintControl : UserControl
    {
        public ReportViewPrintControl()
        {
            InitializeComponent();
        }

        internal void LoadReport(FlowDocument report)
        {
            previewArea.Document = report;
        }

        private void PrintReportClick(object sender, RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();

            if((bool)printDialog.ShowDialog())
            {
                FlowDocument flowDocument = (FlowDocument)previewArea.Document;
                flowDocument.PagePadding = new Thickness(50);

                IDocumentPaginatorSource idpSource = flowDocument;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "Status Report");

            }

        }
    }
}
