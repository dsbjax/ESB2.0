using Database;
using Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ESB2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ESB2db.InitializeDatabase();
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.F2:
                    DatabaseAccess.Login();
                    break;

                case Key.Escape:
                    DatabaseAccess.Logout();
                    break;

                case Key.F4:
                    StatusReport.PrintReport();
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ESB2db.Save();
            base.OnClosing(e);
        }
    }
}
