namespace HealthInstitution.Core.Examinations.Repository
{
    public interface IExaminationDoctorRepository
    {
        public void LoadFromFile();
        public void Save();
    }
}