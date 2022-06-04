using HealthInstitution.Core.Drugs.Model;

namespace HealthInstitution.Core.Prescriptions.Model
{
    public class PrescriptionDTO
    {
        public int DailyDose { get; set; }
        public PrescriptionTime TimeOfUse { get; set; }
        public Drug Drug { get; set; }
        public DateTime HourlyRate { get; set; }

        public PrescriptionDTO(int dailyDose, PrescriptionTime timeOfUse, Drug drug, DateTime hourlyRate)
        {
            this.DailyDose = dailyDose;
            this.TimeOfUse = timeOfUse;
            this.Drug = drug;
            this.HourlyRate = hourlyRate;
        }
    }
}