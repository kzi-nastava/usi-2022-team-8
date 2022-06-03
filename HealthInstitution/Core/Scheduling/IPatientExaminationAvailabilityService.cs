using HealthInstitution.Core.Examinations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public interface IPatientExaminationAvailabilityService
    {
        public void CheckIfPatientHasExaminations(ExaminationDTO examinationDTO);
        public void CheckIfPatientHasOperations(ExaminationDTO examinationDTO);
        public void CheckIfPatientIsAvailable(ExaminationDTO examinationDTO);
    }
}
