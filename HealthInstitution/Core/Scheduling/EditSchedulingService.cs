using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations;

namespace HealthInstitution.Core.Scheduling;

public class EditSchedulingService : IEditSchedulingService
{
    ISchedulingService _schedulingService;
    IExaminationService _examinationService;

    public EditSchedulingService(ISchedulingService schedulingService, IExaminationService examinationService)
    {
        _schedulingService = schedulingService;
        _examinationService = examinationService;
    }

    public Examination GenerateRequestExamination(int id, ExaminationDTO examinationDTO)
    {
        _examinationService.Validate(examinationDTO);
        examinationDTO = _schedulingService.CheckExaminationAvailable(examinationDTO);
        Examination e = new Examination(examinationDTO);
        e.Id = id;
        return e;
    }

    public void EditExamination(int id, ExaminationDTO examinationDTO)
    {
        _examinationService.Validate(examinationDTO);
        examinationDTO = _schedulingService.CheckExaminationAvailable(examinationDTO);
        Examination e = new Examination(examinationDTO);
        e.Id = id;
        _examinationService.Update(id, examinationDTO);
    }
}