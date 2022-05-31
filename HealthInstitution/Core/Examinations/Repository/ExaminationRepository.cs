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
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.RecommededDTO;
using HealthInstitution.Core.Notifications.Model;

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
        var allExaminations = JArray.Parse(File.ReadAllText(this._fileName));
        foreach (var examination in allExaminations)
        {
            Examination loadedExamination = Parse(examination);
            int id = loadedExamination.Id;
            if (id > _maxId) { _maxId = id; }

            this.Examinations.Add(loadedExamination);
            this.ExaminationsById.Add(id, loadedExamination);
        }
    }

    private List<dynamic> PrepareForSerialization()
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
        return reducedExaminations;
    }

    public void Save()
    {
        List<dynamic> reducedExaminations = PrepareForSerialization();
        var allExaminations = JsonSerializer.Serialize(reducedExaminations, _options);
        File.WriteAllText(this._fileName, allExaminations);
    }

    public List<Examination> GetAll()
    {
        return this.Examinations;
    }

    public Examination GetById(int id)
    {
        if (ExaminationsById.ContainsKey(id))
        {
            return ExaminationsById[id];
        }
        return null;
    }

    public List<Examination> GetPatientExaminations(Patient patient)
    {
        List<Examination> patientExaminations = new List<Examination>();
        foreach (var examination in this.Examinations)
            if (examination.MedicalRecord.Patient.Username == patient.Username)
                patientExaminations.Add(examination);
        return patientExaminations;
    }

    private void AddToContainers(Examination examination)
    {
        examination.Doctor.Examinations.Add(examination);
        this.Examinations.Add(examination);
        this.ExaminationsById.Add(examination.Id, examination);

        Save();
        ExaminationDoctorRepository.GetInstance().Save();
    }

    public void Add(ExaminationDTO examinationDTO)
    {
        int id = ++this._maxId;
        DateTime appointment = examinationDTO.Appointment;
        Room room = examinationDTO.Room;
        Doctor doctor = examinationDTO.Doctor;
        MedicalRecord medicalRecord = examinationDTO.MedicalRecord;

        Examination examination = new Examination(id, ExaminationStatus.Scheduled, appointment, room, doctor, medicalRecord, "");
        doctor.Examinations.Add(examination);
        this.Examinations.Add(examination);
        this.ExaminationsById.Add(id, examination);

        Save();
        ExaminationDoctorRepository.GetInstance().Save();
    }

    public void Add(Examination examination)
    {
        AddToContainers(examination);
    }

    private Examination GenerateExamination(ExaminationDTO examinationDTO)
    {
        int id = ++this._maxId;
        DateTime appointment = examinationDTO.Appointment;
        Room room = examinationDTO.Room;
        Doctor doctor = examinationDTO.Doctor;
        MedicalRecord medicalRecord = examinationDTO.MedicalRecord;

        Examination examination = new Examination(id, ExaminationStatus.Scheduled, appointment, room, doctor, medicalRecord, "");
        return examination;
    }

    private ExaminationDTO ParseExaminationToExaminationDTO(Examination examination)
    {
        return new ExaminationDTO(examination.Appointment, examination.Room, examination.Doctor, examination.MedicalRecord);
    }

    public void Update(int id, ExaminationDTO examinationDTO)
    {
        Examination examination = this.ExaminationsById[id];

        CheckIfDoctorIsAvailable(examinationDTO);
        CheckIfPatientIsAvailable(examinationDTO);
        examination.Appointment = examinationDTO.Appointment;
        examination.MedicalRecord = examinationDTO.MedicalRecord;
        this.ExaminationsById[id] = examination;
        Save();
    }

    public void Delete(int id)
    {
        Examination examination = this.ExaminationsById[id];
        this.ExaminationsById.Remove(examination.Id);
        this.Examinations.Remove(examination);
        this.ExaminationsById.Remove(id);
        Save();
        ExaminationDoctorRepository.GetInstance().Save();
    }

    public List<Examination> GetCompletedByPatient(string patientUsername)
    {
        List<Examination> completed = new List<Examination>();

        foreach (Examination examination in this.Examinations)
        {
            if (examination.MedicalRecord.Patient.Username != patientUsername) continue;
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

    public static void FindFirstAvailableAppointment()
    { 
    }
    public static void GetExaminationsWithPriorities(List<Examination> nextTwoHoursExaminations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
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
                    ExaminationDTO examinationDTO = new ExaminationDTO(firstAvailableAppointment, null, examination.Doctor, examination.MedicalRecord);
                    GetInstance().CheckIfDoctorIsAvailable(examinationDTO);
                    GetInstance().CheckIfPatientIsAvailable(examinationDTO);
                }
                catch
                {
                    continue;
                }

                if (RoomRepository.GetInstance().FindAllAvailableRooms(firstAvailableAppointment).Contains(examination.Room))
                {
                    priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(examination.Id, 1, firstAvailableAppointment));
                    break;
                }
            }
        }
    }


    public static void GetOperationsWithPriorities(List<Operation> nextTwoHoursOperations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
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
                if (firstAvailableAppointment.Hour > 22)
                {
                    firstAvailableAppointment += new TimeSpan(9, 0, 0);
                }
                appointmentCounter++;
                try
                {
                    ExaminationDTO examinationDTO = new ExaminationDTO(firstAvailableAppointment, null, operation.Doctor, operation.MedicalRecord);
                    GetInstance().CheckIfDoctorIsAvailable(examinationDTO);
                    GetInstance().CheckIfPatientIsAvailable(examinationDTO);
                }
                catch
                {
                    continue;
                }

                if (RoomRepository.GetInstance().FindAllAvailableRooms(firstAvailableAppointment).Contains(operation.Room))
                {
                    priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(operation.Id, 0, firstAvailableAppointment));
                    break;
                }
            }
        }
    }

    public static List<Tuple<int, int, DateTime>> GetPriorityExaminationsAndOperations(List<Examination> nextTwoHoursExaminations, List<Operation> nextTwoHoursOperations)
    {
        List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
        GetExaminationsWithPriorities(nextTwoHoursExaminations, priorityExaminationsAndOperations);
        GetOperationsWithPriorities(nextTwoHoursOperations, priorityExaminationsAndOperations);

        priorityExaminationsAndOperations.Sort((x, y) => y.Item3.CompareTo(x.Item3));
        return priorityExaminationsAndOperations;
    }

    public static List<Tuple<int, int, DateTime>> FindClosest(List<DateTime> nextTwoHoursAppointments, SpecialtyType specialtyType)
    {
        List<Examination> nextTwoHoursExaminations = new List<Examination>();
        List<Operation> nextTwoHoursOperations = new List<Operation>();
        foreach (Examination examination in GetInstance().Examinations)
        {
            if (nextTwoHoursAppointments.Contains(examination.Appointment) && examination.Doctor.Specialty == specialtyType)
                nextTwoHoursExaminations.Add(examination);
        }
        foreach (Operation operation in OperationRepository.GetInstance().GetAll())
        {
            if (nextTwoHoursAppointments.Contains(operation.Appointment) && operation.Doctor.Specialty == specialtyType)
                nextTwoHoursOperations.Add(operation);
        }
        return GetPriorityExaminationsAndOperations(nextTwoHoursExaminations, nextTwoHoursOperations);
    }

    public static List<DateTime> FindNextTwoHoursAppointments()
    {
        List<DateTime> possibleAppointments = new List<DateTime>();
        DateTime current = DateTime.Now;
        DateTime firstAppointment = current;

        if (current.Minute > 0) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 15, 0);
        if (current.Minute > 15) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 30, 0);
        if (current.Minute > 30) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 45, 0);
        if (current.Minute > 45) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour + 1, 0, 0);

        for (int i = 0; i <= 7; i++)
        {
            TimeSpan ts = new TimeSpan(0, 15, 0);
            possibleAppointments.Add(firstAppointment + i * ts);
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

    public Examination GenerateRequestExamination(Examination examination, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetById(doctorUsername);
        var examinatioDTO = ParseExaminationToExaminationDTO(examination);
        examinatioDTO.Appointment = dateTime;
        CheckIfDoctorIsAvailable(examinatioDTO);
        CheckIfPatientIsAvailable(examinatioDTO);
        var room = RoomRepository.GetInstance().FindAvailableRoom(dateTime);
        Examination e = new Examination(examination.Id, examination.Status, dateTime, room, doctor, examination.MedicalRecord, "");
        return e;
    }

    public void EditExamination(Examination examination, string doctorUsername, DateTime dateTime)
    {
        Doctor doctor = DoctorRepository.GetInstance().GetById(doctorUsername);
        var examinatioDTO = ParseExaminationToExaminationDTO(examination);
        examinatioDTO.Appointment = dateTime;
        CheckIfDoctorIsAvailable(examinatioDTO);
        CheckIfPatientIsAvailable(examinatioDTO);
        var room = RoomRepository.GetInstance().FindAvailableRoom(dateTime);
        Examination e = new Examination(examination.Id, examination.Status, dateTime, room, doctor, examination.MedicalRecord, "");
        SwapExaminationValue(e);
    }

    public void ReserveExamination(ExaminationDTO examinationDTO)
    {
        CheckIfDoctorIsAvailable(examinationDTO);
        CheckIfPatientIsAvailable(examinationDTO);
        examinationDTO.Room = RoomRepository.GetInstance().FindAvailableRoom(examinationDTO.Appointment);
        Add(examinationDTO);
    }

    public List<Tuple<int, int, DateTime>> ReserveUrgentExamination(string patientUsername, SpecialtyType specialtyType)
    {
        List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
        Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
        var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
        List<DateTime> nextTwoHoursAppointments = FindNextTwoHoursAppointments();
        foreach (DateTime appointment in nextTwoHoursAppointments)
        {
            foreach (Doctor doctor in DoctorRepository.GetInstance().Doctors)
            {
                if (doctor.Specialty == specialtyType)
                {
                    try
                    {
                        ExaminationDTO examinationDTO = new ExaminationDTO(appointment, null, doctor, medicalRecord);
                        CheckIfDoctorIsAvailable(examinationDTO);
                        CheckIfPatientIsAvailable(examinationDTO);
                        examinationDTO.Room = RoomRepository.GetInstance().FindAvailableRoom(appointment);
                        Add(examinationDTO);
                        AppointmentNotificationDTO appointmentNotificationDTO = new AppointmentNotificationDTO(null, appointment, doctor, patient);
                        AppointmentNotificationRepository.GetInstance().Add(appointmentNotificationDTO);
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
        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(this._maxId + 1, 2, new DateTime(1, 1, 1)));
        priorityExaminationsAndOperations.AddRange(FindClosest(nextTwoHoursAppointments, specialtyType));
        return priorityExaminationsAndOperations;
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
        var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
        ExaminationDTO examinationDTO = new ExaminationDTO(fit, null, doctor, medicalRecord);
        FindFitDTO findFitDTO = new FindFitDTO(fit, firstFitDTO.end, firstFitDTO.minHour, firstFitDTO.minMinutes, firstFitDTO.maxHour, firstFitDTO.maxMinutes);
        ExaminationDTO firstFit = FindFit(examinationDTO, findFitDTO);
        if (firstFit is not null)
        {
            found = true;
            this.Add(examinationDTO);
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
        var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
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
                    suggestions.Add(GenerateExamination(firstFit));
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