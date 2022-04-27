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
    public string patientUsername { get; set; }
    public List<Prescription> prescriptions { get; set; }
    public List<Referral> referrals { get; set; }

    public MedicalRecord(double height, double weight, List<string> previousIllnesses, List<string> allergens, string patientUsername)
    {
        this.height = height;
        this.weight = weight;
        this.previousIllnesses = previousIllnesses;
        this.allergens = allergens;
        this.patientUsername = patientUsername;
        this.prescriptions = new List<Prescription>();
        this.referrals = new List<Referral>();
    }
}