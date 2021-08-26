using MahApps.Metro.Controls;
using SF_16_POP2020.Misc;
using SF_16_POP2020.Services;
using SF_16_POP2020.Windows.Dialog;
using SF_16_POP2020.Windows.Public;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SF_16_POP2020
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.FindUserByPinAndPassword(pinField.Text.Trim(), passwordField.Password.Trim());
            Window win = null;
            if (user != null)
            {
                switch (user.Role)
                {
                    case Models.ERole.ADMIN:
                        //win = new AdminMainWindow();
                        break;
                    case Models.ERole.DOCTOR:
                        //win = new DoctorMainWindow();
                        break;
                    case Models.ERole.PATIENT:
                        //win = new PatientMainWindow();
                        break;
                }
                Hide();
                win.Show();
            }
            else
                new ErrorDialog("Wrong pin or password.").Show();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new RegisterWindow().Show();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //var window = new SearchClick();
            Hide();
            // window.Show();
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            // var window = new GuestViewWindow();
            Hide();
            // window.Show();
        }
    }
}
