using Database;
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

namespace StatusBoard
{
    /// <summary>
    /// Interaction logic for MainStatusBoard.xaml
    /// </summary>
    public partial class MainStatusBoard : UserControl, ICurrentUserSubscriber
    {
        public MainStatusBoard()
        {
            InitializeComponent();
            CurrentUser.Subscribe(this);
        }

        public void SetCurrentUser(CurrentUser newCurrentUser)
        {
            Visibility = newCurrentUser == null ? Visibility.Visible : Visibility.Collapsed;
            outages.RefreshData();
            statusBar.RefreshData();
        }
    }
}
