namespace SF_16_POP2020.Models
{
    public class User
    {
        public string Pin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public EGender Gender { get; set; }
        public string Password { get; set; }
        public ERole Role { get; set; }
        public Address Address { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
