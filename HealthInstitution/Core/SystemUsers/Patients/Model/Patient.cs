using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.MedicalRecords.Model;

namespace HealthInstitution.Core.SystemUsers.Patients.Model;

public class Patient : User
{
    // public MedicalRecord medicalRecord { get; set; } ??
    public Patient (UserType type, string username, string password, string name, string surname, BlockState blocked) : base(type, username, password, name, surname)
    {
        this.blocked = blocked;
    }

    public override string? ToString()
    {
        return this.name+" "+this.surname;
    }
}