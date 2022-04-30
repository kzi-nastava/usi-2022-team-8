using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.ScheduleEditRequests.Repository;

public class ScheduleEditRequestFileRepository
{
    public String fileName { get; set; }
    public List<ScheduleEditRequest> allRequests { get; set; }
    public Dictionary<Int32, ScheduleEditRequest> allRequestsById { get; set; }

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Converters = { new JsonStringEnumConverter() }
    };

    private ScheduleEditRequestFileRepository(String fileName)
    {
        this.fileName = fileName;
        this.allRequests = new List<ScheduleEditRequest>();
        this.allRequestsById = new Dictionary<int, ScheduleEditRequest>();
        this.LoadRequests();
    }

    private static ScheduleEditRequestFileRepository instance = null;

    public static ScheduleEditRequestFileRepository GetInstance()
    {
        {
            if (instance == null)
            {
                instance = new ScheduleEditRequestFileRepository(@"..\..\..\Data\JSON\scheduleEditRequests.json");
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
            ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(id, null, examinationId, state);
            this.allRequests.Add(scheduleEditRequest);
            this.allRequestsById.Add(id, scheduleEditRequest);
        }
    }

    public void Save()
    {
        var allExaminations = JsonSerializer.Serialize(ShortenRequests(), options);
        File.WriteAllText(this.fileName, allExaminations);
    }

    public List<dynamic> ShortenRequests()
    {
        List<dynamic> reducedRequests = new List<dynamic>();
        foreach (ScheduleEditRequest scheduleEditRequest in this.allRequests)
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
        return this.allRequests;
    }

    public ScheduleEditRequest GetScheduleEditRequestById(int id)
    {
        return this.allRequestsById[id];
    }

    public void AddEditRequests(Examination examination)
    {
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, examination, examination.id, RestRequests.Model.RestRequestState.OnHold);
        this.allRequests.Add(scheduleEditRequest);
        this.allRequestsById.Add(unixTimestamp, scheduleEditRequest);
        Save();
    }

    public void AddDeleteRequest(Examination examination)
    {
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, null, examination.id, RestRequests.Model.RestRequestState.OnHold);
        this.allRequests.Add(scheduleEditRequest);
        this.allRequestsById.Add(unixTimestamp, scheduleEditRequest);
        Save();
    }

    public void DeleteRequest(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetScheduleEditRequestById(id);
        if (scheduleEditRequest != null)
        {
            this.allRequestsById.Remove(scheduleEditRequest.Id);
            this.allRequests.Remove(scheduleEditRequest);
            Save();
        }
    }
    public void AcceptScheduleEditRequests(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetScheduleEditRequestById(id);
        if (scheduleEditRequest != null)
        {
            scheduleEditRequest.state = RestRequests.Model.RestRequestState.Accepted;
            ExaminationRepository.GetInstance().SwapExaminationValue(scheduleEditRequest.newExamination);
            Save();
        }
    }
    public void RejectScheduleEditRequests(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetScheduleEditRequestById(id);
        if (scheduleEditRequest != null)
        {
            scheduleEditRequest.state = RestRequests.Model.RestRequestState.Rejected;
            Save();
        }
    }
}