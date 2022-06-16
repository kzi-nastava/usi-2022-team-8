using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for OperationTable.xaml
    /// </summary>
    public partial class OperationTable : Window
    {
        IOperationService _operationService;
        IDoctorService _doctorService;
        Doctor _loggedDoctor;
        public OperationTable(IOperationService operationService, IDoctorService doctorService)
        {
            InitializeComponent();
            _operationService = operationService;
            _doctorService = doctorService;
        }

        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
            DataContext = new OperationTableViewModel(doctor,_doctorService,_operationService);
        }
    }
}
