using MySql.Data.MySqlClient;
using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;
using System.Collections.Generic;

namespace SF_16_POP2020.Services
{
    public static class UserService
    {
        public static User MapOneFromCommand(MySqlCommand cmd)
        {
            using (var rd = cmd.ExecuteReader())
            {
                if (rd.Read())
                {

                    return new User
                    {
                        Pin = rd.GetString(0),
                        FirstName = rd.GetString(1),
                        LastName = rd.GetString(2),
                        Email = rd.GetString(3),
                        Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                        Password = rd.GetString(5),
                        Role = Util.ParseRole(rd.GetInt32(6)),
                        Address = AddressService.FindById(rd.GetInt32(7))
                    };
                }
            }
            return null;
        }

        public static User FindByPin(string pin)
        {
            User retval = null;
            string sql = @"SELECT 
                pin, first_name, last_name, email, gender, password, role, address_id 
                FROM users WHERE pin = @PIN AND deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    _ = cmd.Parameters.AddWithValue("PIN", pin);
                    retval = MapOneFromCommand(cmd);
                }
            }
            return retval;
        }

        public static User FindAnyByPin(string pin)
        {
            User retval = null;
            string sql = @"SELECT 
            pin, first_name, last_name, email, gender, password, role, address_id 
            FROM users WHERE pin = @PIN";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    _ = cmd.Parameters.AddWithValue("PIN", pin);
                    retval = MapOneFromCommand(cmd);
                }
            }
            return retval;
        }
        public static User FindUserByPinAndPassword(string pin, string password)
        {
            User retval = null;
            string sql = @"SELECT 
            pin, first_name, last_name, email, gender, password, role, address_id  
            FROM users WHERE pin = @PIN
            AND password = @PASSWORD AND deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    _ = cmd.Parameters.AddWithValue("PIN", pin);
                    _ = cmd.Parameters.AddWithValue("PASSWORD", password);
                    retval = MapOneFromCommand(cmd);
                }
            }
            return retval;
        }

        public static bool SaveUser(string firstName, string lastName, string pin, string email, EGender gender, string password, string street, string number, string town, string country)
        {
            if (FindAnyByPin(pin) != null)
                return false;

            var addressId = AddressService.FindByStreetAndNumberAndTownAndCountry(street, number, town, country)?.Id;
            if (!addressId.HasValue) addressId = AddressService.SaveAddress(street, number, town, country);
            string sql = $@"INSERT INTO users (pin, first_name, last_name, email, gender, password, role, address_id)
            VALUES (@PIN,@FIRSTNAME,@LASTNAME,@EMAIL,@GENDER,@PASSWORD,@ROLE,@ADDRESS_ID)";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", pin);
                    cmd.Parameters.AddWithValue("FIRSTNAME", firstName);
                    cmd.Parameters.AddWithValue("LASTNAME", lastName);
                    cmd.Parameters.AddWithValue("EMAIL", email);
                    cmd.Parameters.AddWithValue("GENDER", gender);
                    cmd.Parameters.AddWithValue("PASSWORD", password);
                    cmd.Parameters.AddWithValue("ROLE", ERole.PATIENT);
                    cmd.Parameters.AddWithValue("ADDRESS_ID", addressId);
                    var ret = cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public static void UpdateUser(User user)
        {
            string sql = @"UPDATE users
            SET first_name = @FIRSTNAME,
            last_name = @LASTNAME,
            email = @EMAIL,
            gender = @GENDER,
            password = @PASSWORD,
            role = @ROLE,
            address_id = @ADDRESS_ID
            WHERE pin = @PIN";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("FIRSTNAME", user.FirstName);
                    cmd.Parameters.AddWithValue("LASTNAME", user.LastName);
                    cmd.Parameters.AddWithValue("EMAIL", user.Email);
                    cmd.Parameters.AddWithValue("GENDER", user.Gender);
                    cmd.Parameters.AddWithValue("PASSWORD", user.Password);
                    cmd.Parameters.AddWithValue("ROLE", user.Role);
                    cmd.Parameters.AddWithValue("ADDRESS_ID", user.Address.Id);
                    cmd.Parameters.AddWithValue("PIN", user.Pin);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<User> FindAll()
        {
            var retval = new List<User>();
            string sql = @"SELECT 
            pin, first_name, last_name, email, gender, password, role, address_id
            FROM users WHERE deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        retval.Add(new User
                        {
                            Pin = rd.GetString(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            Email = rd.GetString(3),
                            Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                            Password = rd.GetString(5),
                            Role = Util.ParseRole(rd.GetInt32(6)),
                            Address = AddressService.FindById(rd.GetInt32(7))
                        });
                    }
                }
            }
            return retval;
        }
        public static List<User> FindAllByFirstNameAndLastNameAndEmailAndAddress(string fName, string lName, string email, string address, ERole? role)
        {
            var retval = new List<User>();
            string sql = @" 
            SELECT 
            pin, first_name, last_name, email, gender, password, role, address_id
            FROM users


U
            INNER JOIN address A
            ON U.adress_id = A.id
            WHERE U.deleted = 0
            AND U.first_name LIKE CONCAT('%', @FIRSTNAME, '%')
            AND U.last_name LIKE CONCAT('%', @LASTNAME, '%')
            AND U.email LIKE CONCAT('%', @EMAIL, '%')
		    AND (
                 CONCAT('%', A.street, '%') LIKE CONCAT('%', @ADDRESS, '%')
              OR CONCAT('%', A.number, '%') LIKE CONCAT('%', @ADDRESS, '%')
              OR CONCAT('%', A.town, '%') LIKE CONCAT('%', @ADDRESS, '%')
              OR CONCAT('%', A.country, '%') LIKE CONCAT('%', @ADDRESS, '%')
              OR CONCAT('%', A.street, ' ', A.number, ' ', A.town, ', ',A.country) LIKE CONCAT('%', @ADDRESS, '%')
                )
            ";
            if (role.HasValue)
                sql += " AND U.role = @ROLE";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("FIRSTNAME", fName.Trim());
                    cmd.Parameters.AddWithValue("LASTNAME", lName.Trim());
                    cmd.Parameters.AddWithValue("EMAIL", email.Trim());
                    cmd.Parameters.AddWithValue("ADDRESS", address.Trim());
                    if (role.HasValue)
                        cmd.Parameters.AddWithValue("ROLE", role);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        retval.Add(new User
                        {
                            Pin = rd.GetString(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            Email = rd.GetString(3),
                            Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                            Password = rd.GetString(5),
                            Role = Util.ParseRole(rd.GetInt32(6)),
                            Address = AddressService.FindById(rd.GetInt32(7))
                        });

                    }
                }
            }
            return retval;
        }

    }
}
