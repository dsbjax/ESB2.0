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
    /// Interaction logic for ExceptionErrorDialog.xaml
    /// </summary>
    public partial class ExceptionErrorDialog : Window
    {
        private string v;
        private Exception e;

        public ExceptionErrorDialog()
        {
            InitializeComponent();
        }

        public ExceptionErrorDialog(string callingFunction, Exception e)
        {
            this.callingFunction.Text = callingFunction;
            exceptionType.Text = e.GetType().Name;
            exceptionMessage.Text = e.Message;
            stackTrace.Text = e.StackTrace;
        }
    }
}
