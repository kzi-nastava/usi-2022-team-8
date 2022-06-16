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
        IMedicalRecordService _medicalRecordService;
        IPrescriptionService _prescriptionService;
        public AddPrescriptionDialogCommand(AddPrescriptionDialogViewModel addPrescriptionDialogViewModel, IPrescriptionService prescriptionService, IMedicalRecordService medicalRecordService)
        {
            _addPrescriptionDialogViewModel = addPrescriptionDialogViewModel;
            _medicalRecordService = medicalRecordService;
            _prescriptionService = prescriptionService;
        }

        public override void Execute(object? parameter)
        {
            var medicalRecord = _addPrescriptionDialogViewModel.MedicalRecord;
            try
            {
                PrescriptionDTO prescriptionDTO = CreatePrescriptionDTOFromInputData();
                Prescription prescription = _prescriptionService.Add(prescriptionDTO);
                _medicalRecordService.AddPrescription(medicalRecord.Patient, prescription);
                System.Windows.MessageBox.Show("You have created the prescription!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public PrescriptionDTO CreatePrescriptionDTOFromInputData()
        {
            var drug = _addPrescriptionDialogViewModel.GetDrug();
            if (_medicalRecordService.IsPatientAlergic(_addPrescriptionDialogViewModel.MedicalRecord, drug.Ingredients))
                throw new Exception("Patient is alergic to drug ingredients");
            var hourlyRate = _addPrescriptionDialogViewModel.GetHourlyRate();
            var timeOfUse = _addPrescriptionDialogViewModel.GetTimeOfUse();
            var dailyDose = _addPrescriptionDialogViewModel.GetDailyDose();
            return new PrescriptionDTO(dailyDose, timeOfUse, drug, hourlyRate);
        }
    }
}
