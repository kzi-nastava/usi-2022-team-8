﻿using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.Scheduling
{
    internal class AddExaminationCommand : CommandBase
    {
        private ExaminationTableViewModel _examinationTableViewModel;
        private Doctor _loggedDoctor;

        public AddExaminationCommand(ExaminationTableViewModel examinationTableViewModel, Doctor doctor)
        {
            _examinationTableViewModel = examinationTableViewModel;
            _loggedDoctor = doctor;
        }

        public override void Execute(object? parameter)
        {
            var window = DIContainer.GetService<AddExaminationDialog>();
            window.SetLoggedDoctor(_loggedDoctor);
            window.ShowDialog();
            _examinationTableViewModel.RefreshGrid();
        }
    }
}
        
