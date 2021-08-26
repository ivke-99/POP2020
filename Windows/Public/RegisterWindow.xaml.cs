using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
using SF_16_POP2020.Windows.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SF_16_POP2020.Windows.Public
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PIN { get; set; } = "";
        public string Email { get; set; } = "";
        public string Street { get; set; } = "";
        public string Number { get; set; } = "";
        public string Town { get; set; } = "";
        public string Country { get; set; } = "";
        public EGender Gender { get; set; }
        public List<EGender> EGenderTypes => Enum.GetValues(typeof(EGender)).Cast<EGender>().ToList();

        public RegisterWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new LoginWindow().Show();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var nameRegex = new Regex(@"(^\s*$)|([^a-zA-Z])|([0-9])");
            var pinRegex = new Regex(@"(^\s*$)|([^0-9])");
            var emailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
            string errorMessage;
            if (nameRegex.IsMatch(FirstName))
                errorMessage = "First name format not valid.";
            else if (nameRegex.IsMatch(LastName))
                errorMessage = "Last name format not valid.";
            else if (pinRegex.IsMatch(PIN) || PIN.Trim().Length != 13)
                errorMessage = "PIN should be 13 characters.";
            else if (!emailRegex.IsMatch(Email))
                errorMessage = "E-mail format not valid.";
            else if (pwField.Password.Trim().Length == 0 || pwField.Password.Contains(" ") || pwField.Password.Trim().Length > 20 || pwField.Password.Trim().Length <= 6)
                errorMessage = "Password should be between 6 and 20 chars, no spaces allowed.";
            else if (Street.Trim().Length == 0)
                errorMessage = "Street is empty.";
            else if (Number.Trim().Length == 0)
                errorMessage = "Number is empty.";
            else if (Town.Trim().Length == 0)
                errorMessage = "Town is empty.";
            else if (Country.Trim().Length == 0)
                errorMessage = "Country is empty.";
            else
            {
                var retval = UserService.SaveUser(FirstName.Trim(), LastName.Trim(), PIN.Trim(), Email.Trim(), Gender, pwField.Password.Trim(), Street.Trim(), Number.Trim(), Town.Trim(), Country.Trim());
                if (!retval)
                {
                    new ErrorDialog("PIN already exists.").Show();
                    return;
                }
                else
                {
                    new SuccessDialog("Successfully registered!").Show();
                    Close();
                    new LoginWindow().Show();
                    return;
                }

            }
            new ErrorDialog(errorMessage).Show();
        }
    }
}
