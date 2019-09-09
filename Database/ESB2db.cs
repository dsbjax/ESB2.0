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

namespace Database
{
    public static class ESB2db
    {
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
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionString = config.ConnectionStrings.ConnectionStrings["ESB2DatabaseContainer"];

            if (connectionString.ConnectionString.Contains("xxxxx"))
            {
                var dialog = new InitailizeDatabase();
                dialog.ShowDialog();

                if (!Directory.Exists(dialog.DatabaseFilename))
                    Directory.CreateDirectory(
                        dialog.DatabaseFilename.Substring(0, dialog.DatabaseFilename.LastIndexOf('\\')));

                try
                {
                    CreateDatabase(config, connectionString, dialog);

                    if (database.Users.Count() == 0)
                        CreateAdminAccount(dialog);
                }
                catch (Exception e)
                {
                    var exceptionDialog = new ExceptionErrorDialog("ESB2db.InitializeDatabase", e);
                    exceptionDialog.ShowDialog();
                }
            }

        }

        private static void CreateAdminAccount(InitailizeDatabase dialog)
        {
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

        private static void CreateDatabase(Configuration config, ConnectionStringSettings connectionString, InitailizeDatabase dialog)
        {
            connectionString.ConnectionString = connectionString.ConnectionString.Replace("xxxxx", dialog.DatabaseFilename);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");

            database = new ESB2DatabaseContainer();
            database.Database.CreateIfNotExists();
        }
        #endregion
    }
}
