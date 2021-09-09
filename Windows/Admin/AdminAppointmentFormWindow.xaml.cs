using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
using SF_16_POP2020.Windows.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SF_16_POP2020.Windows.Admin
{
    /// <summary>
    /// Interaction logic for AdminAppointmentFormWindow.xaml
    /// </summary>
    public partial class AdminAppointmentFormWindow : Window
    {
        public bool IsEdit { set; get; }
        public List<EStatus> EStatusTypes => Enum.GetValues(typeof(EStatus)).Cast<EStatus>().ToList();
        public List<Doctor> Doctors { set; get; } = DoctorService.FindAll();
        public List<Patient> Patients { get; set; } = PatientService.FindAll();
        public Appointment Appointment { get; set; }
        public AdminAppointmentFormWindow(Appointment app = null, bool isEdit = false)
        {
            IsEdit = isEdit;
            InitializeComponent();
            if (!IsEdit)
            {
                Appointment = new Appointment();
                CbStatus.IsEnabled = false;
                CbPatient.IsEnabled = false;
                Appointment.Status = EStatus.AVAILABLE;
            }
            else
            {
                Appointment = app;
                Appointment.Doctor = Doctors.FirstOrDefault(e => e.Pin == Appointment.Doctor.Pin);
                Appointment.Status = EStatusTypes.FirstOrDefault(e => e == Appointment.Status);
                if (Appointment.Patient != null)
                    Appointment.Patient = Patients.FirstOrDefault(e => e.Pin == Appointment.Patient.Pin);
            }
            DataContext = Appointment;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var errorText = "";
            if (Appointment.Doctor == null)
                errorText = "Doctor cannot be empty.";
            else if (Appointment.DateOfAppointment < DateTime.Today)
                errorText = "Date cannot be in the past.";
            else
            {
                if (IsEdit)
                    AppointmentService.UpdateAppointment(Appointment);
                else
                    AppointmentService.SaveAppointment(Appointment);
            }
            if (errorText.Length > 0)
            {
                new ErrorDialog(errorText).Show();
            }
            else
            {
                new AdminAppointmentWindow().Show();
                new SuccessDialog("Success!").Show();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e) => Close();
    }
}
