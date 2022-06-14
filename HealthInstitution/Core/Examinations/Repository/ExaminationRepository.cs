using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.Examinations.Repository;

public class ExaminationRepository : IExaminationRepository
{
    private String _fileName = @"..\..\..\Data\JSON\examinations.json";

    private IRoomRepository _roomRepository;

    private IMedicalRecordRepository _medicalRecordRepository;

    private IExaminationDoctorRepository _examinationDoctorRepository;
    public int _maxId { get; set; }
    public List<Examination> Examinations { get; set; }
    public Dictionary<int, Examination> ExaminationsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    public ExaminationRepository(IRoomRepository roomRepository, IMedicalRecordRepository medicalRecordRepository, IExaminationDoctorRepository examinationDoctorRepository)
    {
        _roomRepository = roomRepository;
        _medicalRecordRepository = medicalRecordRepository;
        _examinationDoctorRepository = examinationDoctorRepository;
        Examinations = new List<Examination>();
        ExaminationsById = new Dictionary<int, Examination>();
        _maxId = 0;
        LoadFromFile();
    }

    private Examination Parse(JToken? examination)
    {
        Dictionary<int, Room> roomsById = _roomRepository.GetAllById();
        Dictionary<String, MedicalRecord> medicalRecordsByUsername = _medicalRecordRepository.GetAllByUsername();

        int id = (int)examination["id"];
        ExaminationStatus status;
        Enum.TryParse(examination["status"].ToString(), out status);
        DateTime appointment = (DateTime)examination["appointment"];
        int roomId = (int)examination["room"];
        Room room = roomsById[roomId];
        String doctorUsername = (String)examination["doctor"];
        String patientUsername = (String)examination["medicalRecord"];
        MedicalRecord medicalRecord = medicalRecordsByUsername[patientUsername];
        String anamnesis = (String)examination["anamnesis"];

        return new Examination(id, status, appointment, room, null, medicalRecord, anamnesis);
    }

    public void LoadFromFile()
    {
        var allExaminations = JArray.Parse(File.ReadAllText(_fileName));
        foreach (var examination in allExaminations)
        {
            Examination loadedExamination = Parse(examination);
            int id = loadedExamination.Id;
            if (id > _maxId) { _maxId = id; }

            Examinations.Add(loadedExamination);
            ExaminationsById.Add(id, loadedExamination);
        }
    }

    private List<dynamic> PrepareForSerialization()
    {
        List<dynamic> reducedExaminations = new List<dynamic>();
        foreach (Examination examination in Examinations)
        {
            reducedExaminations.Add(new
            {
                id = examination.Id,
                status = examination.Status,
                appointment = examination.Appointment,
                room = examination.Room.Id,
                medicalRecord = examination.MedicalRecord.Patient.Username,
                anamnesis = examination.Anamnesis
            });
        }
        return reducedExaminations;
    }

    public void Save()
    {
        List<dynamic> reducedExaminations = PrepareForSerialization();
        var allExaminations = JsonSerializer.Serialize(reducedExaminations, _options);
        File.WriteAllText(_fileName, allExaminations);
    }

    public List<Examination> GetAll()
    {
        return Examinations;
    }

    public Dictionary<int, Examination> GetAllById()
    {
        return this.ExaminationsById;
    }

    public Examination GetById(int id)
    {
        if (ExaminationsById.ContainsKey(id))
        {
            return ExaminationsById[id];
        }
        return null;
    }

    private void AddToCollections(Examination examination)
    {
        examination.Doctor.Examinations.Add(examination);
        Examinations.Add(examination);
        ExaminationsById.Add(examination.Id, examination);
    }

    private void SaveAll()
    {
        Save();
        _examinationDoctorRepository.Save();
    }

    public void Add(Examination examination)
    {
        examination.Id = ++_maxId;
        AddToCollections(examination);
        SaveAll();
    }

    public void Update(int id, Examination byExamination)
    {
        Examination examination = GetById(id);
        examination.Doctor = byExamination.Doctor;
        examination.Appointment = byExamination.Appointment;
        examination.MedicalRecord = byExamination.MedicalRecord;
        ExaminationsById[id] = examination;
        Save();
    }

    public void Delete(int id)
    {
        Examination examination = ExaminationsById[id];
        ExaminationsById.Remove(examination.Id);
        Examinations.Remove(examination);
        SaveAll();
    }

    public List<Examination> GetByPatient(string patientUsername)
    {
        List<Examination> patientExaminations = new List<Examination>();
        foreach (var examination in Examinations)
            if (examination.MedicalRecord.Patient.Username == patientUsername)
                patientExaminations.Add(examination);
        return patientExaminations;
    }

    public List<Examination> GetByDoctor(String doctorUsername)
    {
        List<Examination> doctorExaminations = new List<Examination>();
        foreach (var examination in this.GetAll())
            if (examination.Doctor.Username == doctorUsername)
                doctorExaminations.Add(examination);
        return doctorExaminations;
    }

    public List<Examination> GetCompletedByPatient(string patientUsername)
    {
        List<Examination> completed = new List<Examination>();
        foreach (Examination examination in GetByPatient(patientUsername))
        {
            if (examination.Status == ExaminationStatus.Completed)
                completed.Add(examination);
        }
        return completed;
    }

    public List<Examination> GetSeachAnamnesis(string keyword, string patientUsername)
    {
        keyword = keyword.Trim();
        List<Examination> resault = new List<Examination>();
        var completed = GetCompletedByPatient(patientUsername);

        foreach (Examination examination in completed)
        {
            if (examination.Anamnesis.ToLower().Contains(keyword)) resault.Add(examination);
        }

        return resault;
    }

    public void SwapExaminationValue(Examination examination)
    {
        var oldExamination = ExaminationsById[examination.Id];
        Examinations.Remove(oldExamination);
        Examinations.Add(examination);
        examination.Doctor.Examinations.Add(examination);
        oldExamination.Doctor.Examinations.Remove(oldExamination);
        ExaminationsById[examination.Id] = examination;
        Save();
    }
}