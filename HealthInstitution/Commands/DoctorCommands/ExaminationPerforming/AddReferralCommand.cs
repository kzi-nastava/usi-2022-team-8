using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.DoctorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.ExaminationPerforming
{
    internal class AddReferralCommand : CommandBase
    {
        MedicalRecord _medicalRecord;
        Doctor _loggedDoctor;
        public AddReferralCommand(Doctor doctor, MedicalRecord medicalRecord)
        {
            _loggedDoctor = doctor;
            _medicalRecord = medicalRecord;
        }

        public override void Execute(object? parameter)
        {
            var window = DIContainer.GetService<AddReferralDialog>();
            window.SetRefferalFields(_loggedDoctor, _medicalRecord.Patient);
            window.ShowDialog();
        }
    }
}