namespace SF_16_POP2020.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }

        public override string ToString()
        {
            return $"{Name}, {Address?.Street} {Address?.Number}";
        }
    }
}
