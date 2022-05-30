using HealthInstitution.Core.Drugs.Model;

namespace HealthInstitution.Core.Prescriptions.Model
{
    public class PrescriptionDTO
    {
        public int DailyDose { get; set; }
        public PrescriptionTime TimeOfUse { get; set; }
        public Drug Drug { get; set; }
        public DateTime showTime { get; set; }

        public PrescriptionDTO(int dailyDose, PrescriptionTime timeOfUse, Drug drug)
        {
            this.DailyDose = dailyDose;
            this.TimeOfUse = timeOfUse;
            this.Drug = drug;
        }
    }
}