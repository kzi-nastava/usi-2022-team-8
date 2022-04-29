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
    //public string patientUsername { get; set; }
    public Patient patient { get; set; }
    public List<Prescription> prescriptions { get; set; }
    public List<Referral> referrals { get; set; }

    public MedicalRecord(double height, double weight, List<string> previousIllnesses, List<string> allergens, Patient patient)
    {
        this.height = height;
        this.weight = weight;
        this.previousIllnesses = previousIllnesses;
        this.allergens = allergens;
        this.patient = patient;
        this.prescriptions = new List<Prescription>();
        this.referrals = new List<Referral>();
    }

    public MedicalRecord(double height, double weight, List<string> previousIllnesses, List<string> allergens, Patient patient, List<Prescription> prescriptions, List<Referral> referrals) : this(height, weight, previousIllnesses, allergens, patient)
    {
        this.prescriptions = prescriptions;
        this.referrals = referrals;
    }
}