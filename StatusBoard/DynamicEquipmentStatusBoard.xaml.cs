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

namespace StatusBoard
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentStatusBoard.xaml
    /// </summary>
    public partial class DynamicEquipmentStatusBoard : UserControl, IAppTimer
    {
        private const int INTERVAL = 10;
        private int currentPage = 0;

        public DynamicEquipmentStatusBoard()
        {
            InitializeComponent();

            LoadActiveStatusPages();

            AppTimer.Subscribe(this, TimerInterval.Seconds);
        }

        private void LoadActiveStatusPages()
        {
            statusPanel.Children.Clear();
            foreach (var page in DatabaseAccess.GetActiveStatusPages())
                statusPanel.Children.Add(new StatusPageControl(page) { Visibility = Visibility.Collapsed });

            if (statusPanel.Children.Count > 0)
                ((StatusPageControl)statusPanel.Children[currentPage]).Visibility = Visibility.Visible;
        }

        public void Tick(TimerInterval interval)
        {
            if (statusPanel.Children.Count != DatabaseAccess.GetActiveStatusPages().Count())
            {
                currentPage = 0;
                LoadActiveStatusPages();
            }

            if(statusPanel.Children.Count > 0 && DateTime.Now.Second % INTERVAL == 0)
            {
                ((StatusPageControl)statusPanel.Children[currentPage]).Visibility = Visibility.Collapsed;

                currentPage = ++currentPage % statusPanel.Children.Count;
                
                var updatedPage =
                    new StatusPageControl(DatabaseAccess.UpdateStatusPage(((StatusPageControl)statusPanel.Children[currentPage]).StatusPageId));

                ((StatusPageControl)statusPanel.Children[currentPage]).Refresh(updatedPage.EquipmentGroups);
                ((StatusPageControl)statusPanel.Children[currentPage]).Visibility = Visibility.Visible;
            }
        }
    }
}
