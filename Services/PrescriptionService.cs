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
    public static class PrescriptionService
    {
        public static List<Prescription> FindAll()
        {
            var prescriptions = new List<Prescription>();
            var sql = @"SELECT * FROM prescription WHERE 
            deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        prescriptions.Add(new Prescription
                        {
                            Id = rd.GetInt32(0),
                            Description = rd.GetString(1),
                            Doctor = DoctorService.FindByPIN(rd.GetString(2))
                        });
                    }
                }
            }
            return prescriptions;
        }

        public static int SavePrescription(Prescription p)
        {
            string sql = $@"INSERT INTO prescription (description, doctor_pin)
            VALUES (@DESCRIPTION, @DOCTORPIN);
            SELECT LAST_INSERT_ID();";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    _ = cmd.Parameters.AddWithValue("DESCRIPTION", p.Description);
                    _ = cmd.Parameters.AddWithValue("DOCTORPIN", p.Doctor.Pin);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static void DeletePrescription(Prescription p)
        {
            string sql = @"UPDATE prescription SET deleted = 1 WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("ID", p.Id);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public static void UpdatePrescription(Prescription p)
        {
            string sql = @"UPDATE prescription 
            SET description = @DESCRIPTION,
                doctor_pin = @DOCTOR_PIN
                WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("DESCRIPTION", p.Description);
                    cmd.Parameters.AddWithValue("DOCTOR_PIN", p.Doctor?.Pin);
                    cmd.Parameters.AddWithValue("ID", p.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Prescription> FindAllByPatientPin(string pin)
        {
            var list = new List<Prescription>();
            string sql = @"SELECT P.* FROM prescription P INNER JOIN patient_prescription PP
            ON P.id = PP.prescription_id WHERE PP.patient_pin = @PIN AND P.deleted = 0 AND PP.deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", pin);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new Prescription
                        {
                            Id = rd.GetInt32(0),
                            Description = rd.GetString(1),
                            Doctor = DoctorService.FindByPIN(rd.GetString(2))
                        });
                    }
                }
            }
            return list;
        }


        public static List<Prescription> FindAllWherePatientIsNull(Patient patient)
        {
            var prescriptions = new List<Prescription>();
            var sql = @"
            SELECT P.* FROM prescription P
            LEFT JOIN patient_prescription PP ON P.id = PP.prescription_id
            AND PP.patient_pin = @PIN AND PP.deleted = 0
			WHERE P.deleted = 0
			AND PP.patient_pin IS NULL
            ";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", patient.Pin);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        prescriptions.Add(new Prescription
                        {
                            Id = rd.GetInt32(0),
                            Description = rd.GetString(1),
                            Doctor = DoctorService.FindByPIN(rd.GetString(2))
                        });
                    }
                }
            }
            return prescriptions;
        }

        public static void DeletePatientPrescription(Prescription prescription, Patient patient)
        {
            var sql = @"UPDATE patient_prescription SET deleted = 1
            WHERE patient_pin = @PIN AND prescription_id = @PID
            ";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", patient.Pin);
                    cmd.Parameters.AddWithValue("PID", prescription.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void AddPatientPrescription(Prescription prescription, Patient patient)
        {
            var sql = @"INSERT INTO patient_prescription (patient_pin, prescription_id) VALUES (@PIN, @PID)";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", patient.Pin);
                    cmd.Parameters.AddWithValue("PID", prescription.Id);
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public static List<Prescription> FindAllByDescriptionAndDoctor(string desc, Doctor doctor = null)
        {
            var list = new List<Prescription>();
            string sql = @"SELECT * FROM prescription WHERE deleted = 0
            AND description LIKE CONCAT('%', @DESCRIPTION, '%')";
            if (doctor != null)
                sql += " AND doctor_pin = @DPIN";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("DESCRIPTION", desc.Trim());
                    if (doctor != null)
                        cmd.Parameters.AddWithValue("DPIN", doctor.Pin);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new Prescription
                        {
                            Id = rd.GetInt32(0),
                            Description = rd.GetString(1),
                            Doctor = DoctorService.FindByPIN(rd.GetString(2))
                        });
                    }
                }
            }
            return list;
        }

        public static List<Prescription> FindAllByDoctor(string pin)
        {
            var list = new List<Prescription>();
            string sql = @"SELECT * FROM prescription WHERE deleted = 0
                AND doctor_pin = @PIN";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", pin);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new Prescription
                        {
                            Id = rd.GetInt32(0),
                            Description = rd.GetString(1),
                            Doctor = DoctorService.FindByPIN(rd.GetString(2))
                        });
                    }
                }
            }
            return list;
        }
    }
}
