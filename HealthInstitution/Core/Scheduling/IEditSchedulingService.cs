using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.Examinations.Model;

namespace HealthInstitution.Core.Scheduling;

public interface IEditSchedulingService
{
    public Examination GenerateRequestExamination(int id, ExaminationDTO examinationDTO);

    public void EditExamination(int id, ExaminationDTO examinationDTO);
}