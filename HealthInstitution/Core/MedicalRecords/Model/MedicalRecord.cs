using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;

namespace HealthInstitution.Core.MedicalRecords.Model;

public class MedicalRecord
{
    public double height { get; set; }
    public double weight { get; set; }
    public List<String> previousIllnesses { get; set; }
    public List<String> allergens { get; set; }
    public Patient patient { get; set; }
    public List<Prescription> prescriptions { get; set; }
    public List<Referral> referrals { get; set; }
    
}