using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.RestRequests.Model;

public class RestRequest
{
    public Doctor Doctor { get; set; }
    public String Reason { get; set; }
    public DateOnly StartDate { get; set; }
    public int DaysDuration { get; set; }
    public RestRequestState state { get; set; }
    public bool IsUrgent { get; set; }
    
}

public enum RestRequestState
{
    OnHold,
    Accepted,
    Rejected
}