using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
namespace HealthInstitution.Core.MedicalRecords.Model;

public class MedicalRecord
{
    public double Height { get; set; }
    public double Weight { get; set; }
    public List<String> PreviousIllnesses { get; set; }
    public List<String> Allergens { get; set; }
    //public string patientUsername { get; set; }
    public Patient Patient { get; set; }
    public List<Prescription> Prescriptions { get; set; }
    public List<Referral> Referrals { get; set; }

    public MedicalRecord(double height, double weight, List<string> previousIllnesses, List<string> allergens, Patient patient)
    {
        this.Height = height;
        this.Weight = weight;
        this.PreviousIllnesses = previousIllnesses;
        this.Allergens = allergens;
        this.Patient = patient;
        this.Prescriptions = new List<Prescription>();
        this.Referrals = new List<Referral>();
    }

    public MedicalRecord(double height, double weight, List<string> previousIllnesses, List<string> allergens, Patient patient, List<Prescription> prescriptions, List<Referral> referrals) : this(height, weight, previousIllnesses, allergens, patient)
    {
        this.Prescriptions = prescriptions;
        this.Referrals = referrals;
    }
}