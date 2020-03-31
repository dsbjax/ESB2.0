using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    // Tis class is used to store the current active user and notify subscriber of user changes
    public class CurrentUser
    {

        public string Username { get { return currentUser.Username; } }
        public string Fullname { get { return currentUser.Lastname + ", " + currentUser.Firstname; } }
        public UserPermissions UserPermissions {get { return currentUser.UserPermission; }}

        

        internal static User currentUser;
        private static List<ICurrentUserSubscriber> subscribers = new List<ICurrentUserSubscriber>();

        private CurrentUser()
        {
            
        }

        internal static void SetCurrentUser(User user)
        {
            currentUser = user;
            var newCurrentUser = user != null ? new CurrentUser() : null;

            foreach (var subscriber in subscribers)
                subscriber.SetCurrentUser(newCurrentUser);
        }

        public static void Subscribe(ICurrentUserSubscriber subscriber)
        {
            subscribers.Add(subscriber);
        }
    }

    public interface ICurrentUserSubscriber
    {
        void SetCurrentUser(CurrentUser newCurrentUser);
    }
}
