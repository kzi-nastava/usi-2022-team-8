﻿using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;
using System.Windows.Controls;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for AddExaminationDialog.xaml
    /// </summary>
    public partial class AddExaminationDialog : Window
    {
        private Patient _loggedPatient;
        private IDoctorService _doctorService;
        private IMedicalRecordService _medicalRecordService;
        private ISchedulingService _schedulingService;

        public AddExaminationDialog(IDoctorService doctorService,
                                    IMedicalRecordService medicalRecordService,
                                    ISchedulingService schedulingService)
        {
            InitializeComponent();
            _doctorService = doctorService;
            _medicalRecordService = medicalRecordService;
            _schedulingService = schedulingService;
        }

        public AddExaminationDialogViewModel SetLoggedPatient(Patient patient)
        {
            _loggedPatient = patient;
            var dc = new AddExaminationDialogViewModel(this, patient, _doctorService, _medicalRecordService, _schedulingService);
            DataContext = dc;
            return dc;
        }
    }
}