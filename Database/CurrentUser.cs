using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class CurrentUser
    {

        public string Username { get { return DatabaseUser.Username; } }
        public string Fullname { get { return DatabaseUser.Lastname + ", " + DatabaseUser.Firstname; } }
        public UserPermissions UserPermissions {get { return DatabaseUser.UserPermission; }}

        

        internal static User DatabaseUser;
        private static List<ICurrentUserSubscriber> subscribers = new List<ICurrentUserSubscriber>();

        private CurrentUser()
        {
            
        }

        internal static void SetCurrentUser(User user)
        {
            DatabaseUser = user;
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
