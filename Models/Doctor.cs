namespace SF_16_POP2020.Models
{
    public class Doctor : User
    {
        public Clinic Clinic { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
