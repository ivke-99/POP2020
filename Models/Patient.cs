using System.Collections.Generic;

namespace SF_16_POP2020.Models
{
    public class Patient : User
    {
        public List<Prescription> ListOfPrescriptions { get; set; }
    }
}
