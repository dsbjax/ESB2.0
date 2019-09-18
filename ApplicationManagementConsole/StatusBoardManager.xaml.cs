using Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for StatusBoardManager.xaml
    /// </summary>
    public partial class StatusBoardManager : UserControl
    {
        
        public StatusBoardManager()
        {
            InitializeComponent();
            systemComboBox.ItemsSource = DatabaseAccess.GetEquipmentList();
            statusPages.ItemsSource = DatabaseAccess.GetAllStatusPages();
        }

        private void AvailableEquipmentMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if ( pageGroups.SelectedItem != null && statusPages.SelectedItem != null &&
                    !selectedEquipmentListView.Items.Contains(availableEquipmentListView.SelectedItem))
                {
                    int pageIndex = ((StatusPage)statusPages.SelectedItem).Id;
                    int groupIndex = ((StatusPageGrouping)pageGroups.SelectedItem).Id;

                    ((StatusPageGrouping)pageGroups.SelectedItem).Equipment.Add((Equipment)availableEquipmentListView.SelectedItem);
                    DatabaseAccess.GetAllStatusPages();

                    statusPages.SelectedItem = ((ObservableCollection<StatusPage>)statusPages.ItemsSource).First(p => p.Id == pageIndex);
                    pageGroups.SelectedItem = ((StatusPage)statusPages.SelectedItem).StatusPageGroupings.First(g => g.Id == groupIndex);
                }
            }

            pageGroups.SelectedItem = pageGroups.SelectedItem;
        }

        private void SelectedEquipmentMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                int pageIndex = ((StatusPage)statusPages.SelectedItem).Id;
                int groupIndex = ((StatusPageGrouping)pageGroups.SelectedItem).Id;

                ((StatusPageGrouping)pageGroups.SelectedItem).Equipment.Remove((Equipment)selectedEquipmentListView.SelectedItem);
                DatabaseAccess.GetAllStatusPages();

                statusPages.SelectedItem = ((ObservableCollection<StatusPage>)statusPages.ItemsSource).First(p => p.Id == pageIndex);
                pageGroups.SelectedItem = ((StatusPage)statusPages.SelectedItem).StatusPageGroupings.First(g => g.Id == groupIndex);
            }
        }

        private void NewPageClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddNewPageDialog();
            dialog.NewStatusPage += NewStatusPage;

            dialog.ShowDialog();
        }

        private void NewStatusPage(object sender, AddNewStatusPageEventArgs e)
        {
            var newPage = new StatusPage()
            {
                Title = e.Title,
                Description = e.Description,
                IsDisplayed = true,
                Index = DatabaseAccess.GetAllStatusPages().Where(p=> p.Index != -1).Count()
            };

            DatabaseAccess.AddNewStatusPage(newPage);
            statusPages.SelectedItem = ((ObservableCollection<StatusPage>)statusPages.ItemsSource).First(p=> p.Id == newPage.Id);
        }

        private void NewGroupClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddNewPageDialog("Add New Page Group", "Enter to add new group");
            dialog.NewStatusPage += NewStatusGroup;

            dialog.ShowDialog();
        }

        private void NewStatusGroup(object sender, AddNewStatusPageEventArgs e)
        {
            var newGroup = new StatusPageGrouping()
            {
                Title = e.Title,
                Description = e.Description,
                StatusPageId = ((StatusPage)statusPages.SelectedItem).Id,
                IsStatusBar = false,
                Index = ((StatusPage)statusPages.SelectedItem).StatusPageGroupings.Count
            };

            ((StatusPage)statusPages.SelectedItem).StatusPageGroupings.Add(newGroup);
            var page = statusPages.SelectedItem;
            statusPages.SelectedItem = null;

            DatabaseAccess.GetAllStatusPages();

            statusPages.SelectedItem = page;
            pageGroups.SelectedItem = newGroup;
        }

        private void GetBackgroundImageClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Load Background Image",
                Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.gif"
            };

            if (dialog.ShowDialog() == true)
                ((StatusPage)statusPages.SelectedItem).BackgroundImage = dialog.FileName;
        }

        private void StatusPagesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(statusPages.SelectedItem != null)
            {
                if (((StatusPage)statusPages.SelectedItem).IsDisplayed)
                {
                    showHidePage.Content = Resources["hidePage"];
                    showHidePage.ToolTip = "Hide Page";
                }
                else
                {
                    showHidePage.Content = Resources["showPage"];
                    showHidePage.ToolTip = "Show Page";
                }
            }
        }

        private void PageDownClick(object sender, RoutedEventArgs e)
        {
            ((StatusPage)statusPages.Items[statusPages.SelectedIndex]).Index++;
            ((StatusPage)statusPages.Items[statusPages.SelectedIndex + 1]).Index--;

            var selected = ((StatusPage)statusPages.SelectedItem);

            DatabaseAccess.GetAllStatusPages();
            statusPages.SelectedItem = selected;
        }

        private void PageUpClick(object sender, RoutedEventArgs e)
        {
            ((StatusPage)statusPages.Items[statusPages.SelectedIndex]).Index--;
            ((StatusPage)statusPages.Items[statusPages.SelectedIndex - 1]).Index++;

            var selected = ((StatusPage)statusPages.SelectedItem);

            DatabaseAccess.GetAllStatusPages();
            statusPages.SelectedItem = selected;
        }

        private void ShowHidePageClick(object sender, RoutedEventArgs e)
        {
            ((StatusPage)statusPages.SelectedItem).IsDisplayed = !((StatusPage)statusPages.SelectedItem).IsDisplayed;
            var selelected = statusPages.SelectedItem;

            DatabaseAccess.GetAllStatusPages();
            statusPages.SelectedItem = selelected;
        }

        private void DeletePageClick(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.DeleteStatusPage(((StatusPage)statusPages.SelectedItem));
        }

        private void DeleteGroupingClick(object sender, RoutedEventArgs e)
        {
            var page = statusPages.SelectedItem;

            DatabaseAccess.DeletePageGrouping(((StatusPageGrouping)pageGroups.SelectedItem));
            statusPages.SelectedItem = page;
        }

        private void GroupUpClick(object sender, RoutedEventArgs e)
        {
            ((StatusPageGrouping)pageGroups.SelectedItem).Index--;
            ((StatusPageGrouping)pageGroups.Items[pageGroups.SelectedIndex - 1]).Index++;

            var page = statusPages.SelectedItem;
            var group = pageGroups.SelectedItem;

            DatabaseAccess.GetAllStatusPages();

            statusPages.SelectedItem = page;
            pageGroups.SelectedItem = group;
        }

        private void GroupDownClick(object sender, RoutedEventArgs e)
        {
            ((StatusPageGrouping)pageGroups.SelectedItem).Index++;
            ((StatusPageGrouping)pageGroups.Items[pageGroups.SelectedIndex + 1]).Index--;

            var page = statusPages.SelectedItem;
            var group = pageGroups.SelectedItem;

            DatabaseAccess.GetAllStatusPages();

            statusPages.SelectedItem = page;
            pageGroups.SelectedItem = group;
        }
    }

    public class FalseIfNull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }

    public class ValidateBackgroundImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "None") ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PageUpArrowEnable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((StatusPage)value).Index > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PageDownArrowEnable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((StatusPage)value).Index != -1 && ((StatusPage)value).Index < DatabaseAccess.StatusPageCount();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusPageButtonsEnabled : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((StatusPage)value).Index != -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnableGroupUp : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !((StatusPageGrouping)value).IsStatusBar &&
                ((StatusPageGrouping)value).Index > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnableGroupDown : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !((StatusPageGrouping)value).IsStatusBar && 
                ((StatusPageGrouping)value).Index < DatabaseAccess.GroupCount((StatusPageGrouping)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupDeleteEnable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !((StatusPageGrouping)value).IsStatusBar;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
