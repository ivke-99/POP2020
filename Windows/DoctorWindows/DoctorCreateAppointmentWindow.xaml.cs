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
    /// Interaction logic for DoctorCreateAppointmentWindow.xaml
    /// </summary>
    public partial class DoctorCreateAppointmentWindow : Window
    {
        public DoctorCreateAppointmentWindow()
        {
            InitializeComponent();
        }
        private void Back_OnClick(object sender, RoutedEventArgs e) => Close();
        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
            var errorText = "";
            if (DatePicker.SelectedDate != null)
            {
                if (DatePicker.SelectedDate < DateTime.Today)
                    errorText = "Date cannot be in the past.";
                else
                {
                    var app = new Appointment()
                    {
                        DateOfAppointment = DatePicker.SelectedDate.Value,
                        Doctor = new Doctor { Pin = UserState.LoggedUser.Pin },
                        Status = EStatus.AVAILABLE
                    };
                    AppointmentService.SaveAppointment(app);
                }
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
    }

    
}
