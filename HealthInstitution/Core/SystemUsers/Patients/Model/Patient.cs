using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Notifications.Model;

namespace HealthInstitution.Core.SystemUsers.Patients.Model;

public class Patient : User
{
    public List<AppointmentNotification> Notifications { get; set; }
    public Patient (UserType type, string username, string password, string name, string surname) : base(type, username, password, name, surname)
    {
        this.Blocked = BlockState.NotBlocked;
        this.Notifications= new List<AppointmentNotification>();
    }

    public Patient(UserDTO userDTO) : base(userDTO)
    {
        this.Blocked = BlockState.NotBlocked;
        this.Notifications = new List<AppointmentNotification>();
    }

    public override string? ToString()
    {
        return this.Name+" "+this.Surname;
    }
}