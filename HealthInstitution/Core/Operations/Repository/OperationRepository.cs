using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Notifications.Repository;
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
            this.ChangeStatus();
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

            return new Operation(id, appointment, duration, room, null, medicalRecord);
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
        public List<Operation> GetPatientOperations(Patient patient)
        {
            List<Operation> patientOperations = new List<Operation>();
            foreach (var operation in this.Operations)
                if (operation.MedicalRecord.Patient.Username == patient.Username)
                    patientOperations.Add(operation);
            return patientOperations;
        }

        private void ChangeStatus()
        {
            foreach (var operation in this.Operations)
            {
                if ((operation.Status == ExaminationStatus.Scheduled) && (operation.Appointment <= DateTime.Now))
                    operation.Status = ExaminationStatus.Completed;
            }
        }

        public void Add(OperationDTO operationDTO)
        {
            int id = ++this._maxId;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;
            Room room = operationDTO.Room;
            Doctor doctor = operationDTO.Doctor;
            MedicalRecord medicalRecord = operationDTO.MedicalRecord;
            
            Operation operation = new Operation(id, appointment, duration, room, doctor, medicalRecord);
            doctor.Operations.Add(operation);
            this.Operations.Add(operation);
            this.OperationsById.Add(id, operation);

            Save();
            OperationDoctorRepository.GetInstance().Save();
        }

        public void Update(int id, OperationDTO operationDTO)
        {
            Operation operation = OperationsById[id];
            
            CheckIfDoctorIsAvailable(operationDTO);
            CheckIfPatientIsAvailable(operationDTO);
            operation.Appointment = operationDTO.Appointment;
            operation.MedicalRecord = operationDTO.MedicalRecord;
            operation.Duration = operationDTO.Duration;
            this.OperationsById[id] = operation;
            Save();
        }

        public void Delete(int id)
        {
            Operation operation = OperationsById[id];
            this.Operations.Remove(operation);
            this.OperationsById.Remove(id);
            Save();
            OperationDoctorRepository.GetInstance().Save();
        }

        private void CheckIfDoctorHasExaminations(OperationDTO operationDTO)
        {
            Doctor doctor = operationDTO.Doctor;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;

            foreach (var examination in doctor.Examinations)
            {
                if ((appointment < examination.Appointment.AddMinutes(15)) && (appointment.AddMinutes(duration) > examination.Appointment))
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }

        private void CheckIfDoctorHasOperations(OperationDTO operationDTO)
        {
            Doctor doctor = operationDTO.Doctor;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;

            foreach (var operation in doctor.Operations)
            {
                if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(duration) > operation.Appointment))
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }
        public void CheckIfDoctorIsAvailable(OperationDTO operationDTO)
        {
            CheckIfDoctorHasExaminations(operationDTO);
            CheckIfDoctorHasOperations(operationDTO);
        }

        private void CheckIfPatientHasExaminations(OperationDTO operationDTO)
        {
            Patient patient = operationDTO.MedicalRecord.Patient;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;
            var patientExaminations = ExaminationRepository.GetInstance().GetPatientExaminations(patient);

            foreach (var examination in patientExaminations)
            {
                if ((appointment < examination.Appointment.AddMinutes(15)) && (appointment.AddMinutes(duration) > examination.Appointment))
                {
                    throw new Exception("That patient is not available");
                }
            }
        }

        private void CheckIfPatientHasOperations(OperationDTO operationDTO)
        {
            Patient patient = operationDTO.MedicalRecord.Patient;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;
            var patientOperations = GetPatientOperations(patient);

            foreach (var operation in patientOperations)
            {
                if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(duration) > operation.Appointment))
                {
                    throw new Exception("That patient is not available");
                }
            }
        }

        private void CheckIfPatientIsAvailable(OperationDTO operationDTO)
        {
            CheckIfPatientHasExaminations(operationDTO);
            CheckIfPatientHasOperations(operationDTO);
        }

        private Room FindAvailableRoom(OperationDTO operationDTO)
        {
            bool isAvailable;
            List<Room> availableRooms = new List<Room>();
            var rooms = RoomRepository.GetInstance().GetNotRenovating();
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;

            foreach (var room in rooms)
            {
                if (room.Type != RoomType.OperatingRoom) continue;
                isAvailable = true;
                foreach (var operation in this.Operations)
                {
                    if (operation.Room.Id == room.Id)
                    {
                        if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(duration) > operation.Appointment))
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

        public void SwapOperationValue(Operation operation)
        {
            var oldOperation = this.OperationsById[operation.Id];
            this.Operations.Remove(oldOperation);
            this.Operations.Add(operation);
            operation.Doctor.Operations.Add(operation);
            oldOperation.Doctor.Operations.Remove(oldOperation);
            this.OperationsById[operation.Id] = operation;
            Save();
        }

        public void ReserveOperation(OperationDTO operationDTO)
        {
            operationDTO.Room = FindAvailableRoom(operationDTO);
            CheckIfDoctorIsAvailable(operationDTO);
            CheckIfPatientIsAvailable(operationDTO);
            Add(operationDTO);
        }

        public List<Tuple<int, int, DateTime>> ReserveUrgentOperation(string patientUsername, SpecialtyType specialtyType, int duration)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
            var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
            List<DateTime> nextTwoHoursAppointments = ExaminationRepository.FindNextTwoHoursAppointments();
            foreach (DateTime appointment in nextTwoHoursAppointments)
            {
                foreach (Doctor doctor in DoctorRepository.GetInstance().Doctors)
                {
                    if (doctor.Specialty == specialtyType)
                    {
                        try
                        {
                            OperationDTO operationDTO = new OperationDTO(appointment, duration, null, doctor, medicalRecord);
                            CheckIfDoctorIsAvailable(operationDTO);
                            CheckIfPatientIsAvailable(operationDTO);
                            operationDTO.Room = FindAvailableRoom(operationDTO);
                            Add(operationDTO);

                            NotificationRepository.GetInstance().Add(new DateTime(1, 1, 1), appointment, doctor, patient);
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
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(this._maxId+1, 2, new DateTime(1, 1, 1)));
            List<Tuple<int, int, DateTime>> temporaryPriority = ExaminationRepository.FindClosest(nextTwoHoursAppointments, specialtyType);
            foreach (Tuple<int, int, DateTime> tuple in temporaryPriority)
            {
                priorityExaminationsAndOperations.Add(tuple);
            }
            return priorityExaminationsAndOperations;
        }
    }
}
