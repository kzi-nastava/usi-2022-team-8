using HealthInstitution.Core.Examinations.Model;
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
            this.LoadOperations();
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
        public void LoadOperations()
        {
            var roomsById = RoomRepository.GetInstance().roomById;
            var doctorsByUsername = DoctorRepository.GetInstance().doctorsByUsername;
            //var medicalRecordsById = MedicalRecordRepository.GetInstance().medicalRecordById();
            var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().medicalRecordByUsername;
            var operations = JArray.Parse(File.ReadAllText(this.fileName));
            foreach (var operation in operations)
            {
                int id = (int)operation["id"];
                ExaminationStatus status;
                Enum.TryParse(operation["status"].ToString(), out status);
                DateTime appointment = (DateTime)operation["appointment"];
                int duration = (int)operation["duration"];
                int roomId = (int)operation["room"];
                Room room = roomsById[roomId];
                String doctorUsername = (String)operation["doctor"];
                //Doctor doctor = doctorsByUsername[doctorUsername];
                //int medicalRecordId = (int)operation["medicalRecord"];
                //MedicalRecord medicalRecord = medicalRecordsById[medicalRecordId];
                String patientUsername = (String)operation["medicalRecord"];
                MedicalRecord medicalRecord = medicalRecordsByUsername[patientUsername];
                String report = (String)operation["report"];

                Operation loadedOperation = new Operation(id, status, appointment, duration, room, null, medicalRecord);

                if (id > maxId) { maxId = id; }

                this.operations.Add(loadedOperation);
                this.operationsById.Add(id, loadedOperation);
            }


        }

        public List<dynamic> ShortenOperation()
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
                    medicalRecord = operation.medicalRecord.patient.username,
                    report = operation.report
                });
            }
            return reducedOperations;
        }

        public void SaveOperations()
        {
            var allOperations = JsonSerializer.Serialize(ShortenOperation(), options);
            File.WriteAllText(this.fileName, allOperations);
        }

        public List<Operation> GetOperations()
        {
            return this.operations;
        }

        public Operation GetOperationById(int id)
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
            this.operations.Add(operation);
            this.operationsById.Add(id, operation);
            SaveOperations();
        }

        public void UpdateOperation(int id, DateTime appointment, MedicalRecord medicalRecord, int duration)
        {
            Operation operation = GetOperationById(id);
            operation.appointment = appointment;
            operation.medicalRecord = medicalRecord;
            operation.duration = duration;
            this.operationsById[id] = operation;
            SaveOperations();
        }

        public void DeleteOperation(int id)
        {
            Operation operation = GetOperationById(id);
            this.operations.Remove(operation);
            this.operationsById.Remove(id);
            SaveOperations();
        }

        private void CheckIfDoctorIsAvailable(Doctor doctor, DateTime dateTime)
        {
            foreach (var operation in doctor.operations)
            {
                if (operation.appointment == dateTime)
                {
                    throw new Exception("Selected doctor is not available!");
                }
            }
        }

        private Room FindAvailableRoom(DateTime dateTime)
        {
            bool isAvailable;
            List<Room> availableRooms = new List<Room>();
            foreach (var room in RoomRepository.GetInstance().GetRooms())
            {
                if (room.type != RoomType.OperatingRoom) continue;
                isAvailable = true;
                foreach (var operation in OperationRepository.GetInstance().operations)
                {
                    if (operation.appointment == dateTime && operation.room.id == room.id)
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

        public void ReserveOperation(string patientUsername, string doctorUsername, DateTime dateTime, int duration)
        {
            Doctor doctor = DoctorRepository.GetInstance().GetDoctorByUsername(doctorUsername);
            CheckIfDoctorIsAvailable(doctor, dateTime);
            var room = FindAvailableRoom(dateTime);
            Patient patient = PatientRepository.GetInstance().GetPatientByUsername(patientUsername);
            var medicalRecord = MedicalRecordRepository.GetInstance().GetMedicalRecordByUsername(patient);
            OperationRepository.GetInstance().AddOperation(dateTime, duration, room, doctor, medicalRecord);
        }
    }
}
