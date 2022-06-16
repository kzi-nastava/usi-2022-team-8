using HealthInstitution.Core;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Referrals;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Referrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.Referrals
{
    internal class AddReferralDialogCommand : CommandBase
    {
        private AddReferralDialogViewModel _addReferralDialogViewModel;
        private object doctorComboBox;

        public AddReferralDialogCommand(AddReferralDialogViewModel addReferralDialogViewModel)
        {
            _addReferralDialogViewModel = addReferralDialogViewModel;
        }

        public override void Execute(object? parameter)
        {
            var patient = _addReferralDialogViewModel.Patient;
            var loggedDoctor = _addReferralDialogViewModel.Doctor;
            ReferralDTO referralDTO;
            if (_addReferralDialogViewModel.GetChosenType() == 0)
            {
                Doctor refferedDoctor = _addReferralDialogViewModel.GetDoctor();
                referralDTO = new ReferralDTO(ReferralType.SpecificDoctor, loggedDoctor, refferedDoctor, null);
            }
            else
            {
                SpecialtyType specialtyType = _addReferralDialogViewModel.GetSpecialtyType();
                referralDTO = new ReferralDTO(ReferralType.Specialty, loggedDoctor, null, specialtyType);
            }
            Referral referral = ReferralService.Add(referralDTO);
            MedicalRecordService.AddReferral(patient, referral);
            System.Windows.MessageBox.Show("You have created the referral!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
