using HealthInstitution.Core.Drugs.Model;

namespace HealthInstitution.Core.Prescriptions.Model;

public class Prescription
{
    public int Id { get; set; }
    public int DailyDose { get; set; }
    public PrescriptionTime TimeOfUse { get; set; }
    public Drug Drug { get; set; }
    public DateTime HourlyRate { get; set; }

    public Prescription()
    { }

    public Prescription(int id, int dailyDose, PrescriptionTime timeOfUse, Drug drug, DateTime hourlyRate)
    {
        this.Id = id;
        this.DailyDose = dailyDose;
        this.TimeOfUse = timeOfUse;
        this.Drug = drug;
        this.HourlyRate = hourlyRate;
    }

    public Prescription(PrescriptionDTO prescriptionDTO)
    {
        this.DailyDose = prescriptionDTO.DailyDose;
        this.TimeOfUse = prescriptionDTO.TimeOfUse;
        this.Drug = prescriptionDTO.Drug;
        this.HourlyRate = prescriptionDTO.HourlyRate;
    }
}

public enum PrescriptionTime
{
    NotImportant,
    PreMeal,
    DuringMeal,
    PostMeal
}