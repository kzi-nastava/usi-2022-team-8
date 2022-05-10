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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public void LoadFromFile()
        {
            var roomsById = RoomRepository.GetInstance().RoomById;
            var doctorsByUsername = DoctorRepository.GetInstance().DoctorsByUsername;
            var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().MedicalRecordByUsername;
            var allOperations = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var operation in allOperations)
            {
                int id = (int)operation["id"];
                ExaminationStatus status;
                Enum.TryParse(operation["status"].ToString(), out status);
                DateTime appointment = (DateTime)operation["appointment"];
                int duration = (int)operation["duration"];
                int roomId = (int)operation["room"];
                Room room = roomsById[roomId];
                String doctorUsername = (String)operation["doctor"];
                String patientUsername = (String)operation["medicalRecord"];
                MedicalRecord medicalRecord = medicalRecordsByUsername[patientUsername];
                String report = (String)operation["report"];

                Operation loadedOperation = new Operation(id, status, appointment, duration, room, null, medicalRecord);

                if (id > _maxId) { _maxId = id; }

                this.Operations.Add(loadedOperation);
                this.OperationsById.Add(id, loadedOperation);
            }
        }


        public void Save()
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

        public void Add(DateTime startTime, int duration, Room room, Doctor doctor, MedicalRecord medicalRecord)
        {
            int id = ++this._maxId;
            Operation operation = new Operation(id, ExaminationStatus.Scheduled, startTime, duration, room, doctor, medicalRecord);
            doctor.Operations.Add(operation);
            this.Operations.Add(operation);
            this.OperationsById.Add(id, operation);
            Save();
        }

        public void Update(int id, DateTime appointment, MedicalRecord medicalRecord, int duration)
        {
            Operation operation = OperationsById[id];
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
        private bool IsExaminationInOperationTime(Examination examination, DateTime appointment, int duration)
        {
            if (examination.Appointment >= appointment && examination.Appointment.AddMinutes(15) <= appointment.AddMinutes(duration))
                return true;
            return false;
        }

        private bool IsOperationInOperationTime(Operation oldOperation, DateTime appointment, int duration)
        {
            if (appointment >= oldOperation.Appointment && appointment.AddMinutes(duration) <= oldOperation.Appointment.AddMinutes(oldOperation.Duration))
                return true;
            if (oldOperation.Appointment >= appointment && oldOperation.Appointment.AddMinutes(oldOperation.Duration) <= appointment.AddMinutes(duration))
                return true;
            return false;
        }

        private void CheckIfDoctorIsAvailable(Doctor doctor, DateTime dateTime, int duration)
        {
            foreach (var examination in doctor.Examinations)
            {
                if (examination.Appointment <= dateTime && examination.Appointment.AddMinutes(15) > dateTime)
                {
                    throw new Exception("That doctor is not available");
                }
                if (examination.Appointment <= dateTime.AddMinutes(duration) && examination.Appointment.AddMinutes(15) >= dateTime.AddMinutes(duration))
                {
                    throw new Exception("That doctor is not available");
                }
                if (IsExaminationInOperationTime(examination, dateTime, duration))
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

                if (operation.Appointment <= dateTime.AddMinutes(duration) && operation.Appointment.AddMinutes(operation.Duration) >= dateTime.AddMinutes(duration))
                {
                    throw new Exception("That doctor is not available");
                }
                if (IsOperationInOperationTime(operation, dateTime, duration))
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }

        private void CheckIfPatientIsAvailable(Patient patient, DateTime dateTime, int duration)
        {
            var allExaminations = ExaminationRepository.GetInstance().Examinations;
            var allOperations = Operations;
            foreach (var examination in allExaminations)
            {
                if (examination.MedicalRecord.Patient.Username == patient.Username)
                {
                    if (examination.Appointment <= dateTime && examination.Appointment.AddMinutes(15) >= dateTime)
                    {
                        throw new Exception("That patient is not available");
                    }
                    if (examination.Appointment <= dateTime.AddMinutes(duration) && examination.Appointment.AddMinutes(15) >= dateTime.AddMinutes(duration))
                    {
                        throw new Exception("That patient is not available");
                    }
                    if (IsExaminationInOperationTime(examination, dateTime, duration))
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
                    if (operation.Appointment <= dateTime.AddMinutes(duration) && operation.Appointment.AddMinutes(operation.Duration) >= dateTime.AddMinutes(duration))
                    {
                        throw new Exception("That patient is not available");
                    }
                    if (IsOperationInOperationTime(operation, dateTime, duration))
                    {
                        throw new Exception("That patient is not available");
                    }
                }
            }
        }

        private Room FindAvailableRoom(DateTime dateTime, int duration)
        {
            bool isAvailable;
            List<Room> availableRooms = new List<Room>();
            foreach (var room in RoomRepository.GetInstance().GetAll())
            {
                if (room.Type != RoomType.OperatingRoom) continue;
                isAvailable = true;
                foreach (var operation in OperationRepository.GetInstance().Operations)
                {
                    if (operation.Room.Id == room.Id)
                    {
                        if (operation.Appointment <= dateTime.AddMinutes(duration) && operation.Appointment.AddMinutes(operation.Duration) >= dateTime.AddMinutes(duration))
                        {
                            isAvailable = false;
                            break;
                        }
                        if (operation.Appointment <= dateTime && operation.Appointment.AddMinutes(operation.Duration) >= dateTime)
                        {
                            isAvailable = false;
                            break;
                        }
                        if (IsOperationInOperationTime(operation, dateTime, duration))
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
            return availableRooms[0];
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

        public void ReserveOperation(string patientUsername, string doctorUsername, DateTime dateTime, int duration)
        {
            Doctor doctor = DoctorRepository.GetInstance().GetById(doctorUsername);
            Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
            CheckIfDoctorIsAvailable(doctor, dateTime, duration);
            CheckIfPatientIsAvailable(patient, dateTime, duration);   
            var room = FindAvailableRoom(dateTime, duration);
            var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
            Add(dateTime, duration, room, doctor, medicalRecord);
        }

        public List<Tuple<int, int, DateTime>> ReserveUrgentOperation(string patientUsername, SpecialtyType specialtyType, int duration)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            Patient patient = PatientRepository.GetInstance().GetByUsername(patientUsername);
            List<DateTime> nextTwoHoursAppointments = ExaminationRepository.FindNextTwoHoursAppointments();
            foreach (DateTime appointment in nextTwoHoursAppointments)
            {
                foreach (Doctor doctor in DoctorRepository.GetInstance().Doctors)
                {
                    if (doctor.Specialty == specialtyType)
                    {
                        try
                        {
                            CheckIfDoctorIsAvailable(doctor, appointment, duration);
                            CheckIfPatientIsAvailable(patient, appointment, duration);
                            var room = FindAvailableRoom(appointment, duration);
                            var medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
                            Add(appointment, duration, room, doctor, medicalRecord);
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
            List<Tuple<int, int, DateTime>> temporaryPriority = ExaminationRepository.FindClosest(nextTwoHoursAppointments);
            foreach (Tuple<int, int, DateTime> tuple in temporaryPriority)
            {
                priorityExaminationsAndOperations.Add(tuple);
            }
            return priorityExaminationsAndOperations;
        }
    }
}
