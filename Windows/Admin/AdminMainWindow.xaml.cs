using SF_16_POP2020.Misc;
using SF_16_POP2020.Windows.Shared;
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
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
            usernameLabel.Content = "Welcome " + UserState.LoggedUser.ToString() + "!";
        }

        private void LogoutMI_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            UserState.Logout();
            new LoginWindow().Show();
        }

        private void ProfileMI_OnClick(object sender, RoutedEventArgs e)
        {
            new ProfileWindow().Show();
        }

        private void AddressMI_OnClick(object sender, RoutedEventArgs e)
        {
            new AdminAddressWindow().Show();
        }

        private void PrescriptionMI_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            //new LoginWindow().Show();
        }

        private void DoctorMI_OnClick(object sender, RoutedEventArgs e)
        {
            new AdminDoctorWindow().Show();
        }

        private void ClinicMI_OnClick(object sender, RoutedEventArgs e)
        {
            new AdminClinicWindow().Show();
        }

        private void AppointmentMI_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            //new LoginWindow().Show();
        }

        private void PatientMI_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            //new LoginWindow().Show();
        }

        private void SearchMI_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            //new LoginWindow().Show();
        }
    }
}
