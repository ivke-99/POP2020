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

namespace SF_16_POP2020.Windows.Public
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        #region UserAttributes
        public string KFirstName { get; set; } = "";
        public string KLastName { get; set; } = "";
        public string KEmail { get; set; } = "";
        public string KAddress { get; set; } = "";
        public ERole? KRole { get; set; }
        public IList<ERole> KRoleList => Enum.GetValues(typeof(ERole)).Cast<ERole>().ToList();
        #endregion
        #region ClinicAttributes
        public string CAddress { get; set; } = "";
        public string CName { get; set; } = "";
        #endregion
        #region PrescriptionAttributes
        public List<Doctor> PDoctorList => DoctorService.FindAll();
        public Doctor PDoctor { get; set; } = null;
        public string PDescription { get; set; } = "";
        #endregion
        public SearchWindow()
        {
            InitializeComponent();
            DataContext = this;
            UserDG.ItemsSource = UserService.FindAll();
            DGClinics.ItemsSource = ClinicService.FindAll();
            DGPrescriptions.ItemsSource = PrescriptionService.FindAll();
        }

        #region UserFunctions
        private void UsersQuery()
        {
            UserDG.ItemsSource = UserService.FindAllByFirstNameAndLastNameAndEmailAndAddress(KFirstName, KLastName, KEmail, KAddress, KRole);
        }

        private void UserRoleChanged(object sender, SelectionChangedEventArgs e) => UsersQuery();
        private void UsersSearch(object sender, RoutedEventArgs e) => UsersQuery();
        private void ClearUserCombobox(object sender, RoutedEventArgs e)
        {
            CBType.SelectedItem = null;
            KRole = null;
            UsersQuery();
        }
        #endregion

        #region ClinicFunctions
        private void ClinicQuery()
        {
            DGClinics.ItemsSource = ClinicService.FindAllByNameAndAddress(CName, CAddress);
        }
        private void ClinicsSearch(object sender, RoutedEventArgs e) => ClinicQuery();
        #endregion

        #region PrescriptionFunctions
        private void PrescriptionQuery()
        {
            DGPrescriptions.ItemsSource = PrescriptionService.FindAllByDescriptionAndDoctor(PDescription, PDoctor);
        }
        private void PrescriptionSearch(object sender, RoutedEventArgs e) => PrescriptionQuery(); 
        private void PrescriptionDoctorChanged(object sender, SelectionChangedEventArgs e) => PrescriptionQuery();
        private void PrescriptionClearDoctor(object sender, RoutedEventArgs e)
        {
            PDoctor = null;
            CBDoctor.SelectedItem = null;
            PrescriptionQuery();
        }
        #endregion

        private void ClickClear(object sender, RoutedEventArgs e)
        {
            KFirstName = "";
            KRole = null;
            KLastName = "";
            KEmail = "";
            KAddress = "";
            CAddress = "";
            CName = "";
            PDoctor = null;
            PDescription = "";
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbEmail.Text = "";
            tbAddress.Text = "";
            CBType.SelectedItem = null;
            tbClinicName.Text = "";
            tbClinicAddress.Text = "";
            tbPDescription.Text = "";
            CBDoctor.Text = "";
            UserDG.ItemsSource = UserService.FindAll();
            DGClinics.ItemsSource = ClinicService.FindAll();
            DGPrescriptions.ItemsSource = PrescriptionService.FindAll();
        }
    }
}
