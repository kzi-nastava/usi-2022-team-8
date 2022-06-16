using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Referrals.Model;
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
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Referrals;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Referrals;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for AddReferralDialog.xaml
    /// </summary>
    public partial class AddReferralDialog : Window
    {
        private Patient _patient;
        private Doctor _doctor;
        public AddReferralDialog(Doctor doctor, Patient patient)
        {
            InitializeComponent();
            DataContext = new AddReferralDialogViewModel(patient, doctor);
        }
    }
}
