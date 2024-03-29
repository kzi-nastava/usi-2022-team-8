﻿using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.SchedulePerforming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.ExaminationPerforming
{
    internal class FinishExaminationCommand : CommandBase
    {
        private PerformExaminationDialogViewModel _performExaminationDialogViewModel;
        private Examination _selectedExamination;
        IMedicalRecordService _medicalRecordService;
        IExaminationService _examinationService;

        public FinishExaminationCommand(PerformExaminationDialogViewModel performExaminationDialogViewModel, Examination examination, IMedicalRecordService medicalRecordService, IExaminationService examinationService)
        {
            _performExaminationDialogViewModel = performExaminationDialogViewModel;
            _selectedExamination = examination;
            _medicalRecordService = medicalRecordService;
            _examinationService = examinationService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                MedicalRecordDTO medicalRecordDTO = CreateMedicalRecordDTOFromInputData();
                _medicalRecordService.Update(medicalRecordDTO);
                _examinationService.Complete(_selectedExamination, _performExaminationDialogViewModel.Anamnesis);
                System.Windows.MessageBox.Show("You have finished the examination!", "Congrats", MessageBoxButton.OK, MessageBoxImage.Information);
                //this.Close();
                var window = DIContainer.GetService<ConsumedEquipmentDialog>();
                window.SetSelectedRoom(_selectedExamination.Room);
                window.ShowDialog();
            }
            catch
            {
                System.Windows.MessageBox.Show("You haven't fulfilled it the right way!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private MedicalRecordDTO CreateMedicalRecordDTOFromInputData()
        {
            var vm = _performExaminationDialogViewModel;
            double height = Double.Parse(vm.Height);
            double weight = Double.Parse(vm.Weight);
            List<String> previousIllnesses = new List<String>();
            foreach (String illness in vm.PreviousIllnesses)
            {
                previousIllnesses.Add(illness);
            };
            List<String> allergens = new List<String>();
            foreach (String allergen in vm.Allergens)
            {
                allergens.Add(allergen);
            }
            List<Prescription> prescriptions = vm.MedicalRecord.Prescriptions;
            List<Referral> referrals = vm.MedicalRecord.Referrals;
            return new MedicalRecordDTO(height, weight, previousIllnesses, allergens, _selectedExamination.MedicalRecord.Patient, prescriptions, referrals);
        }

    }
}