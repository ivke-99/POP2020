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
    public static class PatientService
    {
        public static Patient FindByPin(string pin)
        {
            Patient patient = null;
            var sql = @"SELECT * FROM users WHERE deleted = 0 AND role = 0 AND pin = @PIN";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", pin);
                    var rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        patient = new Patient
                        {
                            Pin = rd.GetString(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            Email = rd.GetString(3),
                            Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                            Password = rd.GetString(5),
                            Role = Util.ParseRole(rd.GetInt32(6)),
                            Address = AddressService.FindById(rd.GetInt32(8)),
                            ListOfPrescriptions = PrescriptionService.FindAllByPatientPin(rd.GetString(0))
                        };
                    }
                }
            }

            return patient;
        }

        public static List<Patient> FindAll()
        {
            var lista = new List<Patient>();
            var sql = @"SELECT * FROM users WHERE deleted = 0
            AND role = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        lista.Add(new Patient
                        {
                            Pin = rd.GetString(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            Email = rd.GetString(3),
                            Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                            Password = rd.GetString(5),
                            Role = Util.ParseRole(rd.GetInt32(6)),
                            Address = AddressService.FindById(rd.GetInt32(8))
                        });
                    }
                }
            }

            return lista;
        }

        public static List<Patient> FindAllByDoctorAndAppointment(string loggedUserPIN)
        {
            var patients = new List<Patient>();
            var sql = @"SELECT * FROM users U
            INNER JOIN appointment A
            ON U.pin = A.patient_pin
            WHERE U.deleted == 0
            AND A.deleted == 0
            AND status = @STATUS
            AND doctor_pin = @PIN
            AND role = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {

                    cmd.Parameters.AddWithValue("STATUS", EStatus.BOOKED);
                    cmd.Parameters.AddWithValue("PIN", loggedUserPIN);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        patients.Add(new Patient
                        {
                            Pin = rd.GetString(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            Email = rd.GetString(3),
                            Gender = rd.GetBoolean(4) ? EGender.FEMALE : EGender.MALE,
                            Password = rd.GetString(5),
                            Role = Util.ParseRole(rd.GetInt32(6)),
                            Address = AddressService.FindById(rd.GetInt32(8))
                        });
                    }
                    }
                }
            return patients;
        }
    }
}
