using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels.Scheduling
{
    public class OperationViewModel : ViewModelBase
    {
        private Operation _operation;

        public string RoomNo => _operation.Room.Number.ToString();
        public ExaminationStatus Status => _operation.Status;
        public DateTime Appointment => _operation.Appointment;
        public string Doctor => _operation.Doctor.Name + " " + _operation.Doctor.Surname;
        public string Patient => _operation.MedicalRecord.Patient.Name + " " + _operation.MedicalRecord.Patient.Surname;
        public string Duration => _operation.Duration.ToString();
        public OperationViewModel(Operation operation)
        {
            _operation = operation;
        }
    }
}
