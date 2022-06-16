using HealthInstitution.Core;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Prescriptions;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.Prescriptions
{
    internal class AddPrescriptionDialogCommand : CommandBase
    {
        private AddPrescriptionDialogViewModel _addPrescriptionDialogViewModel;

        public AddPrescriptionDialogCommand(AddPrescriptionDialogViewModel addPrescriptionDialogViewModel)
        {
            _addPrescriptionDialogViewModel = addPrescriptionDialogViewModel;
        }

        public override void Execute(object? parameter)
        {
            var medicalRecord = _addPrescriptionDialogViewModel.MedicalRecord;
            try
            {
                var drug = _addPrescriptionDialogViewModel.GetDrug();
                if (PrescriptionService.IsPatientAlergic(medicalRecord, drug.Ingredients))
                    throw new Exception("Patient is alergic to drug ingredients");
                var hourlyRate = _addPrescriptionDialogViewModel.GetHourlyRate();
                var timeOfUse = _addPrescriptionDialogViewModel.GetTimeOfUse();
                var dailyDose = _addPrescriptionDialogViewModel.GetDailyDose();
                PrescriptionDTO prescriptionDTO = new PrescriptionDTO(dailyDose, timeOfUse, drug, hourlyRate);
                Prescription prescription = PrescriptionService.Add(prescriptionDTO);
                MedicalRecordService.AddPrescription(medicalRecord.Patient, prescription);
                System.Windows.MessageBox.Show("You have created the prescription!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
