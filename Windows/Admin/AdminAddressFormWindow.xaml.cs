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
    /// Interaction logic for AdminAddressFormWindow.xaml
    /// </summary>
    public partial class AdminAddressFormWindow : Window
    {
        public bool IsEdit { get; set; }
        public Address Address { get; set; }
        public AdminAddressFormWindow(Address address = null, bool isEdit = false)
        {
            InitializeComponent();
            IsEdit = isEdit;
            if (!isEdit)
                Address = new Address();
            else
                Address = address;
            DataContext = Address;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var errorText = "";
            if (Address.Street.Trim().Length == 0)
                errorText = "Street cannot be empty.";
            else if (Address.Number.Trim().Length == 0)
                errorText = "Number cannot be empty.";
            else if (Address.Town.Trim().Length == 0)
                errorText = "Town cannot be empty.";
            else if (Address.Country.Trim().Length == 0)
                errorText = "Country cannot be empty.";
            else
            {
                if (IsEdit)
                    AddressService.UpdateAddress(Address);
                else
                    AddressService.SaveAddress(Address.Street, Address.Number, Address.Town, Address.Country);
            }
            if (errorText.Length > 0)
                new ErrorDialog(errorText).Show();
            else
            {
                Close();
                new SuccessDialog("Success!").Show();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e) => Close();
    }
}
