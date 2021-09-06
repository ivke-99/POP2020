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
    /// Interaction logic for AdminDoctorWindow.xaml
    /// </summary>
    public partial class AdminDoctorWindow : Window
    {
        public AdminDoctorWindow()
        {
            InitializeComponent();
            DG.ItemsSource = DoctorService.FindAll();
            BtnDelete.IsEnabled = false;
            BtnEdit.IsEnabled = false;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            UserService.DeleteUser(DG.SelectedItem as User);
            DG.ItemsSource = DoctorService.FindAll();
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new AdminDoctorFormWindow(DG.SelectedItem as Doctor, true).Show();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new AdminDoctorFormWindow().Show();
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
