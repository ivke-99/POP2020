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

namespace SF_16_POP2020.Windows.Admin
{
    /// <summary>
    /// Interaction logic for AdminPrescriptionFormWindow.xaml
    /// </summary>
    public partial class AdminPrescriptionFormWindow : Window
    {
        public bool IsEdit { set; get; }
        public Prescription Prescription { get; set; }

        public List<Doctor> Doctors { get; set; }
        public AdminPrescriptionFormWindow(Prescription prescription = null, bool isEdit = false)
        {
            InitializeComponent();
            Doctors = DoctorService.FindAll();
            IsEdit = isEdit;
            if (!isEdit)
                Prescription = new Prescription();
            else
            {
                Prescription = prescription;
                Prescription.Doctor = Doctors.FirstOrDefault(e => e.Pin == Prescription.Doctor.Pin);
            }
            DataContext = Prescription;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var errorText = "";
            if (Prescription.Description.Trim().Length == 0)
                errorText = "Description cannot be empty.";
            else if (Prescription.Doctor == null)
                errorText = "Doctor cannot be empty.";
            else
            {
                if (IsEdit)
                    PrescriptionService.UpdatePrescription(Prescription);
                else
                    PrescriptionService.SavePrescription(Prescription);
            }
            if (errorText.Length > 0)
            {
                new ErrorDialog(errorText).Show();
            }
            else
            {
                Close();
                new SuccessDialog("Success!").Show();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e) => Close();
    }
}
