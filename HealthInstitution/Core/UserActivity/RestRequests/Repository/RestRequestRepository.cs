using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.RestRequests.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core
{
    public class RestRequestRepository : IRestRequestRepository
    {
        private String _fileName = @"..\..\..\Data\RestRequests.json";
        public int _maxId { get; set; }
        public List<RestRequest> RestRequests { get; set; }
        public Dictionary<int, RestRequest> RestRequestsById { get; set; }
        private IDoctorRepository _doctorRepository;

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public RestRequestRepository(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            this.RestRequests = new List<RestRequest>();
            this.RestRequestsById = new Dictionary<int, RestRequest>();
            this._maxId = 0;
            this.LoadFromFile();
        }

        private RestRequest Parse(JToken? RestRequest)
        {
            int id = (int)RestRequest["id"];
            string reason = (string)RestRequest["reason"];
            DateTime startDate = (DateTime)RestRequest["startDate"];
            int daysDuration = (int)RestRequest["daysDuration"];
            RestRequestState state;
            Enum.TryParse(RestRequest["state"].ToString(), out state);
            bool urgent = (bool)RestRequest["urgent"];
            string rejectionReason = (string)RestRequest["rejectionReason"];

            return new RestRequest(id, null, reason, startDate, daysDuration, state, urgent, rejectionReason);
        }

        public void LoadFromFile()
        {
            var allRestRequests = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var RestRequest in allRestRequests)
            {
                RestRequest loadedRestRequest = Parse(RestRequest);
                int id = loadedRestRequest.Id;
                if (id > _maxId) { _maxId = id; }

                this.RestRequests.Add(loadedRestRequest);
                this.RestRequestsById.Add(id, loadedRestRequest);
            }
        }

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedRestRequests = new List<dynamic>();
            foreach (RestRequest RestRequest in this.RestRequests)
            {
                reducedRestRequests.Add(new
                {
                    id = RestRequest.Id,
                    reason = RestRequest.Reason,
                    startDate = RestRequest.StartDate,
                    daysDuration = RestRequest.DaysDuration,
                    state = RestRequest.State,
                    urgent = RestRequest.IsUrgent,
                    rejectionReason = RestRequest.RejectionReason
                });
            }
            return reducedRestRequests;
        }

        public void Save()
        {
            List<dynamic> reducedRestRequests = PrepareForSerialization();
            var allRestRequests = JsonSerializer.Serialize(reducedRestRequests, _options);
            File.WriteAllText(this._fileName, allRestRequests);
        }

        public List<RestRequest> GetAll()
        {
            return this.RestRequests;
        }

        public Dictionary<int, RestRequest> GetAllById()
        {
            return RestRequestsById;
        }

        public RestRequest GetById(int id)
        {
            if (RestRequestsById.ContainsKey(id))
            {
                return RestRequestsById[id];
            }
            return null;
        }

        private void AddToCollections(RestRequest RestRequest)
        {
            RestRequest.Doctor.RestRequests.Add(RestRequest);
            RestRequests.Add(RestRequest);
            RestRequestsById.Add(RestRequest.Id, RestRequest);
        }

        private void SaveAll()
        {
            Save();
            DIContainer.DIContainer.GetService<IRestRequestDoctorRepository>().Save();
        }

        public void Add(RestRequest restRequest)
        {
            int id = ++this._maxId;
            restRequest.Id = id;
            AddToCollections(restRequest);
            SaveAll();
        }

        public void Update(int id, RestRequest byRestRequest)
        {
            RestRequest restRequest = GetById(id);
            restRequest.Reason = byRestRequest.Reason;
            restRequest.StartDate = byRestRequest.StartDate;
            restRequest.DaysDuration = byRestRequest.DaysDuration;
            this.RestRequestsById[id] = restRequest;
            Save();
        }

        public void Delete(int id)
        {
            RestRequest restRequest = RestRequestsById[id];
            this.RestRequests.Remove(restRequest);
            this.RestRequestsById.Remove(id);
            restRequest.Doctor.RestRequests.Remove(restRequest);
            SaveAll();
        }

        public void Accept(RestRequest restRequest)
        {
            restRequest.State = RestRequestState.Accepted;
            Save();
        }

        public void Reject(RestRequest restRequest, string rejectionReason)
        {
            restRequest.State = RestRequestState.Rejected;
            restRequest.RejectionReason = rejectionReason;
            Save();
        }

        public List<RestRequest> GetByDoctor(string doctorUsername)
        {
            /*List<RestRequest> doctorRestRequests = new List<RestRequest>();
            foreach (var restRequest in this.RestRequests)
            {
                if (restRequest.Doctor.Username == doctorUsername)
                    doctorRestRequests.Add(restRequest);
            }
            return doctorRestRequests;*/

            return GetAll().Where(restRequest => restRequest.Doctor.Username == doctorUsername).ToList();
        }
    }
}