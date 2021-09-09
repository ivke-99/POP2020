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

namespace SF_16_POP2020.Windows.DoctorWindows
{
    /// <summary>
    /// Interaction logic for DoctorViewPatientRecordWindow.xaml
    /// </summary>
    public partial class DoctorViewPatientRecordWindow : Window
    {
        public DoctorViewPatientRecordWindow(Patient p)
        {
            InitializeComponent();
            DG.ItemsSource = PrescriptionService.FindAllByPatientPin(p.Pin);
        }
    }
}
