using HealthInstitution.Core.Appointments.Model;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.Examinations.Model;

namespace HealthInstitution.Core.ScheduleEditRequests.Model;

public class ScheduleEditRequest
{
    public int Id { get; set; }
    public Examination examination { get; set; }
    public int examinationId { get; set; }
    public RestRequestState state { get; set; }

    public ScheduleEditRequest(int id, Examination examination,int examinationId , RestRequestState state)
    {
        this.Id = id;
        this.examination = examination;
        this.state = state;
        this.examinationId = examinationId;
    }

    public void Accept()
    {
        state = RestRequestState.Accepted;

        //TODO add when u do exemination repository
    }

    public void Reject()
    {
        state = RestRequestState.Rejected;

        //TODO examination repository
    }
}