using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
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

namespace SF_16_POP2020.Windows.DoctorWindows
{
    /// <summary>
    /// Interaction logic for DoctorMainWindow.xaml
    /// </summary>
    public partial class DoctorMainWindow : Window
    {
        public DoctorMainWindow()
        {
            InitializeComponent();
            DGClinics.ItemsSource = ClinicService.FindAll();
            DGAppointments.ItemsSource = AppointmentService.FindAllByDoctor(UserState.LoggedUser.Pin);
            DGPatients.ItemsSource = PatientService.FindAllByDoctorAndAppointment(UserState.LoggedUser.Pin);
            DGPrescriptionsDoctor.ItemsSource = PrescriptionService.FindAllByDoctor(UserState.LoggedUser.Pin);
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

        private void DGAppointments_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Doctor")
                e.Cancel = true;
        }

        private void SearchMI_OnClick(object sender, RoutedEventArgs e) => new SearchWindow().Show();

        private void DGAppointments_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg.SelectedItem != null)
            {
                var appointment = dg.SelectedItem as Appointment;
                if (appointment.Status == EStatus.AVAILABLE)
                    BtnDelete.IsEnabled = true;
            }
            else
                BtnDelete.IsEnabled = false;
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (DGAppointments.SelectedItem != null)
            {
                AppointmentService.DeleteAppointment(DGAppointments.SelectedItem as Appointment);
                DGAppointments.ItemsSource = AppointmentService.FindAllByDoctor(UserState.LoggedUser.Pin);
            }
        }

        private void DGPatients_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ListOfAppointments":
                case "Password":
                    e.Cancel = true;
                    break;
            }
        }

        private void AddPrescription_Click(object sender, RoutedEventArgs e)
        {
            var sel = DGPatients.SelectedItem as Patient;
            if (sel != null)
                new DoctorAddPrescriptionWindow(sel).Show();
        }

        private void MedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            if (DGPatients.SelectedItem is Patient selected)
                new DoctorViewPatientRecordWindow(selected).Show();
        }

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
            new DoctorCreateAppointmentWindow().Show();
        }

        private void CreatePrescription_OnClick(object sender, RoutedEventArgs e)
        {
            new DoctorCreatePrescriptionWindow().Show();
        }
    }
}
