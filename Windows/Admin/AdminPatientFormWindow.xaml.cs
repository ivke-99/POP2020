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
    /// Interaction logic for AdminPatientFormWindow.xaml
    /// </summary>
    public partial class AdminPatientFormWindow : Window
    {
        public List<EGender> EGenderTypes => Enum.GetValues(typeof(EGender)).Cast<EGender>().ToList();
        public bool IsEdit { set; get; }
        public Patient Patient { get; set; }
        public AdminPatientFormWindow(Patient patient = null, bool isEdit = false)
        {
            IsEdit = isEdit;
            InitializeComponent();
            if (!isEdit)
            {
                Patient = new Patient
                {
                    Address = new Address()
                };
                tbPIN.IsEnabled = true;
            }
            else
            {
                Patient = patient;
                tbPIN.IsEnabled = false;
                pbPassword.Password = Patient.Password;
            }
            DataContext = Patient;
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
            else if (Patient.Address.Street.Trim().Length == 0)
                errorMessage = "Street is empty.";
            else if (Patient.Address.Number.Trim().Length == 0)
                errorMessage = "Number is empty.";
            else if (Patient.Address.Town.Trim().Length == 0)
                errorMessage = "Town is empty.";
            else if (Patient.Address.Country.Trim().Length == 0)
                errorMessage = "Country is empty.";
            else
            {
                Patient.Password = pbPassword.Password;
                if (IsEdit)
                    UserService.UpdateUser(Patient);
                else
                {
                    var ret = UserService.SaveUser(Patient);
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
                new AdminPatientWindow().Show();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e) => Close();
    }
}
