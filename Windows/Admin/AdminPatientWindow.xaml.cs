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
    /// Interaction logic for AdminPatientWindow.xaml
    /// </summary>
    public partial class AdminPatientWindow : Window
    {
        public AdminPatientWindow()
        {
            InitializeComponent();
            DG.ItemsSource = PatientService.FindAll();
            BtnDelete.IsEnabled = false;
            BtnEdit.IsEnabled = false;
            BtnPrescription.IsEnabled = false;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            UserService.DeleteUser(DG.SelectedItem as User);
            DG.ItemsSource = PatientService.FindAll();
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new AdminPatientFormWindow(DG.SelectedItem as Patient, true).ShowDialog();
            DG.ItemsSource = PatientService.FindAll();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new AdminPatientFormWindow().ShowDialog();
            DG.ItemsSource = PatientService.FindAll();
        }


        private void DG_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0)
            {
                BtnDelete.IsEnabled = false;
                BtnEdit.IsEnabled = false;
                BtnPrescription.IsEnabled = false;
            }
            else
            {
                BtnDelete.IsEnabled = true;
                BtnEdit.IsEnabled = true;
                BtnPrescription.IsEnabled = true;
            }
        }
        private void DG_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ListOfPrescriptions")
                e.Cancel = true;
        }

        private void Prescription_OnClick(object sender, RoutedEventArgs e)
        {
            new AdminPatientPrescriptionFormWindow(DG.SelectedItem as Patient).ShowDialog();
        }
    }
}
