using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.Examinations.Model;

namespace HealthInstitution.Core.ScheduleEditRequests.Repository;

public class ScheduleEditRequestRepository
{
    public String fileName { get; set; }
    public List<ScheduleEditRequest> scheduleEditRequests { get; set; }
    public Dictionary<Int32, ScheduleEditRequest> scheduleEditRequestsById { get; set; }

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private ScheduleEditRequestRepository(String fileName)
    {
        this.fileName = fileName;
        this.scheduleEditRequests = new List<ScheduleEditRequest>();
        this.scheduleEditRequestsById = new Dictionary<int, ScheduleEditRequest>();
        this.LoadRequests();
    }

    private static ScheduleEditRequestRepository instance = null;

    public static ScheduleEditRequestRepository GetInstance()
    {
        {
            if (instance == null)
            {
                instance = new ScheduleEditRequestRepository(@"..\..\..\Data\JSON\scheduleEditRequests.json");
            }
            return instance;
        }
    }

    public void LoadRequests()
    {
        var requests = JsonSerializer.Deserialize<List<ScheduleEditRequest>>(File.ReadAllText(@"..\..\..\Data\JSON\scheduleEditRequests.json"), options);
        foreach (ScheduleEditRequest scheduleEditRequest in requests)
        {
            this.scheduleEditRequests.Add(scheduleEditRequest);
            this.scheduleEditRequestsById.Add(scheduleEditRequest.Id, scheduleEditRequest);
        }
    }

    public void SaveScheduleEditRequests()
    {
        var allScheduleEditRequest = JsonSerializer.Serialize(this.scheduleEditRequests, options);
        File.WriteAllText(this.fileName, allScheduleEditRequest);
    }

    public List<ScheduleEditRequest> GetScheduleEditRequests()
    {
        return this.scheduleEditRequests;
    }

    public ScheduleEditRequest GetScheduleEditRequestById(int id)
    {
        return this.scheduleEditRequestsById[id];
    }

    public void AddScheduleEditRequests(Examination examination)
    {
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, examination, examination.id, RestRequests.Model.RestRequestState.OnHold);
        this.scheduleEditRequests.Add(scheduleEditRequest);
        this.scheduleEditRequestsById.Add(unixTimestamp, scheduleEditRequest);
        SaveScheduleEditRequests();
    }

    public void DeleteScheduleEditRequests(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetScheduleEditRequestById(id);
        if (scheduleEditRequest != null)
        {
            this.scheduleEditRequestsById.Remove(scheduleEditRequest.Id);
            this.scheduleEditRequests.Remove(scheduleEditRequest);
            SaveScheduleEditRequests();
        }
    }
}