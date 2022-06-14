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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RestRequests.Repository
{
    public class RestRequestRepository : IRestRequestRepository
    {
        private String _fileName;
        public int _maxId { get; set; }
        public List<RestRequest> RestRequests { get; set; }
        public Dictionary<int, RestRequest> RestRequestsById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        private RestRequestRepository(String fileName)
        {
            this._fileName = fileName;
            this.RestRequests = new List<RestRequest>();
            this.RestRequestsById = new Dictionary<int, RestRequest>();
            this._maxId = 0;
            this.LoadFromFile();
        }
        private static RestRequestRepository s_instance = null;
        public static RestRequestRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new RestRequestRepository(@"..\..\..\Data\JSON\RestRequests.json");
                }
                return s_instance;
            }
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

        public void Add(RestRequest RestRequest)
        {
            int id = ++this._maxId;
            RestRequest.Id = id;
            AddToCollections(RestRequest);
            Save();
            DoctorRepository.GetInstance().Save();
        }

        public void Update(int id, RestRequest byRestRequest)
        {
            RestRequest RestRequest = GetById(id);
            RestRequest.Reason = byRestRequest.Reason;
            RestRequest.StartDate = byRestRequest.StartDate;
            RestRequest.DaysDuration = byRestRequest.DaysDuration;
            this.RestRequestsById[id] = RestRequest;
            Save();
        }
        public void Delete(int id)
        {
            RestRequest RestRequest = RestRequestsById[id];
            this.RestRequests.Remove(RestRequest);
            this.RestRequestsById.Remove(id);
            Save();
            RestRequestDoctorRepository.GetInstance().Save();
        }
        public void AcceptRestRequest(RestRequest restRequest)
        {
            restRequest.State = RestRequestState.Accepted;
            Save();
        }
        public void RejectRestRequest(RestRequest restRequest, string rejectionReason)
        {
            restRequest.State = RestRequestState.Rejected;
            restRequest.RejectionReason = rejectionReason;
            Save();
        }
    }
}
