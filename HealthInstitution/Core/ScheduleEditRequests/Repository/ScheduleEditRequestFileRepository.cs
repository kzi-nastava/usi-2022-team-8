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

public class ScheduleEditRequestFileRepository : IScheduleEditRequestFileRepository
{
    private String _fileName = @"..\..\..\Data\scheduleEditRequests.json";
    public List<ScheduleEditRequest> Requests { get; set; }
    public Dictionary<Int32, ScheduleEditRequest> RequestsById { get; set; }
    private IDoctorRepository _doctorRepository;
    private IExaminationRepository _examinationRepository;
    private IMedicalRecordRepository _medicalRecordRepository;
    private IRoomRepository _roomRepository;

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    public ScheduleEditRequestFileRepository(IDoctorRepository doctorRepository, IExaminationRepository examinationRepository, IMedicalRecordRepository medicalRecordRepository, IRoomRepository roomRepository)
    {
        _doctorRepository = doctorRepository;
        _examinationRepository = examinationRepository;
        _medicalRecordRepository = medicalRecordRepository;
        _roomRepository = roomRepository;
        this.Requests = new List<ScheduleEditRequest>();
        this.RequestsById = new Dictionary<int, ScheduleEditRequest>();
        this.LoadFromFile();
    }

    private Examination ParseExamination(JToken? request, int id)
    {
        Dictionary<int, Room> roomsById = _roomRepository.GetAllById();
        Dictionary<String, Doctor> doctorsByUsername = _doctorRepository.GetAllByUsername();
        Dictionary<String, MedicalRecord> medicalRecordsByUsername = _medicalRecordRepository.GetAllByUsername();

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

        Examination loadedExamination = new Examination(id, status, appointment, room, doctor, medicalRecord, anamnesis);
        return loadedExamination;
    }

    private Examination ParseLoadedExamination(JToken? request, int id)
    {
        Examination loadedExamination;
        if (request["newExamination"] is not null)
            loadedExamination = ParseExamination(request, id);
        else
            loadedExamination = null;
        return loadedExamination;
    }

    public void LoadFromFile()
    {
        var requests = JArray.Parse(File.ReadAllText(this._fileName));
        Examination loadedExamination;
        foreach (var request in requests)
        {
            int id = (int)request["id"];
            RestRequestState state;
            Enum.TryParse(request["state"].ToString(), out state);
            int examinationId = (int)request["examinationId"];
            loadedExamination = ParseLoadedExamination(request, id);
            ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(id, loadedExamination, examinationId, _examinationRepository.GetById(examinationId), state);
            this.Requests.Add(scheduleEditRequest);
            this.RequestsById.Add(id, scheduleEditRequest);
        }
    }

    public void Save()
    {
        var allExaminations = JsonSerializer.Serialize(PrepareForSerialization(), _options);
        File.WriteAllText(this._fileName, allExaminations);
    }

    private dynamic MakeWithoutExamination(ScheduleEditRequest scheduleEditRequest)
    {
        return new
        {
            id = scheduleEditRequest.Id,
            examinationId = scheduleEditRequest.CurrentExamination.Id,
            state = scheduleEditRequest.State
        };
    }

    private dynamic MakeWithExamination(ScheduleEditRequest scheduleEditRequest)
    {
        return new
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
        };
    }

    private List<dynamic> PrepareForSerialization()
    {
        List<dynamic> reducedRequests = new List<dynamic>();
        foreach (ScheduleEditRequest scheduleEditRequest in this.Requests)
        {
            if (scheduleEditRequest.NewExamination is null)
            {
                reducedRequests.Add(MakeWithoutExamination(scheduleEditRequest));
            }
            else
            {
                reducedRequests.Add(MakeWithExamination(scheduleEditRequest));
            }
        }
        return reducedRequests;
    }

    public List<ScheduleEditRequest> GetAll()
    {
        return this.Requests;
    }

    public Dictionary<int, ScheduleEditRequest> GetAllById()
    {
        return RequestsById;
    }

    public ScheduleEditRequest GetById(int id)
    {
        return this.RequestsById[id];
    }

    public void AddEditRequest(ScheduleEditRequest scheduleEditRequest, int unixTimestamp)
    {
        Requests.Add(scheduleEditRequest);
        RequestsById.Add(unixTimestamp, scheduleEditRequest);
        Save();
    }

    public void AddDeleteRequest(ScheduleEditRequest scheduleEditRequest, int unixTimestamp)
    {
        Requests.Add(scheduleEditRequest);
        RequestsById.Add(unixTimestamp, scheduleEditRequest);
        Save();
    }

    public void DeleteRequest(ScheduleEditRequest scheduleEditRequest)
    {
        RequestsById.Remove(scheduleEditRequest.Id);
        Requests.Remove(scheduleEditRequest);
        Save();
    }

    public void AcceptScheduleEditRequests(ScheduleEditRequest scheduleEditRequest)
    {
        scheduleEditRequest.State = RestRequestState.Accepted;
        Save();
    }

    public void RejectScheduleEditRequests(ScheduleEditRequest scheduleEditRequest)
    {
        scheduleEditRequest.State = RestRequestState.Rejected;
        Save();
    }
}