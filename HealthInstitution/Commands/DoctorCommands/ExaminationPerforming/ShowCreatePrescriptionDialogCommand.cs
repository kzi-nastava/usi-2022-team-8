using HealthInstitution.Core;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.GUI.DoctorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.ExaminationPerforming
{
    internal class ShowCreatePrescriptionDialogCommand : CommandBase
    {
        MedicalRecord _medicalRecord;
        public ShowCreatePrescriptionDialogCommand(MedicalRecord medicalRecord)
        {
            _medicalRecord = medicalRecord;
        }

        public override void Execute(object? parameter)
        {
            new AddPrescriptionDialog(_medicalRecord).ShowDialog();
        }
    }
}