using Database;
using Reports;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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
