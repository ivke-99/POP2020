using MySql.Data.MySqlClient;
using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_16_POP2020.Services
{
    public static class DoctorService
    {
        public static List<Doctor> FindAll()
        {
            var doctors = new List<Doctor>();
            var sql = @"SELECT * FROM users WHERE deleted = 0
            AND role = 1";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        doctors.Add(new Doctor
                        {
                            Pin = rd.GetString(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            Email = rd.GetString(3),
                            Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                            Password = rd.GetString(5),
                            Role = Util.ParseRole(rd.GetInt32(6)),
                            Clinic = ClinicService.FindById(rd.GetInt32(7)),
                            Address = AddressService.FindById(rd.GetInt32(8))
                        });
                    }
                }
            }
            return doctors;
        }

        public static Doctor FindByPIN(string pin)
        {
            Doctor doctor = null;
            var sql = @"SELECT * FROM users WHERE deleted = 0 AND role = 1 AND pin = @PIN";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", pin);
                    var rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        doctor = new Doctor
                        {
                            Pin = rd.GetString(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            Email = rd.GetString(3),
                            Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                            Password = rd.GetString(5),
                            Role = Util.ParseRole(rd.GetInt32(6)),
                            Clinic = ClinicService.FindById(rd.GetInt32(7)),
                            Address = AddressService.FindById(rd.GetInt32(8))
                        };
                    }
                }
            }
            return doctor;
        }

        public static bool SaveDoctor(Doctor d)
        {
            if (UserService.FindAnyByPin(d.Pin) != null)
                return false;

            var address_id = AddressService.FindByStreetAndNumberAndTownAndCountry(d.Address.Street.Trim(), d.Address.Number.Trim(), d.Address.Town.Trim(), d.Address.Country.Trim())?.Id;
            if (!address_id.HasValue) address_id = AddressService.SaveAddress(d.Address.Street.Trim(), d.Address.Number.Trim(), d.Address.Town.Trim(), d.Address.Country.Trim());
            string sql = $@"INSERT INTO users (pin, first_name, last_name, email, gender, password, role, address_id, clinic_id)
            VALUES (@PIN,@FIRSTNAME,@LASTNAME,@EMAIL,@GENDER,@PASSWORD,@ROLE,@ADDRESS_ID,@CLINIC_ID);
            SELECT LAST_INSERT_ID();";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", d.Pin.Trim());
                    cmd.Parameters.AddWithValue("FIRSTNAME", d.FirstName.Trim());
                    cmd.Parameters.AddWithValue("LASTNAME", d.LastName.Trim());
                    cmd.Parameters.AddWithValue("EMAIL", d.Email.Trim());
                    cmd.Parameters.AddWithValue("GENDER", d.Gender);
                    cmd.Parameters.AddWithValue("PASSWORD", d.Password.Trim());
                    cmd.Parameters.AddWithValue("ROLE", ERole.DOCTOR);
                    cmd.Parameters.AddWithValue("ADDRESS_ID", address_id);
                    cmd.Parameters.AddWithValue("CLINIC_ID", d.Clinic?.Id);
                    var ret = cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public static void UpdateDoctor(Doctor d)
        {
            var address_id = AddressService.FindByStreetAndNumberAndTownAndCountry(d.Address.Street.Trim(), d.Address.Number.Trim(), d.Address.Town.Trim(), d.Address.Country.Trim())?.Id;
            if (!address_id.HasValue) address_id = AddressService.SaveAddress(d.Address.Street.Trim(), d.Address.Number.Trim(), d.Address.Town.Trim(), d.Address.Country.Trim());
            string sql = @"UPDATE users
            SET first_name = @FIRSTNAME,
            last_name = @LASTNAME,
            email = @EMAIL,
            gender = @GENDER,
            password = @PASSWORD,
            role = @ROLE,
            address_id = @ADDRESS_ID,
            clinic_id = @CLINIC_ID

            WHERE pin = @PIN";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("FIRSTNAME", d.FirstName);
                    cmd.Parameters.AddWithValue("LASTNAME", d.LastName);
                    cmd.Parameters.AddWithValue("EMAIL", d.Email);
                    cmd.Parameters.AddWithValue("GENDER", d.Gender);
                    cmd.Parameters.AddWithValue("PASSWORD", d.Password);
                    cmd.Parameters.AddWithValue("ROLE", d.Role);
                    cmd.Parameters.AddWithValue("ADDRESS_ID", address_id);
                    cmd.Parameters.AddWithValue("CLINIC_ID", d.Clinic?.Id);
                    cmd.Parameters.AddWithValue("PIN", d.Pin);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Doctor> FindAllByClinic(Clinic clinic)
        {
            var doctors = new List<Doctor>();
            var sql = @"SELECT * FROM users WHERE deleted = 0
            AND role = 1 AND clinic_id = @CID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("CID", clinic.Id);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        doctors.Add(new Doctor
                        {
                            Pin = rd.GetString(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            Email = rd.GetString(3),
                            Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                            Password = rd.GetString(5),
                            Role = Util.ParseRole(rd.GetInt32(6)),
                            Clinic = ClinicService.FindById(rd.GetInt32(7)),
                            Address = AddressService.FindById(rd.GetInt32(8))
                        });
                    }
                }
            }
            return doctors;
        }
    }
}
