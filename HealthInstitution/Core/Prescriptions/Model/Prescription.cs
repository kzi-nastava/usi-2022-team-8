using HealthInstitution.Core.Drugs.Model;

namespace HealthInstitution.Core.Prescriptions.Model;

public class Prescription
{
    public int Id { get; set; }
    public int DailyDose { get; set; }
    public PrescriptionTime TimeOfUse { get; set; }
    public Drug Drug { get; set; }
    public DateTime dateTime { get; set; }

    public Prescription(int id, int dailyDose, PrescriptionTime timeOfUse, Drug drug, DateTime dateTime)
    {
        this.Id = id;
        this.DailyDose = dailyDose;
        this.TimeOfUse = timeOfUse;
        this.Drug = drug;
        this.dateTime = dateTime;
    }
}

public enum PrescriptionTime
{
    NotImportant,
    PreMeal,
    DuringMeal,
    PostMeal
}