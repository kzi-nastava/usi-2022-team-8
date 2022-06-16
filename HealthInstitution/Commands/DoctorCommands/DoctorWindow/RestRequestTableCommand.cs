using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.DoctorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.DoctorWindow
{
    internal class RestRequestTableCommand : CommandBase
    {
        private Doctor _loggedDoctor;

        public RestRequestTableCommand(Doctor doctor)
        {
            _loggedDoctor = doctor;
        }

        public override void Execute(object? parameter)
        {
            var window = DIContainer.GetService<RestRequestTable>();
            window.SetLoggedDoctor(_loggedDoctor);
            window.ShowDialog();
        }
    }
}
