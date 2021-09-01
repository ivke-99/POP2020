using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;

namespace SF_16_POP2020.Services
{
    public static class AddressService
    {
        private static Address MapOneFromCommand(MySqlCommand cmd)
        {
            using (var rd = cmd.ExecuteReader())
            {
                if (rd.Read())
                {
                    return new Address
                    {
                        Id = rd.GetInt32(0),
                        Street = rd.GetString(1),
                        Number = rd.GetString(2),
                        Town = rd.GetString(3),
                        Country = rd.GetString(4)
                    };
                }
            }
            return null;
        }

        public static List<Address> FindAll()
        {
            var addresses = new List<Address>();
            var sql = @"SELECT * FROM Address WHERE 
            deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        addresses.Add(new Address
                        {
                            Id = rd.GetInt32(0),
                            Street = rd.GetString(1),
                            Number = rd.GetString(2),
                            Town = rd.GetString(3),
                            Country = rd.GetString(4),
                        });
                    }
                }
            }
            return addresses;
        }

        public static Address FindById(int id)
        {
            Address retval = null;
            string sql = $@"SELECT 
             id, street, number, town, country FROM Address WHERE id = @ID AND deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    _ = cmd.Parameters.AddWithValue("ID", id);

                    retval = MapOneFromCommand(cmd);
                }
                return retval;
            }

        }

        public static List<Address> FindAllByClinic()
        {
            var Addresses = new List<Address>();
            var sql = @"SELECT DISTINCT A.* FROM address A
            INNER JOIN clinic C ON A.id = C.address_id
            WHERE A.deleted = 0 AND C.deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        Addresses.Add(new Address
                        {
                            Id = rd.GetInt32(0),
                            Street = rd.GetString(1),
                            Number = rd.GetString(2),
                            Town = rd.GetString(3),
                            Country = rd.GetString(4),
                        });
                    }
                }
            }
            return Addresses;
        }

        public static Address FindByStreetAndNumberAndTownAndCountry(string Street, string Number, string Town, string Country)
        {
            Address retval = null;
            string sql = $@"SELECT 
            id, street, number, town, country FROM address WHERE
            street LIKE CONCAT('%', @STREET, '%') AND
            number LIKE CONCAT('%', @NUMBER, '%') AND
            town LIKE CONCAT('%', @TOWN, '%') AND
            country LIKE CONCAT('%', @COUNTRY, '%') AND
            deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    _ = cmd.Parameters.AddWithValue("STREET", Street);
                    _ = cmd.Parameters.AddWithValue("NUMBER", Number);
                    _ = cmd.Parameters.AddWithValue("TOWN", Town);
                    _ = cmd.Parameters.AddWithValue("COUNTRY", Country);
                    retval = MapOneFromCommand(cmd);
                }
                return retval;
            }

        }

        public static int? SaveAddressIfNotExists(Address address)
        {
            var address_id = FindByStreetAndNumberAndTownAndCountry(address.Street, address.Number, address.Town, address.Country)?.Id;
            if (!address_id.HasValue) address_id = SaveAddress(address.Street, address.Number, address.Town, address.Country);
            return address_id;
        }

        public static int SaveAddress(string Street, string Number, string Town, string Country)
        {
            string sql = $@"INSERT INTO address (street, number, town, country)
            VALUES (@STREET, @NUMBER, @TOWN, @COUNTRY);
            SELECT LAST_INSERT_ID();";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    _ = cmd.Parameters.AddWithValue("STREET", Street);
                    _ = cmd.Parameters.AddWithValue("NUMBER", Number);
                    _ = cmd.Parameters.AddWithValue("TOWN", Town);
                    _ = cmd.Parameters.AddWithValue("COUNTRY", Country);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static void DeleteAddress(Address adr)
        {
            string sql = @"UPDATE address SET deleted = 1 WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("ID", adr.Id);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public static void UpdateAddress(Address address)
        {
            string sql = @"UPDATE address 
            SET street = @STREET,
                number = @NUMBER,
                town = @TOWN,
                country = @COUNTRY
                WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("STREET", address.Street);
                    cmd.Parameters.AddWithValue("NUMBER", address.Number);
                    cmd.Parameters.AddWithValue("TOWN", address.Town);
                    cmd.Parameters.AddWithValue("COUNTRY", address.Country);
                    cmd.Parameters.AddWithValue("ID", address.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
