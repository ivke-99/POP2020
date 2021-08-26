using System;

namespace SF_16_POP2020.Models
{
    public class Appointment
    {


        public int Id { get; set; }
        public Doctor Doctor { get; set; }
        public EStatus Status { get; set; }
        public Patient Patient { get; set; }
        public DateTime DateOfAppointment { get; set; }
    }
}
