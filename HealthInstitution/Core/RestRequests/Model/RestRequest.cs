using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.RestRequests.Model;

public class RestRequest
{
    public int Id { get; set; }
    public Doctor Doctor { get; set; }
    public String Reason { get; set; }
    public DateTime StartDate { get; set; }
    public int DaysDuration { get; set; }
    public RestRequestState State { get; set; }
    public bool IsUrgent { get; set; }
    public string RejectionReason { get; set; }

    public RestRequest(int id, Doctor doctor, string reason, DateTime startDate, int daysDuration, RestRequestState state, bool isUrgent, string rejectionReason)
    {
        Id = id;
        Doctor = doctor;
        Reason = reason;
        StartDate = startDate;
        DaysDuration = daysDuration;
        State = state;
        IsUrgent = isUrgent;
        RejectionReason = rejectionReason;
    }

    public RestRequest(RestRequestDTO restRequestDTO)
    {
        Doctor = restRequestDTO.Doctor;
        Reason = restRequestDTO.Reason;
        StartDate = restRequestDTO.StartDate;
        DaysDuration = restRequestDTO.DaysDuration;
        State = restRequestDTO.State;
        IsUrgent = restRequestDTO.IsUrgent;
        RejectionReason = restRequestDTO.RejectionReason;
    }
}

public enum RestRequestState
{
    OnHold,
    Accepted,
    Rejected
}