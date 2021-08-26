using SF_16_POP2020.Models;

namespace SF_16_POP2020.Misc
{
    public static class Util
    {
        public static string CONNECTION_STRING = "server=localhost;user=root;database=CLINIC_SERVICE;port=3306;password=ivancar123";
        public static ERole ParseRole(int value)
        {
            if (value == 0)
                return ERole.PATIENT;
            else if (value == 1)
                return ERole.DOCTOR;
            else if (value == 2)
                return ERole.ADMIN;
            else
                return ERole.GUEST;
        }
    }
}

