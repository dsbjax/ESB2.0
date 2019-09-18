using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Database.DatabaseControls;
using System.IO;
using CommonComponents;
using System.Security.AccessControl;

namespace Database
{
    public static class ESB2db
    {
        private static readonly string dbDIR = @"C:\KBR\ESB2";

        private static ESB2DatabaseContainer database;
        internal static ESB2DatabaseContainer GetDatabase()
        {
            if (database == null)
                database = new ESB2DatabaseContainer();

            return database;
        }

        public static void Save()
        {
            if (database.ChangeTracker.HasChanges())
                database.SaveChanges();
        }

        #region Database Initialazation
        public static void InitializeDatabase()
        {
            if (!Directory.Exists(dbDIR))
            {
                var dirInfo = Directory.CreateDirectory(dbDIR);
                var dirSec = dirInfo.GetAccessControl();

                dirSec.AddAccessRule(new FileSystemAccessRule(
                    "Users", 
                    FileSystemRights.FullControl, 
                    AccessControlType.Allow));

                dirInfo.SetAccessControl(dirSec);
            }

            database = new ESB2DatabaseContainer();
            database.Database.CreateIfNotExists();

            if (database.Users.Count() == 0)
                CreateAdminAccount();

            if (!database.StatusPageGroupings.Any(s => s.IsStatusBar == true))
                CreateStaticStatusPage();
        }

        private static void CreateStaticStatusPage()
        {
            var statusPage = new StatusPage()
            {
                Title = "Status Bar",
                Description = "System Generated Page",
                IsDisplayed = false
            };

            statusPage.StatusPageGroupings.Add(new StatusPageGrouping()
            {
                Title = "Status Bar Group",
                Description = "System Generated, only one group allowed.",
                IsStatusBar = true
            });

            database.StatusPages.Add(statusPage);

            database.SaveChanges();
        }

        private static void CreateAdminAccount()
        {
            var dialog = new InitailizeDatabase();
            dialog.ShowDialog();

            var user = new User()
            {
                Username = dialog.Username,
                Firstname = dialog.Firstname,
                Lastname = dialog.Lastname,

                UserPermission = UserPermissions.DatabaseAdmin | UserPermissions.EquipmentManager |
                UserPermissions.ReportViewer | UserPermissions.StatusBoardManager | UserPermissions.StatusUpdates |
                UserPermissions.UserManager,

                Password = PasswordHasher.HashPassword(dialog.Username, dialog.Password),

                AccountStatus = AccountStatus.AccountValid,
                AccountLocked = DateTime.Today.AddDays(-15)
            };

            database.Users.Add(user);
            database.UserLog.Add(new UserLogEvent {
                Timestamp = DateTime.Now,
                User = user,
                UserEvent = UserEvents.AccountCreated });

            database.SaveChanges();
        }

        #endregion
    }
}
