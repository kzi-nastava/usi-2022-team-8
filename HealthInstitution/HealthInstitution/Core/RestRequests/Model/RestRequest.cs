using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.RestRequests.Model;

public class RestRequest
{
    public Doctor doctor { get; set; }
    public String reason { get; set; }
    public DateTime startDate { get; set; }
    public int daysDuration { get; set; }
    public RestRequestState state { get; set; }
    public bool isUrgent { get; set; }
    
}

public enum RestRequestState
{
    OnHold,
    Accepted,
    Rejected
}