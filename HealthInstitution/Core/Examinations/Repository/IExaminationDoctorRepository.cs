namespace HealthInstitution.Core.Examinations.Repository
{
    public interface IExaminationDoctorRepository : ILinkerRepository
    {
        public void LoadFromFile();
        public void Save();
    }
}