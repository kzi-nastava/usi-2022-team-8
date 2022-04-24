using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.Appointments.Model;

namespace HealthInstitution.Core.SystemUsers.Doctors.Model;

public class Doctor : User
{
    public SpecialtyType specialty { get; set; }
    public List<Examination> examinations { get; set; }
    public List<Operation> operations { get; set; }
    public List<RestRequest> restRequests { get; set; }
    public List<Appointment> appointments { get; set; }

    public Doctor(UserType type, string username, string password, string name, string surname, SpecialtyType specialty) : base(type, username, password, name, surname)
    {
        this.specialty = specialty;
    }
}

public enum SpecialtyType
{
    GeneralPractitioner,
    Surgeon,
    Pediatrician,
    Radiologist
}