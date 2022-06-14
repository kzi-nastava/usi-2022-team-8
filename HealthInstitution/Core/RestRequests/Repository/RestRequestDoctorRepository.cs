using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RestRequests.Repository
{
    public class RestRequestDoctorRepository : IRestRequestDoctorRepository
    {
        private String _fileName;
        private RestRequestDoctorRepository(String fileName)
        {
            this._fileName = fileName;
            this.LoadFromFile();
        }

        private static RestRequestDoctorRepository s_instance = null;

        public static RestRequestDoctorRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new RestRequestDoctorRepository(@"..\..\..\Data\JSON\restRequestDoctor.json");
                }
                return s_instance;
            }
        }

        public void LoadFromFile()
        {
            var doctorsByUsername = DoctorRepository.GetInstance().DoctorsByUsername;
            var restRequestsById = RestRequestRepository.GetInstance().RestRequestsById;
            var operationIdsDoctorUsernames = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var pair in operationIdsDoctorUsernames)
            {
                int id = (int)pair["id"];
                String username = (String)pair["doctor"];
                Doctor doctor = doctorsByUsername[username];
                RestRequest restRequest = restRequestsById[id];
                doctor.RestRequests.Add(restRequest);
                restRequest.Doctor = doctor;
            }
        }

        public void Save()
        {
            List<dynamic> restRequestsIdsDoctorUsernames = new List<dynamic>();
            var restRequests = RestRequestRepository.GetInstance().RestRequests;
            foreach (var restRequest in restRequests)
            {
                Doctor doctor = restRequest.Doctor;
                restRequestsIdsDoctorUsernames.Add(new { id = restRequest.Id, doctor = doctor.Username });
            }
            var allPairs = JsonSerializer.Serialize(restRequestsIdsDoctorUsernames);
            File.WriteAllText(this._fileName, allPairs);
        }
    }
}
