using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
using SF_16_POP2020.Windows.Dialog;
using SF_16_POP2020.Windows.Public;
using SF_16_POP2020.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SF_16_POP2020.Windows.PatientWindows
{
    /// <summary>
    /// Interaction logic for PatientMainWindow.xaml
    /// </summary>
    public partial class PatientMainWindow : Window
    {
        public PatientMainWindow()
        {
            InitializeComponent();
            DGClinics.ItemsSource = ClinicService.FindAll();
            DGPrescriptions.ItemsSource = PrescriptionService.FindAllByPatientPin(UserState.LoggedUser.Pin);
            DGAppointments.ItemsSource = AppointmentService.FindAll();
        }

        private void LogoutMI_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserState.Logout();
            new LoginWindow().Show();
        }

        private void ProfileMI_Click(object sender, RoutedEventArgs e) => new ProfileWindow().Show();
        
        private void DGClinics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = DGClinics.SelectedItem as Clinic;
            if (selection != null)
                DGDoctors.ItemsSource = DoctorService.FindAllByClinic(selection);
        }

        private void DGDoctors_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Role":
                case "Clinic":
                case "Password":
                    e.Cancel = true;
                    break;
            }
        }

        private void Appointments_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg.SelectedItem != null)
            {
                var appointment = dg.SelectedItem as Appointment;
                if (appointment.Status == EStatus.AVAILABLE)
                    BtnBook.IsEnabled = true;
            }
            else
                BtnBook.IsEnabled = false;
        }

        private void Book_OnClick(object sender, RoutedEventArgs e)
        {
            if (DGAppointments.SelectedItem != null)
            {
                AppointmentService.BookAppointment(DGAppointments.SelectedItem as Appointment, UserState.LoggedUser.Pin);
                new SuccessDialog("Successfully booked your desired appointment!").Show();
                DGAppointments.ItemsSource = AppointmentService.FindAll();
            }
        }

        private void DGAppointments_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Patient")
                e.Cancel = true;
        }

        private void SearchMI_OnClick(object sender, RoutedEventArgs e) => new SearchWindow().Show();
    }
}
