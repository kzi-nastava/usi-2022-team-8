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
        private String _fileName= @"..\..\..\Data\JSON\restRequestDoctor.json";
        IDoctorRepository _doctorRepository;
        IRestRequestRepository _restRequestRepository;
        public RestRequestDoctorRepository(IDoctorRepository doctorRepository, IRestRequestRepository restRequestRepository)
        {
            _doctorRepository = doctorRepository;
            _restRequestRepository = restRequestRepository;
            this.LoadFromFile();
        }

        public void LoadFromFile()
        {
            var doctorsByUsername = _doctorRepository.GetAllByUsername();
            var restRequestsById = _restRequestRepository.GetAllById();
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
            var restRequests = _restRequestRepository.GetAll();
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
