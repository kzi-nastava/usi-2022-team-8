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
    private String _fileName;
    public List<ScheduleEditRequest> Requests { get; set; }
    public Dictionary<Int32, ScheduleEditRequest> RequestsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private ScheduleEditRequestFileRepository(String fileName)
    {
        this._fileName = fileName;
        this.Requests = new List<ScheduleEditRequest>();
        this.RequestsById = new Dictionary<int, ScheduleEditRequest>();
        this.LoadFromFile();
    }

    private static ScheduleEditRequestFileRepository s_instance = null;

    public static ScheduleEditRequestFileRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new ScheduleEditRequestFileRepository(@"..\..\..\Data\JSON\scheduleEditRequests.json");
            }
            return s_instance;
        }
    }

    public void LoadFromFile()
    {
        var roomsById = RoomRepository.GetInstance().RoomById;
        var doctorsByUsername = DoctorRepository.GetInstance().DoctorsByUsername;
        ExaminationDoctorRepository.GetInstance();
        var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().MedicalRecordByUsername;

        var requests = JArray.Parse(File.ReadAllText(this._fileName));
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
            this.Requests.Add(scheduleEditRequest);
            this.RequestsById.Add(id, scheduleEditRequest);
        }
    }

    public void Save()
    {
        var allExaminations = JsonSerializer.Serialize(shortenRequests(), _options);
        File.WriteAllText(this._fileName, allExaminations);
    }

    private List<dynamic> shortenRequests()
    {
        List<dynamic> reducedRequests = new List<dynamic>();
        foreach (ScheduleEditRequest scheduleEditRequest in this.Requests)
        {
            if (scheduleEditRequest.NewExamination is null)
            {
                reducedRequests.Add(new
                {
                    id = scheduleEditRequest.Id,
                    examinationId = scheduleEditRequest.CurrentExamination.Id,
                    state = scheduleEditRequest.State
                });
            }
            else
            {
                reducedRequests.Add(new
                {
                    id = scheduleEditRequest.Id,
                    examinationId = scheduleEditRequest.CurrentExamination.Id,
                    state = scheduleEditRequest.State,
                    newExamination = new
                    {
                        id = scheduleEditRequest.NewExamination.Id,
                        status = scheduleEditRequest.NewExamination.Status,
                        appointment = scheduleEditRequest.NewExamination.Appointment,
                        doctor = scheduleEditRequest.NewExamination.Doctor.Username,
                        room = scheduleEditRequest.NewExamination.Room.Id,
                        medicalRecord = scheduleEditRequest.NewExamination.MedicalRecord.Patient.Username,
                        anamnesis = scheduleEditRequest.NewExamination.Anamnesis
                    }
                });
            }
        }
        return reducedRequests;
    }

    public List<ScheduleEditRequest> GetAll()
    {
        return this.Requests;
    }

    public ScheduleEditRequest GetById(int id)
    {
        return this.RequestsById[id];
    }

    public void AddEditRequest(Examination examination)
    {
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, examination, examination.Id, RestRequests.Model.RestRequestState.OnHold);
        this.Requests.Add(scheduleEditRequest);
        this.RequestsById.Add(unixTimestamp, scheduleEditRequest);
        Save();
    }

    public void AddDeleteRequest(Examination examination)
    {
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, null, examination.Id, RestRequests.Model.RestRequestState.OnHold);
        this.Requests.Add(scheduleEditRequest);
        this.RequestsById.Add(unixTimestamp, scheduleEditRequest);
        Save();
    }

    public void DeleteRequest(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetById(id);
        if (scheduleEditRequest != null)
        {
            this.RequestsById.Remove(scheduleEditRequest.Id);
            this.Requests.Remove(scheduleEditRequest);
            Save();
        }
    }
    public void AcceptScheduleEditRequests(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetById(id);
        if (scheduleEditRequest != null)
        {
            scheduleEditRequest.State = RestRequests.Model.RestRequestState.Accepted;
            ExaminationRepository.GetInstance().SwapExaminationValue(scheduleEditRequest.NewExamination);
            Save();
        }
    }
    public void RejectScheduleEditRequests(int id)
    {
        ScheduleEditRequest scheduleEditRequest = GetById(id);
        if (scheduleEditRequest != null)
        {
            scheduleEditRequest.State = RestRequests.Model.RestRequestState.Rejected;
            Save();
        }
    }
}