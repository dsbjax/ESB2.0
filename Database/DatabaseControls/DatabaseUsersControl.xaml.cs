using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace Database.DatabaseControls
{
    /// <summary>
    /// Interaction logic for DatabaseUsersControl.xaml
    /// </summary>
    public partial class DatabaseUsersControl : UserControl
    {
        
        
        public DatabaseUsersControl()
        {
            InitializeComponent();
            userListView.ItemsSource = DatabaseAccess.GetUserList(); 
        }

        private void AddNewUserClick(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.NewUser();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DatabaseAccess.RefreshUserList();
        }

        private void ResetPasswordClick(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.ResetPassword((User) userListView.SelectedItem);
        }
    }

    public class NullObjectToBoolean : IValueConverter
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
}
