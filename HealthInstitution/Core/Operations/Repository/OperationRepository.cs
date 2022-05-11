using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations.Model;
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

namespace HealthInstitution.Core.Operations.Repository
{
    internal class OperationRepository
    {
        private String _fileName;
        public int _maxId { get; set; }
        public List<Operation> Operations { get; set; }
        public Dictionary<int, Operation> OperationsById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        private OperationRepository(String fileName)
        {
            this._fileName = fileName;
            this.Operations = new List<Operation>();
            this.OperationsById = new Dictionary<int, Operation>();
            this._maxId = 0;
            this.LoadFromFile();
        }
        private static OperationRepository s_instance = null;
        public static OperationRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new OperationRepository(@"..\..\..\Data\JSON\operations.json");
                }
                return s_instance;
            }
        }

        private Operation Parse(JToken? operation)
        {
            Dictionary<int, Room> roomsById = RoomRepository.GetInstance().RoomById;
            Dictionary<String, MedicalRecord> medicalRecordsByUsername = MedicalRecordRepository.GetInstance().MedicalRecordByUsername;

            int id = (int)operation["id"];
            ExaminationStatus status;
            Enum.TryParse(operation["status"].ToString(), out status);
            DateTime appointment = (DateTime)operation["appointment"];
            int duration = (int)operation["duration"];
            int roomId = (int)operation["room"];
            Room room = roomsById[roomId];
            String patientUsername = (String)operation["medicalRecord"];
            MedicalRecord medicalRecord = medicalRecordsByUsername[patientUsername];
            String report = (String)operation["report"];

            return new Operation(id, status, appointment, duration, room, null, medicalRecord, report);
        }
        public void LoadFromFile()
        { 
            var allOperations = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var operation in allOperations)
            {
                Operation loadedOperation = Parse(operation);
                int id = loadedOperation.Id;
                if (id > _maxId) { _maxId = id; }

                this.Operations.Add(loadedOperation);
                this.OperationsById.Add(id, loadedOperation);
            }
        }

        private List<dynamic> GetForSerialization()
        {
            List<dynamic> reducedOperations = new List<dynamic>();
            foreach (Operation operation in this.Operations)
            {
                reducedOperations.Add(new
                {
                    id = operation.Id,
                    status = operation.Status,
                    room = operation.Room.Number,
                    duration = operation.Duration,
                    appointment = operation.Appointment,
                    medicalRecord = operation.MedicalRecord.Patient.Username,
                    report = operation.Report
                });
            }
            return reducedOperations;
        }


        public void Save()
        {
            List<dynamic> reducedOperations = GetForSerialization();
            var allOperations = JsonSerializer.Serialize(reducedOperations, _options);
            File.WriteAllText(this._fileName, allOperations);
        }

        public List<Operation> GetAll()
        {
            return this.Operations;
        }

        public Operation GetById(int id)
        {
            if (OperationsById.ContainsKey(id))
            {
                return OperationsById[id];
            }
            return null;
        }

        public void Add(DateTime startTime, int duration, Room room, Doctor doctor, MedicalRecord medicalRecord, String report)
        {
            int id = ++this._maxId;
            Operation operation = new Operation(id, ExaminationStatus.Scheduled, startTime, duration, room, doctor, medicalRecord, report);
            doctor.Operations.Add(operation);
            this.Operations.Add(operation);
            this.OperationsById.Add(id, operation);
            Save();
        }

        public void Update(int id, DateTime appointment, MedicalRecord medicalRecord, int duration)
        {
            Operation operation = OperationsById[id];
            CheckIfDoctorIsAvailable(operation.Doctor, appointment, duration);
            CheckIfPatientIsAvailable(medicalRecord.Patient, appointment, duration);
            operation.Appointment = appointment;
            operation.MedicalRecord = medicalRecord;
            operation.Duration = duration;
            this.OperationsById[id] = operation;
            Save();
        }

        public void Delete(int id)
        {
            Operation operation = OperationsById[id];
            this.Operations.Remove(operation);
            this.OperationsById.Remove(id);
            Save();
        }

        private void CheckIfDoctorHasExaminations(Doctor doctor, DateTime dateTime, int duration)
        {
            foreach (var examination in doctor.Examinations)
            {
                if ((dateTime < examination.Appointment.AddMinutes(15)) && (dateTime.AddMinutes(duration) > examination.Appointment))
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }

        private void CheckIfDoctorHasOperations(Doctor doctor, DateTime dateTime, int duration)
        {
            foreach (var operation in doctor.Operations)
            {
                if ((dateTime < operation.Appointment.AddMinutes(operation.Duration)) && (dateTime.AddMinutes(duration) > operation.Appointment))
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }
        public void CheckIfDoctorIsAvailable(Doctor doctor, DateTime dateTime, int duration)
        {
            CheckIfDoctorHasExaminations(doctor, dateTime, duration);
            CheckIfDoctorHasOperations(doctor, dateTime, duration);
        }

        private void CheckIfPatientHasExaminations(Patient patient, DateTime dateTime, int duration)
        {
            var allExaminations = ExaminationRepository.GetInstance().Examinations;
            foreach (var examination in allExaminations)
            {
                if ((examination.MedicalRecord.Patient.Username == patient.Username))
                {
                    if ((dateTime < examination.Appointment.AddMinutes(15)) && (dateTime.AddMinutes(duration) > examination.Appointment))
                    {
                        throw new Exception("That patient is not available");
                    }
                }
            }
        }

        private void CheckIfPatientHasOperations(Patient patient, DateTime dateTime, int duration)
        {
            var allOperations = GetInstance().Operations;
            foreach (var operation in allOperations)
            {
                if (operation.MedicalRecord.Patient.Username == patient.Username)
                {
                    if ((dateTime < operation.Appointment.AddMinutes(operation.Duration)) && (dateTime.AddMinutes(duration) > operation.Appointment))
                    {
                        throw new Exception("That patient is not available");
                    }
                }
            }
        }

        private void CheckIfPatientIsAvailable(Patient patient, DateTime dateTime, int duration)
        {
            CheckIfPatientHasExaminations(patient, dateTime, duration);
            CheckIfPatientHasOperations(patient, dateTime, duration);
        }

        private Room FindAvailableRoom(DateTime dateTime, int duration)
        {
            bool isAvailable;
            List<Room> availableRooms = new List<Room>();
            var rooms = RoomRepository.GetInstance().GetNotRenovating();
            foreach (var room in rooms)
            {
                if (room.Type != RoomType.OperatingRoom) continue;
                isAvailable = true;
                foreach (var operation in this.Operations)
                {
                    if (operation.Room.Id == room.Id)
                    {
                        if ((dateTime < operation.Appointment.AddMinutes(operation.Duration)) && (dateTime.AddMinutes(duration) > operation.Appointment))
                        {
                            isAvailable = false;
                            break;
                        }
                    }
                }
                if (isAvailable)
                    availableRooms.Add(room);
            }

            if (availableRooms.Count == 0) throw new Exception("There are no available rooms!");
            Random random = new Random();
            int index = random.Next(0, availableRooms.Count);
            return availableRooms[index];
        }

        public void ReserveOperation(string patientUsername, string doctorUsername, DateTime dateTime, int duration)
        {
            Doctor doctor = DoctorRepository.GetInstance().GetById(doctorUsername);
            Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
            CheckIfDoctorIsAvailable(doctor, dateTime, duration);
            CheckIfPatientIsAvailable(patient, dateTime, duration);
            var room = FindAvailableRoom(dateTime, duration);
            var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
            Add(dateTime, duration, room, doctor, medicalRecord, "");
        }
    }
}
