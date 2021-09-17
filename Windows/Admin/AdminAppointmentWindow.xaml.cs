using SF_16_POP2020.Models;
using SF_16_POP2020.Services;
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
    /// Interaction logic for AdminAppointmentWindow.xaml
    /// </summary>
    public partial class AdminAppointmentWindow : Window
    {
        public AdminAppointmentWindow()
        {
            InitializeComponent();
            DG.ItemsSource = AppointmentService.FindAll();
            BtnDelete.IsEnabled = false;
            BtnEdit.IsEnabled = false;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            AppointmentService.DeleteAppointment(DG.SelectedItem as Appointment);
            DG.ItemsSource = AppointmentService.FindAll();
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            new AdminAppointmentFormWindow(DG.SelectedItem as Appointment, true).ShowDialog();
            DG.ItemsSource = AppointmentService.FindAll();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new AdminAppointmentFormWindow().ShowDialog();
            DG.ItemsSource = AppointmentService.FindAll();
        }


        private void DG_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0)
            {
                BtnDelete.IsEnabled = false;
                BtnEdit.IsEnabled = false;
            }
            else
            {
                BtnDelete.IsEnabled = true;
                BtnEdit.IsEnabled = true;
            }
        }
    }
}
