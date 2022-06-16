﻿using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.DoctorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.DoctorWindow
{
    internal class ExaminationsTableCommand : CommandBase
    {
        private Doctor _loggedDoctor;

        public ExaminationsTableCommand(Doctor doctor)
        {
            _loggedDoctor = doctor;
        }

        public override void Execute(object? parameter)
        {
            new ExaminationTable(_loggedDoctor).ShowDialog();
        }
    }
}