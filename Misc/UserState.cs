using SF_16_POP2020.Models;
using System;

namespace SF_16_POP2020.Misc
{
    public static class UserState
    {
        private static User loggedUser { get; set; } = null;

        public static User LoggedUser => loggedUser;

        public static void Logout()
        {
            if (loggedUser == null)
                throw new InvalidOperationException();
            loggedUser = null;
        }

        public static void Login(User user)
        {
            if (loggedUser != null)
                throw new InvalidOperationException();
            loggedUser = user;
        }
    }
}
