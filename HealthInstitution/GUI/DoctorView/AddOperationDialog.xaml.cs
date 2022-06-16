using System.Windows;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for AddOperationDialog.xaml
    /// </summary>
    public partial class AddOperationDialog : Window
    {
        public AddOperationDialog(Doctor doctor)
        {
            InitializeComponent();
            DataContext = new AddOperationDialogViewModel(doctor);
        }
    }
}
