using SF_16_POP2020.Models;
using System;

namespace SF_16_POP2020.Misc
{
    public static class UserState
    {
        public static User LoggedUser { get; set; }

        public static void Logout()
        {
            if (LoggedUser == null)
                throw new InvalidOperationException();
            LoggedUser = null;
        }

        public static void Login(User user)
        {
            if (LoggedUser != null)
                throw new InvalidOperationException();
            LoggedUser = user;
        }
    }
}
