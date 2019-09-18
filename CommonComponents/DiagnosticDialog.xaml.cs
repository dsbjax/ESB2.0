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

namespace CommonComponents
{
    /// <summary>
    /// Interaction logic for DiagnosticDialog.xaml
    /// </summary>
    public partial class DiagnosticDialog : Window
    {
        public DiagnosticDialog(string message)
        {
            InitializeComponent();
            text.Text = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
