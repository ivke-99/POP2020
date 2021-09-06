using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
using SF_16_POP2020.Windows.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AdminDoctorFormWindow.xaml
    /// </summary>
    public partial class AdminDoctorFormWindow : Window
    {
        public List<EGender> EGenderTypes => Enum.GetValues(typeof(EGender)).Cast<EGender>().ToList();
        public bool IsEdit { set; get; }
        public Doctor Doctor { get; set; }
        public List<Clinic> Clinics { get; set; }
        public AdminDoctorFormWindow(Doctor doctor = null, bool isEdit = false)
        {
            Clinics = ClinicService.FindAll();
            IsEdit = isEdit;
            InitializeComponent();
            if (!isEdit)
            {
                Doctor = new Doctor
                {
                    Address = new Address()
                };
                tbPIN.IsEnabled = true;
            }
            else
            {
                Doctor = doctor;
                if(Doctor.Clinic != null)
                    Doctor.Clinic = Clinics.FirstOrDefault(e => e.Id == Doctor.Clinic.Id);
                tbPIN.IsEnabled = false;
                pbPassword.Password = Doctor.Password;
            }
            DataContext = Doctor;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var nameRegex = new Regex(@"(^\s*$)|([^a-zA-Z])|([0-9])");
            var pinRegex = new Regex(@"(^\s*$)|([^0-9])");
            var emailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
            string errorMessage = "";
            if (nameRegex.IsMatch(tbFirstName.Text.Trim()))
                errorMessage = "First name format not valid.";
            else if (nameRegex.IsMatch(tbLastName.Text.Trim()))
                errorMessage = "Last name format not valid.";
            else if (pinRegex.IsMatch(tbPIN.Text.Trim()) || tbPIN.Text.Trim().Length != 13)
                errorMessage = "PIN should be 13 characters.";
            else if (!emailRegex.IsMatch(tbEmail.Text.Trim()))
                errorMessage = "E-mail format not valid.";
            else if (pbPassword.Password.Trim().Length == 0 || pbPassword.Password.Contains(" ") || pbPassword.Password.Trim().Length > 20 || pbPassword.Password.Trim().Length <= 6)
                errorMessage = "Password should be between 6 and 20 chars, no spaces allowed.";
            else if (Doctor.Address.Street.Trim().Length == 0)
                errorMessage = "Street is empty.";
            else if (Doctor.Address.Number.Trim().Length == 0)
                errorMessage = "Number is empty.";
            else if (Doctor.Address.Town.Trim().Length == 0)
                errorMessage = "Town is empty.";
            else if (Doctor.Address.Country.Trim().Length == 0)
                errorMessage = "Country is empty.";
            else if (Doctor.Clinic == null)
                errorMessage = "Clinic cannot be empty.";
            else
            {
                Doctor.Password = pbPassword.Password;
                if (IsEdit)
                    DoctorService.UpdateDoctor(Doctor);
                else
                {
                    var ret = DoctorService.SaveDoctor(Doctor);
                    if (!ret)
                    {
                        errorMessage = "User with this pin already exists!";
                    }
                }
            }
            if (errorMessage.Length > 0)
            {
                new ErrorDialog(errorMessage).Show();
            }
            else
            {
                Close();
                new SuccessDialog("Success!").Show();
                new AdminDoctorWindow().Show();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e) => Close();
    }
}
