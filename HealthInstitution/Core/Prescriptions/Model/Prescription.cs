using HealthInstitution.Core.Drugs.Model;

namespace HealthInstitution.Core.Prescriptions.Model;

public class Prescription
{
    public int id { get; set; }
    public int dailyDose { get; set; }
    public PrescriptionTime timeOfUse { get; set; }
    public Drug drug { get; set; }

    public Prescription(int id, int dailyDose, PrescriptionTime timeOfUse, Drug drug)
    {
        this.id = id;
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