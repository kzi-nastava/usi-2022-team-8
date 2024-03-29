﻿using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
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

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for ExaminationTable.xaml
    /// </summary>
    public partial class ExaminationTable : Window
    {
        Doctor _loggedDoctor;
        IExaminationService _examinationService;
        IDoctorService _doctorService;
        public ExaminationTable(IExaminationService examinationService, IDoctorService doctorService)
        {
            InitializeComponent();
            _examinationService = examinationService;
            _doctorService = doctorService;
        }

        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
            DataContext = new ExaminationTableViewModel(doctor, _doctorService, _examinationService);
        }
    }
}
