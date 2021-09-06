using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
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

namespace SF_16_POP2020.Windows.Admin
{
    /// <summary>
    /// Interaction logic for AdminPatientPrescriptionFormWindow.xaml
    /// </summary>
    public partial class AdminPatientPrescriptionFormWindow : Window
    {
        public Patient Patient { get; set; }
        public List<Prescription> AllPrescriptions { get; set; }
        public List<Prescription> PatientPrescriptions { get; set; }

        public AdminPatientPrescriptionFormWindow(Patient patient)
        {
            Patient = patient;
            AllPrescriptions = PrescriptionService.FindAllWherePatientIsNull(patient);
            PatientPrescriptions = PrescriptionService.FindAllByPatientPin(patient.Pin);
            InitializeComponent();
        }

        private void DeletePrescription_OnClick(object sender, RoutedEventArgs e)
        {
            var p = DGP.SelectedItem as Prescription;
            if (p != null)
            {
                PrescriptionService.DeletePatientPrescription(p, Patient);
                RefreshAll();
            }
        }

        private void AddPrescription_OnClick(object sender, RoutedEventArgs e)
        {
            var p = DGAll.SelectedItem as Prescription;
            if (p != null)
            {
                PrescriptionService.AddPatientPrescription(p, Patient);
                RefreshAll();
            }

        }

        private void RefreshAll()
        {
            AllPrescriptions = PrescriptionService.FindAllWherePatientIsNull(Patient);
            PatientPrescriptions = PrescriptionService.FindAllByPatientPin(Patient.Pin);
            DGAll.ItemsSource = AllPrescriptions;
            DGP.ItemsSource = PatientPrescriptions;
        }
    }
}
