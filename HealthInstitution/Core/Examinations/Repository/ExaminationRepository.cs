using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations.Repository;

internal class ExaminationRepository
{
    public String fileName { get; set; }
    public int maxId { get; set; }
    public List<Examination> examinations { get; set; }
    public Dictionary<int, Examination> examinationsById { get; set; }

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private ExaminationRepository(String fileName)
    {
        this.fileName = fileName;
        this.examinations = new List<Examination>();
        this.examinationsById = new Dictionary<int, Examination>();
        this.maxId = 0;
        this.LoadExaminations();
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

    public void LoadExaminations()
    {
        var roomsById = RoomRepository.GetInstance().roomById;
        //var doctorsByUsername = DoctorRepository.GetInstance().doctorsByUsername;
        var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().medicalRecordByUsername;
        var examinations = JArray.Parse(File.ReadAllText(this.fileName));
        foreach (var examination in examinations)
        {
            int id = (int)examination["id"];
            ExaminationStatus status;
            Enum.TryParse(examination["status"].ToString(), out status);
            DateTime appointment = (DateTime)examination["appointment"];
            int roomId = (int)examination["room"];
            Room room = roomsById[roomId];
            String doctorUsername = (String)examination["doctor"];
            //Doctor doctor = doctorsByUsername[doctorUsername];
            String patientUsername = (String)examination["medicalRecord"];
            MedicalRecord medicalRecord = medicalRecordsByUsername[patientUsername];
            String anamnesis = (String)examination["anamnesis"];

            Examination loadedExamination = new Examination(id, status, appointment, room, null, medicalRecord, anamnesis);

            if (id > maxId) { maxId = id; }

            this.examinations.Add(loadedExamination);
            this.examinationsById.Add(id, loadedExamination);
            this.maxId += this.examinations.Count();
        }
    }

    public List<dynamic> ShortenExamination()
    {
        List<dynamic> reducedExaminations = new List<dynamic>();
        foreach (Examination examination in this.examinations)
        {
            reducedExaminations.Add(new
            {
                id = examination.id,
                status = examination.status,
                appointment = examination.appointment,
                room = examination.room.id,
                medicalRecord = examination.medicalRecord.patient.username,
                anamnesis = examination.anamnesis
            });
        }
        return reducedExaminations;
    }

    public void SaveExaminations()
    {
        var allExaminations = JsonSerializer.Serialize(ShortenExamination(), options);
        File.WriteAllText(this.fileName, allExaminations);
    }

    public List<Examination> GetExaminations()
    {
        return this.examinations;
    }

    public Examination GetExaminationById(int id)
    {
        if (examinationsById.ContainsKey(id))
        {
            return examinationsById[id];
        }
        return null;
    }

    public void AddExamination(DateTime appointment, Room room, Doctor doctor, MedicalRecord medicalRecord)
    {
        int id = ++this.maxId;
        Examination examination = new Examination(id, ExaminationStatus.Scheduled, appointment, room, doctor, medicalRecord, "");
        this.examinations.Add(examination);
        this.examinationsById.Add(id, examination);
        SaveExaminations();
    }

    public void UpdateExamination(int id, DateTime appointment, MedicalRecord medicalRecord)
    {
        Examination examination = GetExaminationById(id);
        examination.appointment = appointment;
        examination.medicalRecord = medicalRecord;
        this.examinationsById[id] = examination;
        SaveExaminations();
    }

    public void DeleteExamination(int id)
    {
        Examination examination = GetExaminationById(id);
        if (examination != null)
        {
            this.examinationsById.Remove(examination.id);
            this.examinations.Remove(examination);
            this.examinationsById.Remove(id);
            SaveExaminations();
        }
    }

    private void CheckIfDoctorIsAvailable(Doctor doctor, DateTime dateTime)
    {
        foreach (var examination in doctor.examinations)
        {
            if (examination.appointment == dateTime)
            {
                throw new Exception("That doctor is not available");
            }
        }

        foreach (var operation in doctor.operations)
        {
            if (operation.appointment <= dateTime && operation.appointment.AddMinutes(operation.duration) >= dateTime)
            {
                throw new Exception("That doctor is not available");
            }

            if (operation.appointment <= dateTime.AddMinutes(15) && operation.appointment.AddMinutes(operation.duration) >= dateTime.AddMinutes(15))
            {
                throw new Exception("That doctor is not available");
            }
        }
    }

    private Room FindAvailableRoom(DateTime dateTime)
    {
        bool isAvailable;
        List<Room> availableRooms = new List<Room>();
        foreach (var room in RoomRepository.GetInstance().GetRooms())
        {
            if (room.type != RoomType.ExaminationRoom) continue;
            isAvailable = true;
            foreach (var examination in ExaminationRepository.GetInstance().examinations)
            {
                if (examination.appointment == dateTime && examination.room.id == room.id)
                {
                    isAvailable = false;
                    break;
                }
            }
            if (isAvailable)
                availableRooms.Add(room);
        }

        if (availableRooms.Count == 0) throw new Exception("There are no available rooms!");
        return availableRooms[0];
    }

    public void SwapExaminationValue(Examination examination)
    {
        var oldExamination = this.GetExaminationById(examination.id);
        this.examinations.Remove(oldExamination);
        this.examinations.Add(examination);
        this.examinationsById[examination.id] = examination;
    }

    public Examination GenerateRequestExamination(Examination examination, string patientUsername, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetDoctorByUsername(doctorUsername);
        CheckIfDoctorIsAvailable(doctor, dateTime);
        var room = FindAvailableRoom(dateTime);
        Patient patient = PatientRepository.GetInstance().GetPatientByUsername(patientUsername);
        Examination e = new Examination(examination.id, examination.status, dateTime, room, doctor, examination.medicalRecord, "");
        return e;
    }

    public void EditExamination(Examination examination, string patientUsername, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetDoctorByUsername(doctorUsername);
        CheckIfDoctorIsAvailable(doctor, dateTime);
        var room = FindAvailableRoom(dateTime);
        Patient patient = PatientRepository.GetInstance().GetPatientByUsername(patientUsername);
        Examination e = new Examination(examination.id, examination.status, dateTime, room, doctor, examination.medicalRecord, "");
        SwapExaminationValue(e);
        // da li moze samo examination.dateTime = dateTime...
    }

    public void ReserveExamination(string patientUsername, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetDoctorByUsername(doctorUsername);
        CheckIfDoctorIsAvailable(doctor, dateTime);
        var room = FindAvailableRoom(dateTime);
        Patient patient = PatientRepository.GetInstance().GetPatientByUsername(patientUsername);
        var medicalRecord = MedicalRecordRepository.GetInstance().GetMedicalRecordByUsername(patient);
        ExaminationRepository.GetInstance().AddExamination(dateTime, room, doctor, medicalRecord);
    }
}