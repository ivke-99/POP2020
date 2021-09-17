using SF_16_POP2020.Misc;
using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
using SF_16_POP2020.Windows.Admin;
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
    /// Interaction logic for DoctorCreatePrescriptionWindow.xaml
    /// </summary>
    public partial class DoctorCreatePrescriptionWindow : Window
    {
        public string Description { get; set; } = "";
        public DoctorCreatePrescriptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Back_OnClick(object sender, RoutedEventArgs e) => Close();

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
            if (Description.Trim().Length == 0)
            {
                new ErrorDialog("Description cannot be empty.").Show();
                return;
            }
            var pres = new Prescription { Doctor = new Doctor { Pin = UserState.LoggedUser.Pin }, Description = Description };
            PrescriptionService.SavePrescription(pres);
            Close();
            new SuccessDialog("Success!").Show();
        }
    }
}
