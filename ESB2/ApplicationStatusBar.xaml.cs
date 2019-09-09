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
using WPFLibrary;

namespace ESB2
{
    /// <summary>
    /// Interaction logic for ApplicationStatusBar.xaml
    /// </summary>
    public partial class ApplicationStatusBar : UserControl, IAppTimer, ICurrentUserSubscriber
    {
        public ApplicationStatusBar()
        {
            InitializeComponent();

            AppTimer.Subscribe(this, TimerInterval.Minutes);
            CurrentUser.Subscribe(this);

            SetApplicationDateTime();
        }

        public void SetCurrentUser(CurrentUser newCurrentUser)
        {
            if(newCurrentUser != null)
            {
                username.Text = newCurrentUser.Fullname;
                loginPrompt.Visibility = Visibility.Collapsed;
                loginName.Visibility = Visibility.Visible;
            }
            else
            {
                loginPrompt.Visibility = Visibility.Visible;
                loginName.Visibility = Visibility.Collapsed;
            }
        }

        public void Tick(TimerInterval interval)
        {
            SetApplicationDateTime();
        }

        private void SetApplicationDateTime()
        {
            date.Text = DateTime.Now.ToLongDateString();
            utcTime.Text = DateTime.UtcNow.ToString("HHmm");
            localTime.Text = DateTime.Now.ToString("HHmm");
        }
    }
}
