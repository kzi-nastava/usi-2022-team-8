using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Notifications.Model;

namespace HealthInstitution.Core.SystemUsers.Patients.Model;

public class Patient : User
{
    public List<Notification> Notifications { get; set; }
    // public MedicalRecord medicalRecord { get; set; } ??
    public Patient (UserType type, string username, string password, string name, string surname, BlockState blocked) : base(type, username, password, name, surname)
    {
        this.Blocked = blocked;
        this.Notifications= new List<Notification>();
    }

    public override string? ToString()
    {
        return this.Name+" "+this.Surname;
    }
}