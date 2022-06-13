using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Doctors.Repository
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        public void LoadFromFile();
        public void Save();
        public List<Doctor> GetAll();
        public Doctor GetById(String username);
        public void DeleteExamination(Examination examination);
        public void DeleteOperation(Operation operation);
        public List<Examination> GetScheduledExaminations(Doctor doctor);
        public List<Operation> GetScheduledOperations(Doctor doctor);
        public List<Examination> GetExaminationsInThreeDays(List<Examination> examinations);
        public List<Operation> GetOperationsInThreeDays(List<Operation> operations);
        public List<Examination> GetExaminationsByDate(List<Examination> examinations, DateTime date);
        public List<Operation> GetOperationsByDate(List<Operation> operations, DateTime date);
        public void DeleteNotifications(Doctor doctor);
        public void DeleteRestRequest(RestRequest restRequest);
        public List<Doctor> GetSearchName(string keyword);
        public List<Doctor> GetSearchSurname(string keyword);
        public List<Doctor> GetSearchSpecialty(string keyword);

    }
}
