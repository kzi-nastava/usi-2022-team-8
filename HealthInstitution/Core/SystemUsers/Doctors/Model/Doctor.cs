using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;

namespace HealthInstitution.Core.SystemUsers.Doctors.Model;

public class Doctor : User
{
    public SpecialtyType specialty { get; set; }
    public List<Examination> examinations { get; set; }
    public List<Operation> operations { get; set; }
    public List<RestRequest> restRequests { get; set; }

    public Doctor(string username, string password, string name, string surname, SpecialtyType specialty, List<Examination> examinations, List<Operation> operations) : base(UserType.Doctor, username, password, name, surname)
    {
        this.specialty = specialty;
        this.examinations = examinations;
        this.operations = operations;
        this.restRequests = new List<RestRequest>();
    }
}

public enum SpecialtyType
{
    GeneralPractitioner,
    Surgeon,
    Pediatrician,
    Radiologist
}