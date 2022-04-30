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
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Examinations.Repository;
using Newtonsoft.Json.Linq;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution.Core.ScheduleEditRequests.Repository;

public class ScheduleEditRequestRepository
{
    public String fileName { get; set; }
    public List<ScheduleEditRequest> scheduleEditRequests { get; set; }
    public Dictionary<int, ScheduleEditRequest> scheduleEditRequestsById { get; set; }

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
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
        var roomsById = RoomRepository.GetInstance().roomById;
        var doctorsByUsername = DoctorRepository.GetInstance().doctorsByUsername;
        ExaminationDoctorRepository.GetInstance();
        var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().medicalRecordByUsername;

        var requests = JArray.Parse(File.ReadAllText(this.fileName));
        Examination loadedExamination;
        foreach (var request in requests)
        {
            int id = (int)request["id"];
            RestRequestState state;
            Enum.TryParse(request["state"].ToString(), out state);
            int examinationId = (int)request["examinationId"];
            if (request["newExamination"] is not null)
            {
                ExaminationStatus status;
                Enum.TryParse(request["newExamination"]["status"].ToString(), out status);
                DateTime appointment = (DateTime)request["newExamination"]["appointment"];
                int roomId = (int)request["newExamination"]["room"];
                Room room = roomsById[roomId];
                String doctorUsername = (String)request["newExamination"]["doctor"];
                Doctor doctor = doctorsByUsername[doctorUsername];
                String patientUsername = (String)request["newExamination"]["medicalRecord"];
                MedicalRecord medicalRecord = medicalRecordsByUsername[patientUsername];
                String anamnesis = (String)request["newExamination"]["anamnesis"];

                loadedExamination = new Examination(id, status, appointment, room, doctor, medicalRecord, anamnesis);
            }
            else
            {
                loadedExamination = null;
            }
            ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(id, loadedExamination, examinationId, state);
            this.scheduleEditRequests.Add(scheduleEditRequest);
            this.scheduleEditRequestsById.Add(id, scheduleEditRequest);
        }
    }

    public void SaveScheduleEditRequests()
    {
        var allExaminations = JsonSerializer.Serialize(ShortenRequests(), options);
        File.WriteAllText(this.fileName, allExaminations);
    }

    public List<dynamic> ShortenRequests()
    {
        List<dynamic> reducedRequests = new List<dynamic>();
        foreach (ScheduleEditRequest scheduleEditRequest in this.scheduleEditRequests)
        {
            if (scheduleEditRequest.newExamination is null)
            {
                reducedRequests.Add(new
                {
                    id = scheduleEditRequest.Id,
                    examinationId = scheduleEditRequest.currentExamination.id,
                    state = scheduleEditRequest.state
                });
            }
            else
            {
                reducedRequests.Add(new
                {
                    id = scheduleEditRequest.Id,
                    examinationId = scheduleEditRequest.currentExamination.id,
                    state = scheduleEditRequest.state,
                    newExamination = new
                    {
                        id = scheduleEditRequest.newExamination.id,
                        status = scheduleEditRequest.newExamination.status,
                        appointment = scheduleEditRequest.newExamination.appointment,
                        doctor = scheduleEditRequest.newExamination.doctor.username,
                        room = scheduleEditRequest.newExamination.room.id,
                        medicalRecord = scheduleEditRequest.newExamination.medicalRecord.patient.username,
                        anamnesis = scheduleEditRequest.newExamination.anamnesis
                    }
                });
            }
        }
        return reducedRequests;
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

    public void AddDeleteRequest(Examination examination)
    {
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, null, examination.id, RestRequests.Model.RestRequestState.OnHold);
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
    public void AcceptScheduleEditRequests(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetScheduleEditRequestById(id);
        if (scheduleEditRequest != null)
        {
            scheduleEditRequest.state = RestRequests.Model.RestRequestState.Accepted;
            SaveScheduleEditRequests();
        }
    }
    public void RejectScheduleEditRequests(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetScheduleEditRequestById(id);
        if (scheduleEditRequest != null)
        {
            scheduleEditRequest.state = RestRequests.Model.RestRequestState.Rejected;
            SaveScheduleEditRequests();
        }
    }
}