using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;

namespace HealthInstitution.Core.ScheduleEditRequests.Model;

public class ScheduleEditRequest
{
    public int Id { get; set; }
    public Examination currentExamination { get; set; }
    public Examination newExamination { get; set; }
    public RestRequestState state { get; set; }

    public ScheduleEditRequest(int id, Examination examination, int examinationId, RestRequestState state)
    {
        this.Id = id;
        this.newExamination = examination;
        this.state = state;
        this.currentExamination = ExaminationRepository.GetInstance().examinationsById[examinationId];
    }
}