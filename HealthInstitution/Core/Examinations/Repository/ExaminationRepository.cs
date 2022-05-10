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
        this._fileName = fileName;
        this.Examinations = new List<Examination>();
        this.ExaminationsById = new Dictionary<int, Examination>();
        this._maxId = 0;
        this.LoadFromFile();
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

    public void LoadFromFile()
    {
        var roomsById = RoomRepository.GetInstance().RoomById;
        var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().MedicalRecordByUsername;
        var allExaminations = JArray.Parse(File.ReadAllText(this._fileName));
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

            if (id > _maxId) { _maxId = id; }

            this.Examinations.Add(loadedExamination);
            this.ExaminationsById.Add(id, loadedExamination);
        }
    }

    public void Save()
    {
        List<dynamic> reducedExaminations = new List<dynamic>();
        foreach (Examination examination in this.Examinations)
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
        var allExaminations = JsonSerializer.Serialize(reducedExaminations, _options);
        File.WriteAllText(this._fileName, allExaminations);
    }

    public List<Examination> GetAll()
    {
        return this.Examinations;
    }

    public Examination GetById(int examinationId)
    {
        if (ExaminationsById.ContainsKey(examinationId))
        {
            return ExaminationsById[examinationId];
        }
        return null;
    }

    public void AddExamination(DateTime appointment, Room room, Doctor doctor, MedicalRecord medicalRecord)
    {
        int id = ++this._maxId;
        Examination examination = new Examination(id, ExaminationStatus.Scheduled, appointment, room, doctor, medicalRecord, "");
        doctor.Examinations.Add(examination);
        this.Examinations.Add(examination);
        this.ExaminationsById.Add(id, examination);
        Save();
    }

    public void UpdateExamination(int id, DateTime appointment, MedicalRecord medicalRecord)
    {
        Examination examination = this.ExaminationsById[id];
        Doctor doctor = examination.Doctor;
        CheckIfDoctorIsAvailable(doctor, appointment);
        CheckIfPatientIsAvailable(medicalRecord.Patient, appointment);
        examination.Appointment = appointment;
        examination.MedicalRecord = medicalRecord;
        this.ExaminationsById[id] = examination;
        Save();
    }

    public void DeleteExamination(int id)
    {
        Examination examination = this.ExaminationsById[id];
        if (examination != null)
        {
            this.ExaminationsById.Remove(examination.Id);
            this.Examinations.Remove(examination);
            this.ExaminationsById.Remove(id);
            Save();
        }
    }

    private bool IsExaminationInOperationTime(Operation operation, DateTime appointment)
    {
        if (appointment >= operation.Appointment && appointment.AddMinutes(15) <= operation.Appointment.AddMinutes(operation.Duration))
            return true;
        return false;
    }

    private void CheckIfDoctorIsAvailable(Doctor doctor, DateTime dateTime)
    {
        foreach (var examination in doctor.Examinations)
        {
            if (examination.Appointment == dateTime)
            {
                throw new Exception("That doctor is not available");
            }
        }

        foreach (var operation in doctor.Operations)
        {
            if (operation.Appointment <= dateTime && operation.Appointment.AddMinutes(operation.Duration) >= dateTime)
            {
                throw new Exception("That doctor is not available");
            }

            if (operation.Appointment <= dateTime.AddMinutes(15) && operation.Appointment.AddMinutes(operation.Duration) >= dateTime.AddMinutes(15))
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
        var allExaminations = this.Examinations;
        var allOperations = OperationRepository.GetInstance().Operations;
        foreach (var examination in allExaminations)
        {
            if (examination.MedicalRecord.Patient.Username == patient.Username)
            {
                if (examination.Appointment == dateTime)
                {
                    throw new Exception("That patient is not available");
                }
            }
        }
        foreach (var operation in allOperations)
        {
            if (operation.MedicalRecord.Patient.Username == patient.Username)
            {
                if (operation.Appointment <= dateTime && operation.Appointment.AddMinutes(operation.Duration) >= dateTime)
                {
                    throw new Exception("That patient is not available");
                }
                if (operation.Appointment <= dateTime.AddMinutes(15) && operation.Appointment.AddMinutes(operation.Duration) >= dateTime.AddMinutes(15))
                {
                    throw new Exception("That patient is not available");
                }
                if (IsExaminationInOperationTime(operation, dateTime))
                {
                    throw new Exception("That patient is not available");
                }
            }
        }
    }

    private Room FindAvailableRoom(DateTime dateTime)
    {
        bool isAvailable;
        List<Room> availableRooms = new List<Room>();
        foreach (var room in RoomRepository.GetInstance().GetAll())
        {
            if (room.Type != RoomType.ExaminationRoom) continue;
            isAvailable = true;
            foreach (var examination in ExaminationRepository.GetInstance().Examinations)
            {
                if (examination.Appointment == dateTime && examination.Room.Id == room.Id)
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

    private List<Room> FindAllAvailableRooms(DateTime appointment)
    {
        bool isAvailable;
        List<Room> availableRooms = new List<Room>();
        foreach (var room in RoomRepository.GetInstance().GetAll())
        {
            if (room.Type != RoomType.ExaminationRoom) continue;
            isAvailable = true;
            foreach (var examination in Examinations)
            {
                if (examination.Appointment == appointment && examination.Room.Id == room.Id)
                {
                    isAvailable = false;
                    break;
                }
            }
            if (isAvailable)
                availableRooms.Add(room);
        }
        return availableRooms;

    }

    public static void FillExaminationsForPriority(List<Examination> nextTwoHoursExaminations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
    {
        int appointmentCounter;
        TimeSpan ts = new TimeSpan(0, 15, 0);
        DateTime firstAvailableAppointment;
        foreach (Examination examination in nextTwoHoursExaminations)
        {
            appointmentCounter = 1;

            while (true)
            {
                firstAvailableAppointment = examination.Appointment + appointmentCounter * ts;
                if (firstAvailableAppointment.Hour > 22)
                {
                    firstAvailableAppointment += new TimeSpan(9, 0, 0);
                }
                appointmentCounter++;
                try
                {
                    ExaminationRepository.GetInstance().CheckIfDoctorIsAvailable(examination.Doctor, firstAvailableAppointment);
                    ExaminationRepository.GetInstance().CheckIfPatientIsAvailable(examination.MedicalRecord.Patient, firstAvailableAppointment);
                }
                catch
                {
                    continue;
                }

                if (ExaminationRepository.GetInstance().FindAllAvailableRooms(firstAvailableAppointment).Contains(examination.Room))
                {
                    priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(examination.Id, 1, firstAvailableAppointment));
                    break;
                }

            }
        }
    }
    
    public static void FillOperationsForPriority(List<Operation> nextTwoHoursOperations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
    {
        int appointmentCounter;
        TimeSpan ts = new TimeSpan(0, 15, 0);
        DateTime firstAvailableAppointment;
        foreach (Operation operation in nextTwoHoursOperations)
        {
            ts = new TimeSpan(0, operation.Duration, 0);
            appointmentCounter = 1;

            while (true)
            {
                firstAvailableAppointment = operation.Appointment + appointmentCounter * ts;
                if (firstAvailableAppointment.Hour>22)
                {
                    firstAvailableAppointment += new TimeSpan(9, 0, 0);
                }
                appointmentCounter++;
                try
                {
                    ExaminationRepository.GetInstance().CheckIfDoctorIsAvailable(operation.Doctor, firstAvailableAppointment);
                    ExaminationRepository.GetInstance().CheckIfPatientIsAvailable(operation.MedicalRecord.Patient, firstAvailableAppointment);
                }
                catch
                {
                    continue;
                }

                if (ExaminationRepository.GetInstance().FindAllAvailableRooms(firstAvailableAppointment).Contains(operation.Room))
                {
                    priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(operation.Id, 0, firstAvailableAppointment));
                    break;
                }

            }
        }
    }
    public static List<Tuple<int, int, DateTime>> GetPriorityExaminationsAndOperations(List<Examination> nextTwoHoursExaminations, List<Operation> nextTwoHoursOperations)
    {
        List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int,int, DateTime>>();
        FillExaminationsForPriority(nextTwoHoursExaminations, priorityExaminationsAndOperations);
        FillOperationsForPriority(nextTwoHoursOperations, priorityExaminationsAndOperations);

        priorityExaminationsAndOperations.Sort((x, y) => y.Item3.CompareTo(x.Item3));
        return priorityExaminationsAndOperations;
    }

    public static List<Tuple<int, int, DateTime>> FindClosest(List<DateTime> nextTwoHoursAppointments)
    {
        List<Examination> nextTwoHoursExaminations = new List<Examination>();
        List<Operation> nextTwoHoursOperations = new List<Operation>();
        foreach (Examination examination in ExaminationRepository.GetInstance().Examinations)
        {
            if (nextTwoHoursAppointments.Contains(examination.Appointment))
                nextTwoHoursExaminations.Add(examination);
        }
        foreach (Operation operation in OperationRepository.GetInstance().GetAll())
        {
            if (nextTwoHoursAppointments.Contains(operation.Appointment))
                nextTwoHoursOperations.Add(operation);
        }
        return GetPriorityExaminationsAndOperations(nextTwoHoursExaminations,nextTwoHoursOperations);
        
    }

    public static List<DateTime> FindNextTwoHoursAppointments()
    {
        List<DateTime> possibleAppointments = new List<DateTime>();
        DateTime current = DateTime.Now;
        DateTime firstAppointment=current;
        if (current.Minute > 0) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 15, 0);
        if (current.Minute > 15) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 30, 0);
        if (current.Minute > 30) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 45, 0);
        if (current.Minute > 45) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour + 1, 0, 0);
        for (int i=0;i<=7;i++)
        {
            TimeSpan ts = new TimeSpan(0,15,0);
            possibleAppointments.Add(firstAppointment + i * ts);
            /*if ((firstAppointment + i * ts).Hour < 23)
                possibleAppointments.Add(firstAppointment + i * ts);
            else
                break;*/
        }
        return possibleAppointments;
    }

    public void SwapExaminationValue(Examination examination)
    {
        var oldExamination = this.ExaminationsById[examination.Id];
        this.Examinations.Remove(oldExamination);
        this.Examinations.Add(examination);
        examination.Doctor.Examinations.Add(examination);
        oldExamination.Doctor.Examinations.Remove(oldExamination);
        this.ExaminationsById[examination.Id] = examination;
        Save();
    }

    public Examination GenerateRequestExamination(Examination examination, string patientUsername, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetById(doctorUsername);
        CheckIfDoctorIsAvailable(doctor, dateTime);
        var room = FindAvailableRoom(dateTime);
        Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
        Examination e = new Examination(examination.Id, examination.Status, dateTime, room, doctor, examination.MedicalRecord, "");
        return e;
    }

    public void EditExamination(Examination examination, string patientUsername, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetById(doctorUsername);
        Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
        CheckIfDoctorIsAvailable(doctor, dateTime);
        CheckIfPatientIsAvailable(patient, dateTime);
        var room = FindAvailableRoom(dateTime);       
        Examination e = new Examination(examination.Id, examination.Status, dateTime, room, doctor, examination.MedicalRecord, "");
        SwapExaminationValue(e);
    }

    public void ReserveExamination(string patientUsername, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetById(doctorUsername);
        Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
        CheckIfDoctorIsAvailable(doctor, dateTime);
        CheckIfPatientIsAvailable(patient, dateTime);
        var room = FindAvailableRoom(dateTime);
        var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
        AddExamination(dateTime, room, doctor, medicalRecord);
    }

    public List<Tuple<int,int,DateTime>> ReserveUrgentExamination(string patientUsername, SpecialtyType specialtyType)
    {
        List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
        Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
        List<DateTime> nextTwoHoursAppointments=FindNextTwoHoursAppointments();
        foreach(DateTime appointment in nextTwoHoursAppointments)
        {
            foreach (Doctor doctor in DoctorRepository.GetInstance().Doctors)
            {
                if (doctor.Specialty == specialtyType)
                {
                    try {
                        CheckIfDoctorIsAvailable(doctor, appointment);
                        CheckIfPatientIsAvailable(patient, appointment);
                        var room = FindAvailableRoom(appointment);
                        var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
                        AddExamination(appointment, room, doctor, medicalRecord);
                        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(this._maxId, 2, appointment));
                        return priorityExaminationsAndOperations;
                    }
                    catch
                    {
                        continue;
                    }
                    
                }
            }
        }
        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(this._maxId, 2, new DateTime(1, 1, 1)));
        List < Tuple<int, int, DateTime> > temporaryPriority = FindClosest(nextTwoHoursAppointments);
        foreach(Tuple<int, int, DateTime> tuple in temporaryPriority)
        {
            priorityExaminationsAndOperations.Add(tuple);
        }
        return priorityExaminationsAndOperations;
        
        
        
    }
}