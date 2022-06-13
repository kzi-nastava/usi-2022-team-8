using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Patients
{
    public interface IPatientService
    {
        public List<Patient> GetAll();
        public Patient GetByUsername(string username);
        public void ChangeBlockedStatus(string username);
        public void Add(UserDTO userDTO, MedicalRecords.Model.MedicalRecordDTO medicalRecordDTO);
        public void Update(UserDTO userDTO);
        public void Delete(string username);
        public void DeleteNotifications(Patient patient);
        public bool IsAvailableForDeletion(Patient patient);
    }
}
