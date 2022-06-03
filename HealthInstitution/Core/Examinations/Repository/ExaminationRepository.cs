using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.RecommededDTO;
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

internal class ExaminationRepository
{
    private String _fileName;
    public int _maxId { get; set; }
    public List<Examination> Examinations { get; set; }
    public Dictionary<int, Examination> ExaminationsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private ExaminationRepository(String fileName)
    {
        _fileName = fileName;
        Examinations = new List<Examination>();
        ExaminationsById = new Dictionary<int, Examination>();
        _maxId = 0;
        LoadFromFile();
    }

    private static ExaminationRepository s_instance = null;

    public static ExaminationRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new ExaminationRepository(@"..\..\..\Data\JSON\examinations.json");
            }
            return s_instance;
        }
    }

    private Examination Parse(JToken? examination)
    {
        Dictionary<int, Room> roomsById = RoomRepository.GetInstance().RoomById;
        Dictionary<String, MedicalRecord> medicalRecordsByUsername = MedicalRecordRepository.GetInstance().MedicalRecordByUsername;

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
        ExaminationDoctorRepository.GetInstance().Save();
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

    public List<Examination> GetByPatient(string username)
    {
        List<Examination> patientExaminations = new List<Examination>();
        foreach (var examination in Examinations)
            if (examination.MedicalRecord.Patient.Username == username)
                patientExaminations.Add(examination);
        return patientExaminations;
    }

    public List<Examination> GetCompletedByPatient(string patientUsername)
    {
        List<Examination> completed = new List<Examination>();

        foreach (Examination examination in Examinations)
        {
            if (examination.Status == ExaminationStatus.Completed && examination.MedicalRecord.Patient.Username == patientUsername)
                completed.Add(examination);
        }
        return completed;
    }

    //stankelino

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

    private ExaminationDTO FindFit(ExaminationDTO examinationDTO, FindFitDTO findFitDTO)
    {
        bool found = false;
        while (findFitDTO.fit <= findFitDTO.end)
        {
            try
            {
                Room room = RoomRepository.GetInstance().FindAvailableRoom(findFitDTO.fit);
                examinationDTO.Appointment = findFitDTO.fit;
                examinationDTO.Room = room;
                CheckIfPatientIsAvailable(examinationDTO);
                CheckIfDoctorIsAvailable(examinationDTO);
                found = true;
                break;
            }
            catch
            {
                findFitDTO.fit = IncrementFit(findFitDTO.fit, findFitDTO.maxHour, findFitDTO.maxMinutes, findFitDTO.minHour, findFitDTO.minMinutes);
            }
        }
        if (found)
            return examinationDTO;
        else
            return null;
    }

    public bool FindFirstFit(FirstFitDTO firstFitDTO)
    {
        bool found = false;
        DateTime fit = GenerateFitDateTime(firstFitDTO.minHour, firstFitDTO.minMinutes);
        Doctor doctor = DoctorRepository.GetInstance().GetById(firstFitDTO.doctorUsername);
        Patient patient = PatientRepository.GetInstance().GetByUsername(firstFitDTO.patientUsername);
        var medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
        ExaminationDTO examinationDTO = new ExaminationDTO(fit, null, doctor, medicalRecord);
        FindFitDTO findFitDTO = new FindFitDTO(fit, firstFitDTO.end, firstFitDTO.minHour, firstFitDTO.minMinutes, firstFitDTO.maxHour, firstFitDTO.maxMinutes);
        ExaminationDTO firstFit = FindFit(examinationDTO, findFitDTO);
        if (firstFit is not null)
        {
            found = true;
            ExaminationService.Add(examinationDTO);
            MessageBox.Show("Examination scheduled for: " + fit.ToString());
        }
        return found;
    }

    public DateTime GenerateFitDateTime(int minHour, int minMinutes)
    {
        DateTime fit = DateTime.Today.AddDays(1);
        fit = fit.AddHours(minHour);
        fit = fit.AddMinutes(minMinutes);
        return fit;
    }

    public List<Examination> FindClosestFit(ClosestFitDTO closestFitDTO)
    {
        Doctor pickedDoctor = DoctorRepository.GetInstance().GetById(closestFitDTO.doctorUsername);
        Patient patient = PatientRepository.GetInstance().GetByUsername(closestFitDTO.patientUsername);
        var medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
        List<Examination> suggestions = new List<Examination>();
        List<Doctor> viableDoctors = new List<Doctor>();

        if (closestFitDTO.doctorPriority)
        {
            closestFitDTO.maxHour = 22;
            closestFitDTO.maxMinutes = 45;
            viableDoctors.Add(pickedDoctor);
            closestFitDTO.end = closestFitDTO.end.AddHours(-closestFitDTO.end.Hour + 22);
            closestFitDTO.end = closestFitDTO.end.AddMinutes(-closestFitDTO.end.Minute + 45);
        }
        else
        {
            viableDoctors = DoctorRepository.GetInstance().GetAll();
            viableDoctors.Remove(pickedDoctor);
        }

        foreach (Doctor doctor in viableDoctors)
        {
            DateTime fit = GenerateFitDateTime(closestFitDTO.minHour, closestFitDTO.minMinutes);
            ExaminationDTO examinationDTO = new ExaminationDTO(fit, null, doctor, medicalRecord);

            if (suggestions.Count == 3) break;
            while (fit <= closestFitDTO.end)
            {
                if (suggestions.Count == 3) break;
                FindFitDTO findFitDTO = new FindFitDTO(fit, closestFitDTO.end, closestFitDTO.maxHour, closestFitDTO.minMinutes, closestFitDTO.maxHour, closestFitDTO.maxMinutes);
                ExaminationDTO firstFit = FindFit(examinationDTO, findFitDTO);
                if (firstFit is not null)
                {
                    suggestions.Add(new Examination(firstFit));
                    fit = firstFit.Appointment;
                    fit = IncrementFit(fit, closestFitDTO.maxHour, closestFitDTO.maxMinutes, closestFitDTO.minHour, closestFitDTO.minMinutes);
                }
            }
        }
        return suggestions;
    }

    public static DateTime IncrementFit(DateTime fit, int maxHour, int maxMinutes, int minHour, int minMinutes)
    {
        fit = fit.AddMinutes(15);

        if ((fit.Hour > maxHour) || (fit.Hour == maxHour && fit.Minute > maxMinutes))
        {
            fit = fit.AddDays(1);
            fit = fit.AddHours(minHour - fit.Hour);
            fit = fit.AddMinutes(minMinutes - fit.Minute);
        }
        return fit;
    }
}