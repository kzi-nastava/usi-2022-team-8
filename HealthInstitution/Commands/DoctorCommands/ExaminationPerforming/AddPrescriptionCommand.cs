using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.GUI.DoctorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.ExaminationPerforming
{
    internal class AddPrescriptionCommand : CommandBase
    {
        MedicalRecord _medicalRecord;
        public AddPrescriptionCommand(MedicalRecord medicalRecord)
        {
            _medicalRecord = medicalRecord;
        }

        public override void Execute(object? parameter)
        {
            var window = DIContainer.GetService<AddPrescriptionDialog>();
            window.SetMedicalRecord(_medicalRecord);
            window.ShowDialog();
        }
    }
}