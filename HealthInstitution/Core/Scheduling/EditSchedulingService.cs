using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations;

namespace HealthInstitution.Core.Scheduling;

public class EditSchedulingService
{
    public static Examination GenerateRequestExamination(int id, ExaminationDTO examinationDTO)
    {
        examinationDTO.Validate();
        examinationDTO = SchedulingService.CheckExaminationAvailable(examinationDTO);
        Examination e = new Examination(examinationDTO);
        e.Id = id;
        return e;
    }

    public static void EditExamination(int id, ExaminationDTO examinationDTO)
    {
        examinationDTO.Validate();
        examinationDTO = SchedulingService.CheckExaminationAvailable(examinationDTO);
        Examination e = new Examination(examinationDTO);
        e.Id = id;
        ExaminationService.Update(id, examinationDTO);
    }
}