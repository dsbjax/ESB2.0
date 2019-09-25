using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for DatabaseAdministrator.xaml
    /// </summary>
    public partial class DatabaseAdministrator : UserControl
    {
        public DatabaseAdministrator()
        {
            InitializeComponent();
        }

        private void DatabaseBackupClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(@"C:\KBR\ESB2\Backups"))
                Directory.CreateDirectory(@"C:\KBR\ESB2\Backups");

            try
            {
                File.Copy(@"C:\KBR\ESB2\esb2db.sdf", 
                    @"C:\KBR\ESB2\Backups\esb2db." + DateTime.Now.ToString("dd-MMM-yyyy HHmm." + "Backup"), true);

                new UserDialog("Database Backup", "Database Backup Success!").ShowDialog();

            }catch(Exception ex)
            {
                new UserDialog("Database Back Failed", ex.Message).ShowDialog();
            }
        }

        private void DatabaseArchiveClick(object sender, RoutedEventArgs e)
        {

        }

        private void DatabaseRestoreClick(object sender, RoutedEventArgs e)
        {
            var backfileDialog = new OpenFileDialog()
            {
                Title = "Select Backup File to Restore",
                InitialDirectory = @"C:\KBR\ESB2\Backup",
                Filter = "ESB2 Backup Files|*.Backup"
            };

            if((bool)backfileDialog.ShowDialog())
            {
                try
                {
                    File.Copy(backfileDialog.FileName, @"C:\KBR\ESB2\esb2db.sdf", true);
                    new UserDialog("Database Restore Complete", "You must restart the application to complete restore.").ShowDialog();
                    Application.Current.MainWindow.Close();
                }catch(Exception ex)
                {
                    new UserDialog("Database Restore Failed", ex.Message).ShowDialog();
                }
            }
        }
    }
}
