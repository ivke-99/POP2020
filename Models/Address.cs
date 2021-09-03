namespace SF_16_POP2020.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return $"{Street} {Number} {Town}, {Country}";
        }

        public Address()
        {
            Street = "";
            Number = "";
            Town = "";
            Country = "";
        }

        public Address(string street,string number,string town,string country)
        {
            Street = street;
            Number = number;
            Town = town;
            Country = country;
        }
    }
}
