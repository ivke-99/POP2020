namespace SF_16_POP2020.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Doctor Doctor { get; set; }
    }
}
