namespace HealthInstitution.Core.Appointments.Model;

public class Appointment
{
    public DateTime startTime { get; set; }
    public bool isFree { get; set; }

    // public Calendar calendar { get; set; }

    public Appointment(DateTime startTime, bool isFree)
    {
        this.startTime = startTime;
        this.isFree = isFree;
        //this.calendar = calendar;
    }
}