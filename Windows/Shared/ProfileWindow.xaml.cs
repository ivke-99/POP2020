using SF_16_POP2020.Misc;
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

namespace SF_16_POP2020.Windows.Shared
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        public User User { get; set; }
        public List<EGender> EGenderTypes => Enum.GetValues(typeof(EGender)).Cast<EGender>().ToList();
        public ProfileWindow()
        {
            InitializeComponent();
            User = UserService.FindByPin(UserState.LoggedUser.Pin);
            pbPassword.Password = User.Password;
            DataContext = User;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            var nameRegex = new Regex(@"(^\s*$)|([^a-zA-Z])|([0-9])");
            var emailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
            string errorMessage = "";
            if (nameRegex.IsMatch(User.FirstName))
                errorMessage = "First name format not valid.";
            else if (nameRegex.IsMatch(User.LastName))
                errorMessage = "Last name format not valid.";
            else if (!emailRegex.IsMatch(User.Email))
                errorMessage = "E-mail format not valid.";
            else if (pbPassword.Password.Trim().Length == 0 || pbPassword.Password.Contains(" ") || pbPassword.Password.Trim().Length > 20 || pbPassword.Password.Trim().Length <= 6)
                errorMessage = "Password should be between 6 and 20 chars, no spaces allowed.";
            else if (User.Address.Street.Trim().Length == 0)
                errorMessage = "Street is empty.";
            else if (User.Address.Number.Trim().Length == 0)
                errorMessage = "Number is empty.";
            else if (User.Address.Town.Trim().Length == 0)
                errorMessage = "Town is empty.";
            else if (User.Address.Country.Trim().Length == 0)
                errorMessage = "Country is empty.";
            if (errorMessage.Length > 1)
            {
                new ErrorDialog(errorMessage).Show();
                return;
            }
            var addressId = AddressService.SaveAddressIfNotExists(User.Address);
            if (addressId.HasValue) User.Address.Id = (int)addressId;
            User.Password = pbPassword.Password;
            UserService.UpdateUser(User);
            new SuccessDialog("Successfully updated profile!").Show();
            Close();
        }
    }
}
