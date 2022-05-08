using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.RestRequests.Model;

namespace HealthInstitution.Core.ScheduleEditRequests.Model;

public class ScheduleEditRequest
{
    public int Id { get; set; }
    public Examination CurrentExamination { get; set; }
    public Examination NewExamination { get; set; }
    public RestRequestState State { get; set; }

    public ScheduleEditRequest(int id, Examination examination, int examinationId, RestRequestState state)
    {
        this.Id = id;
        this.NewExamination = examination;
        this.State = state;
        this.CurrentExamination = ExaminationRepository.GetInstance().ExaminationsById[examinationId];
    }
}