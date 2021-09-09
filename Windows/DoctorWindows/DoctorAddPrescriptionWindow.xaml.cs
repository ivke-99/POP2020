using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
using SF_16_POP2020.Windows.Dialog;
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
    /// Interaction logic for DoctorAddPrescriptionWindow.xaml
    /// </summary>
    public partial class DoctorAddPrescriptionWindow : Window
    {
        public Patient Patient { get; set; }
        public Prescription Prescription { get; set; }
        public List<Prescription> Prescriptions => PrescriptionService.FindAllByDoctor(UserState.LoggedUser.Pin);
        public DoctorAddPrescriptionWindow(Patient patient)
        {
            InitializeComponent();
            DataContext = this;
            Patient = patient;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new DoctorMainWindow().Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Prescription != null) {
                PrescriptionService.AddPatientPrescription(Prescription, Patient);
                Close();
                new DoctorMainWindow().Show();
                new SuccessDialog("Successfully added the prescription to the patient.").Show();
            }
            else
            {
                new ErrorDialog("Prescription not selected.").Show();
            }
        }


    }
}
