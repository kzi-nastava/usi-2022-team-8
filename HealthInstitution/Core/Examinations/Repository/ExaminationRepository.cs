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
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Operations;

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

    public List<Examination> GetPatientExaminations(Patient patient)
    {
        List<Examination> patientExaminations = new List<Examination>();
        foreach (var examination in Examinations)
            if (examination.MedicalRecord.Patient.Username == patient.Username)
                patientExaminations.Add(examination);
        return patientExaminations;
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
    public void Add(ExaminationDTO examinationDTO)
    {
        int id = ++_maxId;
        Examination examination = new Examination(id, ExaminationStatus.Scheduled, examinationDTO.Appointment, examinationDTO.Room, examinationDTO.Doctor, examinationDTO.MedicalRecord, "");
        AddToCollections(examination);
        SaveAll();
    }

    public void Add(Examination examination)
    {
        AddToCollections(examination);
    }

    private Examination GenerateExamination(ExaminationDTO examinationDTO)
    {
        int id = ++_maxId;
        Examination examination = new Examination(id, ExaminationStatus.Scheduled, examinationDTO.Appointment, examinationDTO.Room, examinationDTO.Doctor, examinationDTO.MedicalRecord, "");
        return examination;
    }

    private ExaminationDTO ParseExaminationToExaminationDTO(Examination examination)
    {
        return new ExaminationDTO(examination.Appointment, examination.Room, examination.Doctor, examination.MedicalRecord);
    }

    public void Update(int id, ExaminationDTO examinationDTO)
    {
        Examination examination = ExaminationsById[id];
        CheckIfDoctorIsAvailable(examinationDTO);
        CheckIfPatientIsAvailable(examinationDTO);
        examination.Appointment = examinationDTO.Appointment;
        examination.MedicalRecord = examinationDTO.MedicalRecord;
        ExaminationsById[id] = examination;
        Save();
    }

    public void Delete(int id)
    {
        Examination examination = ExaminationsById[id];
        ExaminationsById.Remove(examination.Id);
        Examinations.Remove(examination);
        ExaminationsById.Remove(id);
        SaveAll();  
    }

    public List<Examination> GetCompletedByPatient(string patientUsername)
    {
        List<Examination> completed = new List<Examination>();

        foreach (Examination examination in Examinations)
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

    private void CheckIfDoctorHasOperations(ExaminationDTO examinationDTO)
    {
        var doctor = examinationDTO.Doctor;
        DateTime appointment = examinationDTO.Appointment;

        foreach (var operation in doctor.Operations)
        {
            if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(15) > operation.Appointment))
            {
                throw new Exception("That doctor is not available");
            }
        }
    }

    public void CheckIfDoctorIsAvailable(ExaminationDTO examinationDTO)
    {
        CheckIfDoctorHasExaminations(examinationDTO);
        CheckIfDoctorHasOperations(examinationDTO);
    }

    private void CheckIfPatientHasExaminations(ExaminationDTO examinationDTO)
    {
        var patient = examinationDTO.MedicalRecord.Patient;
        DateTime appointment = examinationDTO.Appointment;
        var patientExaminations = GetPatientExaminations(patient);

        foreach (var examination in patientExaminations)
        {
            if (examination.Appointment == appointment)
            {
                throw new Exception("That patient is not available");
            }
        }
    }

    private void CheckIfPatientHasOperations(ExaminationDTO examinationDTO)
    {
        var patient = examinationDTO.MedicalRecord.Patient;
        DateTime appointment = examinationDTO.Appointment;
        var patientOperations = OperationService.GetPatientOperations(patient);

        foreach (var operation in patientOperations)
        {
            if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(15) > operation.Appointment))
            {
                throw new Exception("That patient is not available");
            }
        }
    }

    private void CheckIfPatientIsAvailable(ExaminationDTO examinationDTO)
    {
        CheckIfPatientHasExaminations(examinationDTO);
        CheckIfPatientHasOperations(examinationDTO);
    }

    private void CheckIfDoctorHasExaminations(ExaminationDTO examinationDTO)
    {
        var doctor = examinationDTO.Doctor;
        DateTime appointment = examinationDTO.Appointment;

        foreach (var examination in doctor.Examinations)
        {
            if (examination.Appointment == appointment)
            {
                throw new Exception("That doctor is not available");
            }
        }
    }
    public static DateTime FindFirstAvailableAppointment(DateTime appointment, int appointmentCounter, TimeSpan ts)
    {
        DateTime firstAvailableAppointment = appointment + appointmentCounter * ts;
        if (firstAvailableAppointment.Hour > 22)
        {
            firstAvailableAppointment += new TimeSpan(9, 0, 0);
        }
        return firstAvailableAppointment;
    }
    public static void GetExaminationsWithPriorities(List<Examination> nextTwoHoursExaminations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
    {
        int appointmentCounter; 
        DateTime firstAvailableAppointment;
        foreach (Examination examination in nextTwoHoursExaminations)
        {
            appointmentCounter = 1;
            while (true)
            {
                firstAvailableAppointment = FindFirstAvailableAppointment(examination.Appointment, appointmentCounter, new TimeSpan(0,15,0));
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
        DateTime firstAvailableAppointment;
        foreach (Operation operation in nextTwoHoursOperations)
        {
            appointmentCounter = 1;
            while (true)
            {
                firstAvailableAppointment = FindFirstAvailableAppointment(operation.Appointment, appointmentCounter, new TimeSpan(0, operation.Duration, 0));
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
        var oldExamination = ExaminationsById[examination.Id];
        Examinations.Remove(oldExamination);
        Examinations.Add(examination);
        examination.Doctor.Examinations.Add(examination);
        oldExamination.Doctor.Examinations.Remove(oldExamination);
        ExaminationsById[examination.Id] = examination;
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
    private void TrySchedulingUrgentExamination(DateTime appointment, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
    {
        ExaminationDTO examinationDTO = new ExaminationDTO(appointment, null, doctor, medicalRecord);
        CheckIfDoctorIsAvailable(examinationDTO);
        CheckIfPatientIsAvailable(examinationDTO);
        examinationDTO.Room = RoomRepository.GetInstance().FindAvailableRoom(appointment);
        Add(examinationDTO);
        AppointmentNotificationDTO appointmentNotificationDTO = new AppointmentNotificationDTO(null, appointment, doctor, medicalRecord.Patient);
        AppointmentNotificationRepository.GetInstance().Add(appointmentNotificationDTO);
        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(this._maxId, 2, appointment));
    }
    public List<Tuple<int, int, DateTime>> ReserveUrgentExamination(string patientUsername, SpecialtyType specialtyType)
    {
        List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
        Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
        var medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
        List<DateTime> nextTwoHoursAppointments = FindNextTwoHoursAppointments();
        foreach (DateTime appointment in nextTwoHoursAppointments)
        {
            foreach (Doctor doctor in DoctorRepository.GetInstance().Doctors)
            {
                if (doctor.Specialty == specialtyType)
                {
                    try
                    {
                        TrySchedulingUrgentExamination(appointment, doctor, medicalRecord, priorityExaminationsAndOperations);
                        return priorityExaminationsAndOperations;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(_maxId + 1, 2, new DateTime(1, 1, 1)));
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
        var medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
        ExaminationDTO examinationDTO = new ExaminationDTO(fit, null, doctor, medicalRecord);
        FindFitDTO findFitDTO = new FindFitDTO(fit, firstFitDTO.end, firstFitDTO.minHour, firstFitDTO.minMinutes, firstFitDTO.maxHour, firstFitDTO.maxMinutes);
        ExaminationDTO firstFit = FindFit(examinationDTO, findFitDTO);
        if (firstFit is not null)
        {
            found = true;
            Add(examinationDTO);
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