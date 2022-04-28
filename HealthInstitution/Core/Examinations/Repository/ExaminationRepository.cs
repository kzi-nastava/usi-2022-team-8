using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Appointments.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;


namespace HealthInstitution.Core.Examinations.Repository;

public class ExaminationRepository
{
    public String fileName { get; set; }
    public List<Examination> examinations { get; set; }
    public Dictionary<Int32, Examination> examinationsById { get; set; }

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private ExaminationRepository(String fileName)
    {
        this.fileName = fileName;
        this.examinations = new List<Examination>();
        this.examinationsById = new Dictionary<Int32, Examination>();
        this.LoadRequests();
    }

    private static ExaminationRepository instance = null;

    public static ExaminationRepository GetInstance()
    {
        {
            if (instance == null)
            {
                instance = new ExaminationRepository(@"..\..\..\Data\JSON\examinations.json");
            }
            return instance;
        }
    }

    public void LoadRequests()
    {
        var examinations = JsonSerializer.Deserialize<List<Examination>>(File.ReadAllText(@"..\..\..\Data\JSON\examinations.json"), options);
        foreach (Examination examination in examinations)
        {
            this.examinations.Add(examination);
            this.examinationsById.Add(examination.id, examination);
        }
    }

    public void SaveExaminations()
    {
        var allExaminations = JsonSerializer.Serialize(this.examinations, options);
        File.WriteAllText(this.fileName, allExaminations);
    }

    public List<Examination> GetExaminations()
    {
        return this.examinations;
    }

    public Examination GetExaminationById(int id)
    {
        return this.examinationsById[id];
    }

    public void AddExamination(Appointment appointment, ExaminationStatus status, Room room, Doctor doctor, MedicalRecord medicalRecord, string anamnesis)
    {
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        Examination examination = new Examination(unixTimestamp, appointment,status,room,doctor,medicalRecord,anamnesis);
        this.examinations.Add(examination);
        this.examinationsById.Add(unixTimestamp, examination);
        SaveExaminations();
    }

    public void SwapExamination(Examination examination)
    {
        var oldExamination = examinationsById[examination.id];
        if (oldExamination != null)
        {
            this.examinations.Remove(oldExamination);
            this.examinations.Add(examination);
            this.examinationsById[examination.id] = examination;
        }
    }

    public void DeleteExaminations(int id)
    {
        Examination examination = GetExaminationById(id);
        if (examination != null)
        {
            this.examinationsById.Remove(examination.id);
            this.examinations.Remove(examination);
            SaveExaminations();
        }
    }
}