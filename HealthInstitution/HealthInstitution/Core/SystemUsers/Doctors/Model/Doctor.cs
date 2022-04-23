using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;

namespace HealthInstitution.Core.SystemUsers.Doctors.Model;

public class Doctor
{
    public SpecialtyType specialty { get; set; }
    public List<Examination> examinations { get; set; }
    public List<Operation> operations { get; set; }
    public List<RestRequest> restRequests { get; set; }
}

public enum SpecialtyType
{
    GeneralPractitioner,
    Surgeon,
    Pediatrician,
    Radiologist
}