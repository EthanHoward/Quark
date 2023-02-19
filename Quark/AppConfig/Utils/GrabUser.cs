using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Quark.AppConfig.Util.Config;
using Quark.Util.Logging;

namespace Quark.AppConfig.Utils
{
    public abstract class GrabUser
    {
        protected GrabUser()
        {
            UserName = null;
        }

        private static string UserName { get; set; }

        public static string ShowDialog()
        {
            if (Logger.Instance != null) Logger.Instance.WithClass("Showing user selection Dialog...");
            var users = GetSystemUsers();

            // here, if there is only one user, we don't need to show the dialog
            if (users.Count == 1)
            {
                UserName = users[0];
                return UserName;
            }

            var dialog = new GrabUserDialog(users);
            if (dialog.ShowDialog() != true) return null;
            UserName = dialog.SelectedUser;
            return UserName;
        }

        private static List<string> GetSystemUsers()
        {
            var currentUser = WindowsIdentity.GetCurrent().User;
            if (currentUser != null)
                return currentUser
                    .Translate(typeof(NTAccount))
                    .ToString()
                    .Split('\\')
                    .Skip(1)
                    .ToList();
            return new List<string>();
        }
    }
}