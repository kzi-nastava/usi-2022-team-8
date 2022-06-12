using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.RestRequests.Model;

public class RestRequest
{
    public int Id { get; set; }
    public Doctor Doctor { get; set; }
    public String Reason { get; set; }
    public DateOnly StartDate { get; set; }
    public int DaysDuration { get; set; }
    public RestRequestState State { get; set; }
    public bool IsUrgent { get; set; }

    public RestRequest(int id, Doctor doctor, string reason, DateOnly startDate, int daysDuration, RestRequestState state, bool isUrgent)
    {
        Id = id;
        Doctor = doctor;
        Reason = reason;
        StartDate = startDate;
        DaysDuration = daysDuration;
        State = state;
        IsUrgent = isUrgent;
    }
}

public enum RestRequestState
{
    OnHold,
    Accepted,
    Rejected
}