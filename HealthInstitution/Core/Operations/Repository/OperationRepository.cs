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
        public String fileName { get; set; }
        public int maxId { get; set; }
        public List<Operation> operations { get; set; }
        public Dictionary<int, Operation> operationsById { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private OperationRepository(String fileName)
        {
            this.fileName = fileName;
            this.operations = new List<Operation>();
            this.operationsById = new Dictionary<int, Operation>();
            this.maxId = 0;
            this.LoadFromFile();
        }
        private static OperationRepository instance = null;
        public static OperationRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new OperationRepository(@"..\..\..\Data\JSON\operations.json");
                }
                return instance;
            }
        }
        public void LoadFromFile()
        {
            var roomsById = RoomRepository.GetInstance().roomById;
            var doctorsByUsername = DoctorRepository.GetInstance().doctorsByUsername;
            var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().medicalRecordByUsername;
            var allOperations = JArray.Parse(File.ReadAllText(this.fileName));
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

                if (id > maxId) { maxId = id; }

                this.operations.Add(loadedOperation);
                this.operationsById.Add(id, loadedOperation);
            }
        }


        public void SaveToFile()
        {
            List<dynamic> reducedOperations = new List<dynamic>();
            foreach (Operation operation in this.operations)
            {
                reducedOperations.Add(new
                {
                    id = operation.id,
                    status = operation.status,
                    room = operation.room.number,
                    duration = operation.duration,
                    appointment = operation.appointment,
                    medicalRecord = operation.medicalRecord.patient.username,
                    report = operation.report
                });
            }
            var allOperations = JsonSerializer.Serialize(reducedOperations, options);
            File.WriteAllText(this.fileName, allOperations);
        }

        public List<Operation> Get()
        {
            return this.operations;
        }

        public Operation GetById(int id)
        {
            if (operationsById.ContainsKey(id))
            {
                return operationsById[id];
            }
            return null;
        }

        public void AddOperation(DateTime startTime, int duration, Room room, Doctor doctor, MedicalRecord medicalRecord)
        {
            int id = ++this.maxId;
            Operation operation = new Operation(id, ExaminationStatus.Scheduled, startTime, duration, room, doctor, medicalRecord);
            doctor.operations.Add(operation);
            this.operations.Add(operation);
            this.operationsById.Add(id, operation);
            SaveToFile();
        }

        public void UpdateOperation(int id, DateTime appointment, MedicalRecord medicalRecord, int duration)
        {
            Operation operation = operationsById[id];
            operation.appointment = appointment;
            operation.medicalRecord = medicalRecord;
            operation.duration = duration;
            this.operationsById[id] = operation;
            SaveToFile();
        }

        public void DeleteOperation(int id)
        {
            Operation operation = operations[id];
            this.operations.Remove(operation);
            this.operationsById.Remove(id);
            SaveToFile();
        }
        private bool IsExaminationInOperationTime(Examination examination, DateTime appointment, int duration)
        {
            if (examination.appointment >= appointment && examination.appointment.AddMinutes(15) <= appointment.AddMinutes(duration))
                return true;
            return false;
        }

        private bool IsOperationInOperationTime(Operation oldOperation, DateTime appointment, int duration)
        {
            if (appointment >= oldOperation.appointment && appointment.AddMinutes(duration) <= oldOperation.appointment.AddMinutes(oldOperation.duration))
                return true;
            if (oldOperation.appointment >= appointment && oldOperation.appointment.AddMinutes(oldOperation.duration) <= appointment.AddMinutes(duration))
                return true;
            return false;
        }

        private void CheckIfDoctorIsAvailable(Doctor doctor, DateTime dateTime, int duration)
        {
            foreach (var examination in doctor.examinations)
            {
                if (examination.appointment <= dateTime && examination.appointment.AddMinutes(15) >= dateTime)
                {
                    throw new Exception("That doctor is not available");
                }
                if (examination.appointment <= dateTime.AddMinutes(duration) && examination.appointment.AddMinutes(15) >= dateTime.AddMinutes(duration))
                {
                    throw new Exception("That doctor is not available");
                }
                if (IsExaminationInOperationTime(examination, dateTime, duration))
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

                if (operation.appointment <= dateTime.AddMinutes(duration) && operation.appointment.AddMinutes(operation.duration) >= dateTime.AddMinutes(duration))
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
            var allExaminations = ExaminationRepository.GetInstance().examinations;
            var allOperations = operations;
            foreach (var examination in allExaminations)
            {
                if (examination.medicalRecord.patient.username == patient.username)
                {
                    if (examination.appointment <= dateTime && examination.appointment.AddMinutes(15) >= dateTime)
                    {
                        throw new Exception("That doctor is not available");
                    }
                    if (examination.appointment <= dateTime.AddMinutes(duration) && examination.appointment.AddMinutes(15) >= dateTime.AddMinutes(duration))
                    {
                        throw new Exception("That doctor is not available");
                    }
                    if (IsExaminationInOperationTime(examination, dateTime, duration))
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
                    if (operation.appointment <= dateTime.AddMinutes(duration) && operation.appointment.AddMinutes(operation.duration) >= dateTime.AddMinutes(duration))
                    {
                        throw new Exception("That doctor is not available");
                    }
                    if (IsOperationInOperationTime(operation, dateTime, duration))
                    {
                        throw new Exception("That doctor is not available");
                    }
                }
            }
        }

        private Room FindAvailableRoom(DateTime dateTime, int duration)
        {
            bool isAvailable;
            List<Room> availableRooms = new List<Room>();
            foreach (var room in RoomRepository.GetInstance().GetRooms())
            {
                if (room.type != RoomType.OperatingRoom) continue;
                isAvailable = true;
                foreach (var operation in OperationRepository.GetInstance().operations)
                {
                    if (operation.room.id == room.id)
                    {
                        if (operation.appointment <= dateTime.AddMinutes(duration) && operation.appointment.AddMinutes(operation.duration) >= dateTime.AddMinutes(duration))
                        {
                            isAvailable = false;
                            break;
                        }
                        if (operation.appointment <= dateTime && operation.appointment.AddMinutes(operation.duration) >= dateTime)
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

        public void ReserveOperation(string patientUsername, string doctorUsername, DateTime dateTime, int duration)
        {
            Doctor doctor = DoctorRepository.GetInstance().GetDoctorByUsername(doctorUsername);
            Patient patient = PatientRepository.GetInstance().GetPatientByUsername(patientUsername);
            CheckIfDoctorIsAvailable(doctor, dateTime, duration);
            CheckIfPatientIsAvailable(patient, dateTime, duration);   
            var room = FindAvailableRoom(dateTime, duration);
            var medicalRecord = MedicalRecordRepository.GetInstance().GetMedicalRecordByUsername(patient);
            AddOperation(dateTime, duration, room, doctor, medicalRecord);
        }
    }
}
