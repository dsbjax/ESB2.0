using Database.DatabaseControls;
using Database.LoginControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Database
{
    // This classs is used to access the database for the status board program
    public static class DatabaseAccess
    {
        private static ESB2DatabaseContainer database = ESB2db.GetDatabase();

        public static IEnumerable<Outage> LastMonthsOutages()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);

            return database.Outages
                .Where(o=> o.Completed == true)
                .Where(o => o.End.Value.Month == lastMonth.Month && o.End.Value.Year == lastMonth.Year)
                .OrderBy(o => o.Start);
        }

        public static IEnumerable<Outage> GetOutages(DateTime start, DateTime end)
        {
            return database.Outages
                .Where(o => o.Completed == true)
                .Where(o => o.End.Value > start && o.End.Value < end)
                .OrderBy(o => o.Start);

        }


        #region Login/Logout
        // Accounts will reset after password failure lockout in minutes
        private static readonly double ACCOUNT_RESET_TIMESPAN = 15;
        private static readonly string DEFAULT_PASSWORD = "password123";


        public static void Login()
        {
            if (CurrentUser.currentUser != null)
                return;

            var dialog = new LoginDialog();

            if ((bool)dialog.ShowDialog())
            {
                var userLogin = database.Users.FirstOrDefault(u => u.Username == dialog.username.Text.ToLower());

                //Handle User Not Found
                if(userLogin == null)
                {
                    new LoginStatusMessageDialog("User Not Found").ShowDialog();
                    return;
                }

                //Handle Account Disabled
                if ((userLogin.AccountStatus & AccountStatus.AccountDisabled) == AccountStatus.AccountDisabled)
                {
                    new LoginStatusMessageDialog("Account Disabled").ShowDialog();
                    return;
                }

                //Handle Account Locked
                if((userLogin.AccountStatus & AccountStatus.AccountLocked) == AccountStatus.AccountLocked)
                {
                    if (DateTime.Now - userLogin.AccountLocked > TimeSpan.FromMinutes(ACCOUNT_RESET_TIMESPAN))
                    {
                        userLogin.AccountStatus ^= AccountStatus.AccountLocked;
                        userLogin.FailedLoginCounter = 0;

                        database.UserLog.Add(new UserLogEvent()
                        {
                            Timestamp = DateTime.Now,
                            User = userLogin,
                            UserEvent = UserEvents.AccountUnlocked
                        });
                    }
                    else
                    {
                        new LoginStatusMessageDialog("Account Locked").ShowDialog();
                        return;
                    }
                }

                //Validate Password
                if (!userLogin.Password.SequenceEqual(PasswordHasher.HashPassword(dialog.Username, dialog.Password)))
                {
                    new LoginStatusMessageDialog("Password Incorrect").ShowDialog();
                    database.UserLog.Add(new UserLogEvent()
                    {
                        Timestamp = DateTime.Now,
                        User = userLogin,
                        UserEvent = UserEvents.FailedLogin
                    });

                    userLogin.FailedLoginCounter++;
                    if (userLogin.FailedLoginCounter > 5)
                    {
                        userLogin.AccountStatus |= AccountStatus.AccountLocked;
                        database.UserLog.Add(new UserLogEvent()
                        {
                            Timestamp = DateTime.Now,
                            User = userLogin,
                            UserEvent = UserEvents.AccountLocked
                        });
                    }

                    return;
                }

                //Handle Change Password
                while((userLogin.AccountStatus & AccountStatus.UserMustChangePassword) == AccountStatus.UserMustChangePassword)
                {
                    var changePasswordDialog = new UserMustChangePasswordDialog();

                    changePasswordDialog.ShowDialog();

                    if (PasswordHasher.HashPassword(userLogin.Username, changePasswordDialog.OldPassword).SequenceEqual(userLogin.Password))
                    {
                        userLogin.Password = PasswordHasher.HashPassword(userLogin.Username, changePasswordDialog.NewPassword);
                        userLogin.AccountStatus ^= AccountStatus.UserMustChangePassword;

                        database.UserLog.Add(new UserLogEvent()
                        {
                            Timestamp = DateTime.Now,
                            User = userLogin,
                            UserEvent = UserEvents.PasswordChanged
                        });

                        Save();
                    }
                    else
                        new LoginStatusMessageDialog("Password Incorrect").ShowDialog();
                }

                //Login Success! Reset counters, log event and set current user.
                userLogin.FailedLoginCounter = 0;
                database.UserLog.Add(new UserLogEvent()
                {
                    Timestamp = DateTime.Now,
                    User = userLogin,
                    UserEvent = UserEvents.UserLogin
                });

                CurrentUser.SetCurrentUser(userLogin);
            }
        }


        public static StatusPage UpdateStatusPage(int statusPageId)
        {
            return database.StatusPages.First(p => p.Id == statusPageId);
        }


        public static void Logout()
        {
            if(CurrentUser.currentUser != null)
                database.UserLog.Add(new UserLogEvent()
                {
                    Timestamp = DateTime.Now,
                    User = CurrentUser.currentUser,
                    UserEvent = UserEvents.UserLogin
                });

            Save();
            CurrentUser.SetCurrentUser(null);
        }
        #endregion

        private static ObservableCollection<StatusPage> allStatusPages = new ObservableCollection<StatusPage>();
        public static ObservableCollection<StatusPage> GetAllStatusPages()
        {
            allStatusPages.Clear();
            if (database.ChangeTracker.HasChanges())
                Save();

            var pages = database.StatusPages.OrderBy(p => p.Index).ToList();

            foreach (var page in pages)
                page.StatusPageGroupings = page.StatusPageGroupings.OrderBy(g => g.Index).ToList();

            foreach (var page in pages)
                allStatusPages.Add(page);

            return allStatusPages;
        }


        #region User Control
        private static ObservableCollection<User> userList = new ObservableCollection<User>();
        internal static void NewUser()
        {
            var dialog = new NewUserDialog();
            dialog.NewUserRequest += NewUserRequestHandler;

            dialog.ShowDialog();
        }

        public static ObservableCollection<User> GetUserList()
        {
            RefreshUserList();
            return userList;
        }

        internal static void RefreshUserList()
        {
            userList.Clear();
            foreach (var user in database.Users.OrderBy(u => u.Firstname).OrderBy(u => u.Lastname))
                userList.Add(user);
        }

        public static List<UserLogEvent> GetUserEventsLog()
        {
            return database.UserLog.OrderBy(l => l.Timestamp).ToList();
        }

        // To be implemented
        public static IEnumerable GetUserEvents()
        {
            return null;
        }

        internal static void ResetPassword(User selectedItem)
        {
            database.Users.First(u => u.Id == selectedItem.Id).Password = PasswordHasher.HashPassword(selectedItem.Username, DEFAULT_PASSWORD);
            Save();
        }

        internal static void SetPassword(User selectedItem, string password)
        {
            database.Users.First(u => u.Id == selectedItem.Id).Password = PasswordHasher.HashPassword(selectedItem.Username, password);
            Save();
        }


        private static void NewUserRequestHandler(object sender, NewUSerEventArgs e)
        {
            if (database.Users.Any(u => u.Username == e.Username.ToLower()))
                new LoginStatusMessageDialog("Username: " + e.Username + "exists, select new username").ShowDialog();
            else
            {
                var newUser = new User()
                {
                    Username = e.Username.ToLower(),
                    Firstname = e.Firstname,
                    Lastname = e.Lastname,
                    UserPermission = UserPermissions.ReportViewer,
                    AccountStatus = AccountStatus.UserMustChangePassword,
                    Password = PasswordHasher.HashPassword(e.Username.ToLower(), "password123"),
                    AccountLocked = DateTime.Today.AddDays(-15),
                    FailedLoginCounter = 0
                };

                database.Users.Add(newUser);

                database.UserLog.Add(new UserLogEvent()
                {
                    Timestamp = DateTime.Now,
                    User = newUser,
                    UserEvent = UserEvents.AccountCreated
                });

                Save();
                RefreshUserList();
            }
        }
        #endregion

        public static void DeletePageGrouping(StatusPageGrouping selectedItem)
        {
            database.StatusPageGroupings.Remove(selectedItem);
            GetAllStatusPages();
        }

        public static void DeleteStatusPage(StatusPage selectedItem)
        {
            database.StatusPages.Remove(selectedItem);
            GetAllStatusPages();
        }

        public static int StatusPageCount()
        {
            return database.StatusPages.Max(p => p.Index);
        }

        #region Equipment Manager
        private static ObservableCollection<EquipmentSystem> systemList = new ObservableCollection<EquipmentSystem>();
        public static ObservableCollection<EquipmentSystem> GetEquipmentList()
        {
            RefreshSystemList();
            return systemList;
        }

        internal static void RefreshSystemList()
        {
            var tempSystemList = database.EquipmentSystems.OrderBy(e => e.Nomenclature);

            foreach(var system in tempSystemList)
            {
                foreach (var grouping in system.EquipmentGroupings)
                    grouping.Equipment = grouping.Equipment.OrderBy(e => e.Nomenclature).ToList();

                system.EquipmentGroupings = system.EquipmentGroupings.OrderBy(e => e.Title).ToList();
            }

            systemList.Clear();
            foreach (var system in tempSystemList)
                systemList.Add(system);
        }

        internal static void AddEquipmentSystem()
        {
            var dialog = new AddNewEquipmentSystemDialog();
            dialog.NewSystemRequest += NewSystemRequest;

            dialog.ShowDialog();
        }

        private static void NewSystemRequest(object sender, NewSystemRequestEventArgs e)
        {
            database.EquipmentSystems.Add(new EquipmentSystem()
            {
                Nomenclature = e.Nomenclature,
                Description = e.Description
            });

            Save();
            RefreshSystemList();
        }

        internal static void DeleteEquipmentSystem(EquipmentSystem equipmentSystem)
        {
            var dialog = new ConfirmDeleteDialog();
            if ((bool)dialog.ShowDialog())
            {
                database.EquipmentSystems.Remove(equipmentSystem);

                Save();
                RefreshSystemList();
            }
        }

        public static int GroupCount(StatusPageGrouping value)
        {
            int count = 0;
            foreach (var page in database.StatusPages.ToList())
                if (page.StatusPageGroupings.Contains(value))
                    count = page.StatusPageGroupings.Count;

            return count;
        }

        private static ListView listView;
        internal static void AddEquipmentGrouping(EquipmentSystem selectedItem, ListView list)
        {
            listView = list;
            var dialog = new AddNewEquipmentGroupingDialog(selectedItem);
            dialog.NewEquipmentGroupRequest += NewEquipmentGroupRequest;

            dialog.ShowDialog();
        }

        private static void NewEquipmentGroupRequest(object sender, NewEquipmentGroupRequestEventArgs e)
        {
            database.EquipmentGroupings.Add(new EquipmentGrouping()
            {
                Title = e.Title,
                Description = e.Description,
                EquipmentSystemId = e.EquipmentSystem.Id
            });

            Save();
            RefreshSystemList();
            listView.SelectedItem = database.EquipmentSystems.First(s => s.Id == e.EquipmentSystem.Id);
        }

        internal static void DeleteEquipmentGrouping(EquipmentGrouping equipmentGrouping, ListView list)
        {


            var dialog = new ConfirmDeleteDialog();
            if((bool)dialog.ShowDialog())
            {
                EquipmentSystem selectedSystem = null;

                foreach (var system in database.EquipmentSystems)
                    if (system.EquipmentGroupings.Any(e => e.Id == equipmentGrouping.Id))
                        selectedSystem = system; database.EquipmentGroupings.Remove(equipmentGrouping);

                Save();
                RefreshSystemList();
                list.SelectedItem = systemList.First(s => s.Id == selectedSystem.Id);
            }
        }

        internal static void AddNewEquipment(EquipmentGrouping equipmentGrouping, ListView systemListView, ListView equipmentGroupsListView)
        {
            var dialog = new AddNewEquipmentDialog(equipmentGrouping, systemListView, equipmentGroupsListView);
            dialog.NewEquipmentRequest += NewEquipmentRequest;

            dialog.ShowDialog();
        }

        private static void NewEquipmentRequest(object sender, NewEquipmentRequestEventArgs e)
        {
            var newEquipment = new Equipment()
            {
                Nomenclature = e.Nomenclature,
                Description = e.Description,
                EquipmentStatus = EquipmentStatus.Operational,
                EquipmentGroupingId = e.EquipmentGrouping.Id
            };

            database.EquipmentListing.Add(newEquipment);
            Save();
            RefreshSystemList();

            foreach(var system in database.EquipmentSystems)
                foreach(var equimpmentGrouping in system.EquipmentGroupings)
                    if(equimpmentGrouping.Equipment.Any(eq=> eq.Id == newEquipment.Id))
                    {
                        e.SystemListView.SelectedItem = system;
                        e.EquipmentGroupsListView.SelectedItem = equimpmentGrouping;
                    }
        }

        internal static void DeleteEquipment(Equipment selectedItem, ListView equipmentGroupsListView, int equipmentGroupingId, ListView systemListView, int systemId)
        {
            var dialog = new ConfirmDeleteDialog();

            if((bool) dialog.ShowDialog())
            {
                database.EquipmentListing.Remove(selectedItem);
                Save();
                RefreshSystemList();

                systemListView.SelectedItem = systemList.First(s => s.Id == systemId);
                equipmentGroupsListView.SelectedItem = 
                    ((EquipmentSystem)systemListView.SelectedItem).EquipmentGroupings.First(e => e.Id == equipmentGroupingId);
            }
        }
        #endregion


        private static ObservableCollection<Outage> outages = new ObservableCollection<Outage>();
        private static ObservableCollection<Outage> scheduledOutages = new ObservableCollection<Outage>();

        internal static ObservableCollection<Outage> GetScheduledOutages()
        {
            scheduledOutages.Clear();

            Save();

            foreach (var outage in database.Outages.Where(o =>
            o.OutageType == OutageType.ScheduledMaintenance &&
            o.Canceled == false &&
            o.Completed == false).OrderBy(o => o.Start))
                scheduledOutages.Add(outage);

            return scheduledOutages;
        }


        internal static ObservableCollection<Outage> GetOutages()
        {
            outages.Clear();

            Save();

            foreach (var outage in database.Outages.Where(o =>
            o.OutageType == OutageType.CorrectiveMaintenance &&
            o.Completed == false).OrderBy(o => o.Start))
                outages.Add(outage);

            return outages;
        }

        private static ObservableCollection<Outage> next7DaysScheduledOutages = new ObservableCollection<Outage>();
        public static ObservableCollection<Outage> GetNext7DaysScheduledOutages()
        {
            Save();

            var nextday = DateTime.Today.AddDays(2);
            var nextWeek = DateTime.Today.AddDays(8);

            next7DaysScheduledOutages.Clear();
            foreach (var outage in database.Outages.
                Where(o => o.OutageType == OutageType.ScheduledMaintenance).
                Where(o => o.Completed == false && o.Canceled == false).
                Where(o=> o.Start.CompareTo(nextday) > 0).
                Where(o=> o.Start.CompareTo(nextWeek) < 0).
                OrderBy(o => o.Start))

                next7DaysScheduledOutages.Add(outage);

            return next7DaysScheduledOutages;
        }

        private static ObservableCollection<Outage> tomorrowScheduledOutages = new ObservableCollection<Outage>();
        public static ObservableCollection<Outage> GetTomorrowScheduledOutages()
        {
            Save();

            var tomorrow = DateTime.Today.AddDays(1);
            var nextDay = DateTime.Today.AddDays(2);

            tomorrowScheduledOutages.Clear();
            foreach (var outage in database.Outages.
                Where(o => 
                o.OutageType == OutageType.ScheduledMaintenance &&
                o.Completed == false &&
                o.Canceled == false &&
                o.Start.CompareTo(tomorrow) > 0 &&
                o.Start.CompareTo(nextDay) < 0).
                OrderBy(o => o.Start))

                tomorrowScheduledOutages.Add(outage);

            return tomorrowScheduledOutages;
        }

        private static ObservableCollection<Outage> todayScheduledOutages = new ObservableCollection<Outage>();
        public static ObservableCollection<Outage> GetTodayScheduledOutages()
        {
            Save();

            var currentTime = DateTime.UtcNow;
            var tomorrow = DateTime.Today.AddDays(1);

            todayScheduledOutages.Clear();
            foreach (var outage in database.Outages.
                Where(o => o.OutageType == OutageType.ScheduledMaintenance).
                Where(o => o.Completed == false).
                Where(o=> o.Canceled == false).
                Where(o => o.Start.CompareTo(currentTime) > 0).
                Where(o => o.Start.CompareTo(tomorrow) < 0).
                OrderBy(o => o.Start))
                
                todayScheduledOutages.Add(outage);

            return todayScheduledOutages;
        }

        private static ObservableCollection<Outage> activeScheduledOutages = new ObservableCollection<Outage>();
        public static ObservableCollection<Outage> GetActiveScheduleOutages()
        {
            Save();

            var currentTime = DateTime.UtcNow;

            activeScheduledOutages.Clear();
            foreach (var outage in database.Outages.
                Where(o =>
                o.OutageType == OutageType.ScheduledMaintenance &&
                o.Completed == false &&
                o.Canceled == false &&
                o.Start.CompareTo(currentTime) < 0).
                OrderBy(o => o.Start))
            {
                foreach (var equipment in outage.Equipment)
                    if (equipment.EquipmentStatus != EquipmentStatus.OffLine)
                        equipment.EquipmentStatus = EquipmentStatus.OffLine;

                activeScheduledOutages.Add(outage);
            }

            Save();

            GetStatusBarEquipment();

            return activeScheduledOutages;
        }


        private static void Save()
        {
            if (database.ChangeTracker.HasChanges())
                database.SaveChanges();
        }

        private static ObservableCollection<Outage> currentOutages = new ObservableCollection<Outage>();
        public static ObservableCollection<Outage> GetCurrentOutages()
        {
            Save();

            currentOutages.Clear();
            foreach (var outage in database.Outages.
                Where(o => o.OutageType == OutageType.CorrectiveMaintenance).
                Where(o => o.Completed == false).
                OrderBy(o => o.Start))

                currentOutages.Add(outage);

            return currentOutages;
        }

        private static ObservableCollection<Equipment> statusBarEquipment = new ObservableCollection<Equipment>();
        public static ObservableCollection<Equipment> GetStatusBarEquipment()
        {
            statusBarEquipment.Clear();



            Save();

            foreach (var equipment in database.StatusPageGroupings.First(s => s.IsStatusBar == true).Equipment.OrderBy(e=> e.Nomenclature))
                statusBarEquipment.Add(equipment);

            return statusBarEquipment;
        }


        public static void InsertInStatusBar(Equipment selectedItem)
        {
            database.StatusPageGroupings.First(s => s.IsStatusBar == true).Equipment.Add(selectedItem);
            GetStatusBarEquipment();
        }


        public static void RemoveFromStatusBar(Equipment selectedItem)
        {
            database.StatusPageGroupings.First(s => s.IsStatusBar == true).Equipment.Remove(selectedItem);
            GetStatusBarEquipment();
        }


        public static void AddNewStatusPage(StatusPage newPage)
        {
            database.StatusPages.Add(newPage);
            GetAllStatusPages();
        }


        public static IEnumerable<StatusPage> GetActiveStatusPages()
        {
            var pages = database.StatusPages.Where(p => p.IsDisplayed == true).OrderBy(p => p.Index).ToList();

            foreach (var page in pages)
                page.StatusPageGroupings = page.StatusPageGroupings.OrderBy(g => g.Index).ToList();

            return pages;
        }

        public static IEnumerable<Outage> GetStatusReport()
        {
            List<Outage> outages = new List<Outage>();

            outages.AddRange(database.Outages.Where(o => o.Canceled == false && o.Completed == false));

            var twoDays = DateTime.Today.AddDays(-2);
            outages.AddRange(database.Outages.Where(o => (o.Canceled == true || o.Completed == true) && o.End.Value > twoDays));

            
            return outages.OrderBy(o => o.Start).ToList();
        }
    }

    public interface IRefreshData
    {
        void RefreshData();
    }

}
