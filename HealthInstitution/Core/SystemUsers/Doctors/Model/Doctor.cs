using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;

namespace HealthInstitution.Core.SystemUsers.Doctors.Model;

public class Doctor : User
{
    public SpecialtyType Specialty { get; set; }
    public List<Examination> Examinations { get; set; }
    public List<Operation> Operations { get; set; }
    public List<RestRequest> RestRequests { get; set; }
    public List<Notification> Notifications { get; set; }
    public double AvgRating { get; set; }

    public Doctor(string username, string password, string name, string surname, SpecialtyType specialty) : base(UserType.Doctor, username, password, name, surname)
    {
        this.Specialty = specialty;
        this.Examinations = new List<Examination>();
        this.Operations = new List<Operation>();
        this.RestRequests = new List<RestRequest>();
        this.Notifications = new List<Notification>();
        AvgRating = 0;
    }

    public override string? ToString()
    {
        return this.Name + " " + this.Surname;
    }
}

public enum SpecialtyType
{
    GeneralPractitioner,
    Surgeon,
    Pediatrician,
    Radiologist
}