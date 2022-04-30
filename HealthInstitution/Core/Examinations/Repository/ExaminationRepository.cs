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
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Operations.Model;

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
        this.LoadFromFile();
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

    public void LoadFromFile()
    {
        var roomsById = RoomRepository.GetInstance().roomById;
        var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().medicalRecordByUsername;
        var allExaminations = JArray.Parse(File.ReadAllText(this.fileName));
        foreach (var examination in allExaminations)
        {
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

            Examination loadedExamination = new Examination(id, status, appointment, room, null, medicalRecord, anamnesis);

            if (id > maxId) { maxId = id; }

            this.examinations.Add(loadedExamination);
            this.examinationsById.Add(id, loadedExamination);
        }
    }

    public void SaveToFile()
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
        var allExaminations = JsonSerializer.Serialize(reducedExaminations, options);
        File.WriteAllText(this.fileName, allExaminations);
    }

    public List<Examination> Get()
    {
        return this.examinations;
    }

    public Examination GetById(int examinationId)
    {
        if (examinationsById.ContainsKey(examinationId))
        {
            return examinationsById[examinationId];
        }
        return null;
    }

    public void AddExamination(DateTime appointment, Room room, Doctor doctor, MedicalRecord medicalRecord)
    {
        int id = ++this.maxId;
        Examination examination = new Examination(id, ExaminationStatus.Scheduled, appointment, room, doctor, medicalRecord, "");
        doctor.examinations.Add(examination);
        this.examinations.Add(examination);
        this.examinationsById.Add(id, examination);
        SaveToFile();
    }

    public void UpdateExamination(int id, DateTime appointment, MedicalRecord medicalRecord)
    {
        Examination examination = this.examinationsById[id];
        Doctor doctor = examination.doctor;
        CheckIfDoctorIsAvailable(doctor, appointment);
        CheckIfPatientIsAvailable(medicalRecord.patient, appointment);
        examination.appointment = appointment;
        examination.medicalRecord = medicalRecord;
        this.examinationsById[id] = examination;
        SaveToFile();
    }

    public void DeleteExamination(int id)
    {
        Examination examination = this.examinationsById[id];
        if (examination != null)
        {
            this.examinationsById.Remove(examination.id);
            this.examinations.Remove(examination);
            this.examinationsById.Remove(id);
            SaveToFile();
        }
    }

    private bool IsExaminationInOperationTime(Operation operation, DateTime appointment)
    {
        if (appointment >= operation.appointment && appointment.AddMinutes(15) <= operation.appointment.AddMinutes(operation.duration))
            return true;
        return false;
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
            if (IsExaminationInOperationTime(operation, dateTime))
            {
                throw new Exception("That doctor is not available");
            }
        }
    }

    private void CheckIfPatientIsAvailable(Patient patient, DateTime dateTime)
    {
        var allExaminations = this.examinations;
        var allOperations = OperationRepository.GetInstance().operations;
        foreach (var examination in allExaminations)
        {
            if (examination.medicalRecord.patient.username == patient.username)
            {
                if (examination.appointment == dateTime)
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }
        foreach (var operation in allOperations)
        {
            if (operation.medicalRecord.patient.username == patient.username)
            {
                if (operation.appointment <= dateTime && operation.appointment.AddMinutes(operation.duration) >= dateTime)
                {
                    throw new Exception("That doctor is not available");
                }
                if (operation.appointment <= dateTime.AddMinutes(15) && operation.appointment.AddMinutes(operation.duration) >= dateTime.AddMinutes(15))
                {
                    throw new Exception("That doctor is not available");
                }
                if (IsExaminationInOperationTime(operation, dateTime))
                {
                    throw new Exception("That doctor is not available");
                }
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
        var oldExamination = this.examinationsById[examination.id];
        this.examinations.Remove(oldExamination);
        this.examinations.Add(examination);
        examination.doctor.examinations.Add(examination);
        oldExamination.doctor.examinations.Remove(oldExamination);
        this.examinationsById[examination.id] = examination;
        SaveToFile();
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
        Patient patient = PatientRepository.GetInstance().GetPatientByUsername(patientUsername);
        CheckIfDoctorIsAvailable(doctor, dateTime);
        CheckIfPatientIsAvailable(patient, dateTime);
        var room = FindAvailableRoom(dateTime);       
        Examination e = new Examination(examination.id, examination.status, dateTime, room, doctor, examination.medicalRecord, "");
        SwapExaminationValue(e);
    }

    public void ReserveExamination(string patientUsername, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetDoctorByUsername(doctorUsername);
        Patient patient = PatientRepository.GetInstance().GetPatientByUsername(patientUsername);
        CheckIfDoctorIsAvailable(doctor, dateTime);
        CheckIfPatientIsAvailable(patient, dateTime);
        var room = FindAvailableRoom(dateTime);
        var medicalRecord = MedicalRecordRepository.GetInstance().GetMedicalRecordByUsername(patient);
        AddExamination(dateTime, room, doctor, medicalRecord);
    }
}