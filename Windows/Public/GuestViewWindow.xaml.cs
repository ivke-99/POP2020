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

namespace SF_16_POP2020.Windows.Public
{
    /// <summary>
    /// Interaction logic for GuestViewWindow.xaml
    /// </summary>
    public partial class GuestViewWindow : Window
    {
        private Doctor SelectedDoctor { get; set; } = null;
        private Address SelectedAddress { get; set; } = null;
        public GuestViewWindow()
        {
            InitializeComponent();
            DG.ItemsSource = ClinicService.FindAll();
            cbAddress.ItemsSource = AddressService.FindAll();
            cbDoctor.ItemsSource = DoctorService.FindAll();
        }

        private void CBSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var s = sender as ComboBox;
            if (s.Name == "cbDoctor")
            {
                SelectedDoctor = s.SelectedItem as Doctor;
            }
            else
            {
                SelectedAddress = cbAddress.SelectedItem as Address;
            }
            DG.ItemsSource = ClinicService.FindByDoctorAndAddress(SelectedDoctor, SelectedAddress);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new LoginWindow().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cbAddress.SelectedItem = null;
            cbDoctor.SelectedItem = null;
            DG.ItemsSource = ClinicService.FindAll();
        }
    }
}
