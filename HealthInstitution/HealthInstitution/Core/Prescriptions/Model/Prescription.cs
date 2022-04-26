using HealthInstitution.Core.Drugs.Model;

namespace HealthInstitution.Core.Prescriptions.Model;

public class Prescription
{
    public int dailyDose { get; set; }
    public PrescriptionTime timeOfUse { get; set; }
    public Drug drug { get; set; }

    public Prescription(int dailyDose, PrescriptionTime timeOfUse, Drug drug)
    {
        this.dailyDose = dailyDose;
        this.timeOfUse = timeOfUse;
        this.drug = drug;
    }
}

public enum PrescriptionTime
{
    NotImportant,
    PreMeal,
    DuringMeal,
    PostMeal
}