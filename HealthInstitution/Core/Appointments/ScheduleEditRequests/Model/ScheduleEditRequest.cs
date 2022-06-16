using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.RestRequests.Model;

namespace HealthInstitution.Core.ScheduleEditRequests.Model;

public class ScheduleEditRequest
{
    public int Id { get; set; }
    public Examination CurrentExamination { get; set; }
    public Examination NewExamination { get; set; }
    public Operation CurrentOperation { get; set; }
    public Operation NewOperation { get; set; }
    public RestRequestState State { get; set; }

    public ScheduleEditRequest(int id, Examination examination, int examinationId, Examination currentExamination, RestRequestState state)
    {
        this.Id = id;
        this.NewExamination = examination;
        this.State = state;
        this.CurrentExamination = currentExamination;
        this.CurrentOperation = null;
        this.NewOperation = null;
    }

    public ScheduleEditRequest(int id, Examination currentExamination, Examination newExamination, RestRequestState state)
    {
        this.Id = id;
        this.CurrentExamination = currentExamination;
        this.NewExamination = newExamination;
        this.State = state;
        this.CurrentOperation = null;
        this.NewOperation = null;
    }
    public ScheduleEditRequest(int id, Operation currentOperation, Operation newOperation, RestRequestState state)
    {
        this.Id = id;
        this.CurrentOperation = currentOperation;
        this.NewOperation = newOperation;
        this.State = state;
        this.CurrentExamination = null;
        this.NewExamination = null;
    }
}