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
    /// Interaction logic for AdminClinicFormWindow.xaml
    /// </summary>
    public partial class AdminClinicFormWindow : Window
    {
        public bool IsEdit { set; get; }
        public Clinic Clinic { get; set; }
        public AdminClinicFormWindow(Clinic c = null, bool isEdit = false)
        {
            IsEdit = isEdit;
            InitializeComponent();
            if (!isEdit)
            {
                Clinic = new Clinic
                {
                    Address = new Address()
                };
            }
            else
            {
                Clinic = c;
                if (Clinic.Address == null)
                {
                    Clinic.Address = new Address();
                }
            }
            DataContext = Clinic;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var textval = "";
            if (Clinic.Address.Street.Trim().Length == 0)
                textval = "Street cannot be empty.";
            else if (Clinic.Address.Number.Trim().Length == 0)
                textval = "Number cannot be empty.";
            else if (Clinic.Address.Town.Trim().Length == 0)
                textval = "Town cannot be empty.";
            else if (Clinic.Address.Country.Trim().Length == 0)
                textval = "Country cannot be empty.";
            else if (Clinic.Name.Trim().Length == 0)
                textval = "Name cannot be empty.";
            else
            {
                if (IsEdit)
                    ClinicService.UpdateClinic(Clinic);
                else
                    ClinicService.SaveClinic(Clinic);
            }
            if (textval.Length > 0)
                new ErrorDialog(textval).Show();
            else
            {
                Close();
                new SuccessDialog("Success!").Show();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e) => Close();
    }
}
