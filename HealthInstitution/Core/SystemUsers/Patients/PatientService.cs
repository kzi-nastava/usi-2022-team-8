using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Patients
{
    internal static class PatientService
    {
        static PatientRepository s_patientRepository = PatientRepository.GetInstance();
        public static Patient GetByUsername(string username)
        {
            return s_patientRepository.GetByUsername(username);
        }
        public static void ChangeBlockedStatus(string username)
        {
            Patient patient = GetByUsername(username);
            User user = UserService.GetByUsername(username);
            s_patientRepository.ChangeBlockedStatus(patient);
            UserService.ChangeBlockedStatus(user);
        }
    }
}
