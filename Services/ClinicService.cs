using MySql.Data.MySqlClient;
using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;
using System.Collections;
using System.Collections.Generic;

namespace SF_16_POP2020.Services
{
    public static class ClinicService
    {
        public static List<Clinic> FindAll()
        {
            var list = new List<Clinic>();
            var sql = @"SELECT * FROM clinic WHERE deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var clinic = new Clinic
                        {
                            Id = rd.GetInt32(0),
                            Name = rd.GetString(1),
                            Address = AddressService.FindById(rd.GetInt32(2)),
                        };
                        list.Add(clinic);
                    }
                }
            }

            return list;

        }

        public static Clinic FindById(int id)
        {
            Clinic retval = null;
            var sql = @"SELECT * FROM clinic WHERE deleted = 0
            AND id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("ID", id);
                    var rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        retval = new Clinic
                        {
                            Id = rd.GetInt32(0),
                            Name = rd.GetString(1),
                            Address = AddressService.FindById(rd.GetInt32(2)),
                        };
                    }
                }
            }
            return retval;
        }

        public static Clinic FindByDoctor(Doctor doctor)
        {
            var sql = @"SELECT * FROM clinic WHERE deleted = 0";
            if (doctor != null)
                sql += " AND id = @DOCTORID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    if (doctor != null)
                        cmd.Parameters.AddWithValue("DOCTORID", doctor.Clinic?.Id);
                    var rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        var clinic = new Clinic
                        {
                            Id = rd.GetInt32(0),
                            Name = rd.GetString(1),
                            Address = AddressService.FindById(rd.GetInt32(2)),
                        };
                        return clinic;
                    }
                }
            }
            return null;
        }

        public static List<Clinic> FindByDoctorAndAddress(Doctor doctor,Address address)
        {
            var list = new List<Clinic>();
            var sql = @"SELECT * FROM clinic WHERE deleted = 0";
            if (doctor != null)
                sql += " AND id = @DOCTORID";
            if (address != null)
                sql += " AND address_id = @ADDRESS_ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    if (doctor != null)
                        cmd.Parameters.AddWithValue("DOCTORID", doctor.Clinic?.Id);
                    if (address != null)
                        cmd.Parameters.AddWithValue("ADDRESS_ID", address.Id);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var clinic = new Clinic
                        {
                            Id = rd.GetInt32(0),
                            Name = rd.GetString(1),
                            Address = AddressService.FindById(rd.GetInt32(2)),
                        };
                        list.Add(clinic);
                    }
                }
            }
            return list;
        }

        public static List<Clinic> FindAllByAddress(Address address)
        {
            var list = new List<Clinic>();
            var sql = @"SELECT * FROM clinic WHERE deleted = 0";
            if (address != null)
                sql += " AND address_id = @AID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    if (address != null)
                        cmd.Parameters.AddWithValue("AID", address.Id);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var c = new Clinic
                        {
                            Id = rd.GetInt32(0),
                            Name = rd.GetString(1),
                            Address = AddressService.FindById(rd.GetInt32(2)),
                        };
                        list.Add(c);
                    }
                }
            }
            return list;
        }

        public static void DeleteClinic(Clinic dz)
        {
            string sql = @"UPDATE clinic SET deleted = 1 WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("ID", dz.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static bool SaveClinic(Clinic clinic)
        {
            var addressId = AddressService.SaveAddressIfNotExists(clinic.Address);
            if (addressId.HasValue) clinic.Address.Id = (int)addressId;
            string sql = $@"INSERT INTO clinic (clinic_name, address_id)
            VALUES (@NAME, @ADDRESS_ID)";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("NAME", clinic.Name.Trim());
                    cmd.Parameters.AddWithValue("ADDRESS_ID", addressId);
                    var ret = cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public static void UpdateClinic(Clinic clinic)
        {
            var addressId = AddressService.SaveAddressIfNotExists(clinic.Address);
            if (addressId.HasValue) clinic.Address.Id = (int)addressId;
            string sql = @"UPDATE clinic 
            SET clinic_name = @NAME,
            address_id = @ADDRESS

            WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("NAME", clinic.Name.Trim());
                    cmd.Parameters.AddWithValue("ADDRESS", addressId);
                    cmd.Parameters.AddWithValue("ID", clinic.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Clinic> FindAllByNameAndAddress(string name, string address)
        {
            var list = new List<Clinic>();
            string sql = @" 
            SELECT C.id, C.clinic_name, C.address_id
            FROM clinic C 
            INNER JOIN address A
            ON C.address_id = A.id
            WHERE C.deleted = 0
            AND C.clinic_name LIKE CONCAT('%', @NAME, '%')
		    AND CONCAT('%', A.street, ' ', A.number, ' ', A.town, ', ', A.country) LIKE CONCAT('%', @ADDRESS, '%')";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("NAME", name.Trim());
                    cmd.Parameters.AddWithValue("ADDRESS", address.Trim());
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var c = new Clinic
                        {
                            Id = rd.GetInt32(0),
                            Name = rd.GetString(1),
                            Address = AddressService.FindById(rd.GetInt32(2)),
                        };
                        list.Add(c);
                    }
                }
            }

            return list;
        }
    }

}
