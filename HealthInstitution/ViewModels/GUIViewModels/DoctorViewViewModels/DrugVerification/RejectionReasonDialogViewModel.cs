using HealthInstitution.Commands.DoctorCommands.DrugVerification;
using HealthInstitution.Commands.DoctorCommands.Scheduling;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.DrugVerification
{
    public class RejectionReasonDialogViewModel : ViewModelBase
    {
        public Drug SelectedDrug { get; set; }

        private string _rejectionReason;

        public string RejectionReason
        {
            get
            {
                return _rejectionReason;
            }
            set
            {
                _rejectionReason = value;
                OnPropertyChanged(nameof(RejectionReason));
            }
        }
        public ICommand RejectionReasonCommand { get; }
        IDrugVerificationService _drugVerificationService;
        public RejectionReasonDialogViewModel(Drug drug, IDrugVerificationService drugVerificationService)
        {
            SelectedDrug = drug;
            _drugVerificationService = drugVerificationService;
            RejectionReasonCommand = new RejectionReasonCommand(this, drugVerificationService);
        }
    }
}
