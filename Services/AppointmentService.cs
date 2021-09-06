﻿using MySql.Data.MySqlClient;
using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;
using System.Collections.Generic;
using System.Text;

namespace SF_16_POP2020.Services
{
    public static class AppointmentService
    {
        public static List<Appointment> FindAll()
        {
            var list = new List<Appointment>();
            var sql = @"SELECT * FROM appointment WHERE deleted = 0";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var a = new Appointment
                        {
                            Id = rd.GetInt32(0),
                            Doctor = DoctorService.FindByPIN(rd.GetString(1)),
                            Status = rd.GetInt32(2) == 1 ? EStatus.BOOKED : EStatus.AVAILABLE,
                            Patient = rd.IsDBNull(3) ? null :  PatientService.FindByPin(rd.GetString(3)),
                            DateOfAppointment = rd.GetDateTime(5)
                        };
                        list.Add(a);
                    }
                }
            }
            return list;
        }

        public static bool SaveAppointment(Appointment appointment)
        {
            string sql;
            if (appointment.Patient != null)
                sql = @"INSERT INTO appointment (doctor_pin, status, patient_pin, date_of_appointment)
                VALUES (@DPIN, @STATUS, @PPIN, @DATE)";
            else
                sql = @"INSERT INTO appointment (doctor_pin, status, date_of_appointment)
                VALUES (@DPIN, @STATUS, @DATE)";

            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("DPIN", appointment.Doctor.Pin);
                    cmd.Parameters.AddWithValue("STATUS", appointment.Status);
                    if (appointment.Patient != null)
                        cmd.Parameters.AddWithValue("PPIN", appointment.Patient.Pin);
                    cmd.Parameters.AddWithValue("DATE", appointment.DateOfAppointment);
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public static void DeleteAppointment(Appointment a)
        {
            string sql = @"UPDATE appointment SET deleted = 1 WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("ID", a.Id);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public static void UpdateAppointment(Appointment a)
        {
            string sql = $@"UPDATE appointment SET doctor_pin = @DPIN,
            status = @STATUS,
            patient_pin = @PPIN,
            date_of_appointment = @DATE
            WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("DPIN", a.Doctor.Pin);
                    cmd.Parameters.AddWithValue("STATUS", a.Status);
                    if (a.Patient != null)
                        cmd.Parameters.AddWithValue("PPIN", a.Patient.Pin);
                    else
                        cmd.Parameters.AddWithValue("PPIN", null);
                    cmd.Parameters.AddWithValue("DATE", a.DateOfAppointment);
                    cmd.Parameters.AddWithValue("ID", a.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void BookAppointment(Appointment ap, string loggedUserPin)
        {
            string sql = @"UPDATE appointment 
                SET patient_pin = @PIN,
                status = @STATUS
                WHERE id = @ID";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", loggedUserPin);
                    cmd.Parameters.AddWithValue("STATUS", EStatus.BOOKED);
                    cmd.Parameters.AddWithValue("ID", ap.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Appointment> FindAllByDoctor(string loggedUserPin)
        {
            var list = new List<Appointment>();
            var sql = @"SELECT * FROM appointment WHERE deleted = 0 AND doctor_pin = @PIN";
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("PIN", loggedUserPin);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var a = new Appointment
                        {
                            Id = rd.GetInt32(0),
                            Doctor = DoctorService.FindByPIN(rd.GetString(1)),
                            Status = rd.GetInt32(2) == 1 ? EStatus.BOOKED : EStatus.AVAILABLE,
                            Patient = rd.IsDBNull(3) ? null : PatientService.FindByPin(rd.GetString(3)),
                            DateOfAppointment = rd.GetDateTime(5)
                        };
                        list.Add(a);
                    }
                }
            }
            return list;
        }
    }
}
