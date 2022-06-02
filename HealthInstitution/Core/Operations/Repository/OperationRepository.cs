using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Rooms;
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

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedOperations = new List<dynamic>();
            foreach (Operation operation in this.Operations)
            {
                reducedOperations.Add(new
                {
                    id = operation.Id,
                    status = operation.Status,
                    room = operation.Room.Id,
                    duration = operation.Duration,
                    appointment = operation.Appointment,
                    medicalRecord = operation.MedicalRecord.Patient.Username,
                });
            }
            return reducedOperations;
        }


        public void Save()
        {
            List<dynamic> reducedOperations = PrepareForSerialization();
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
        

        private void ChangeStatus()
        {
            foreach (var operation in this.Operations)
            {
                if ((operation.Status == ExaminationStatus.Scheduled) && (operation.Appointment <= DateTime.Now))
                    operation.Status = ExaminationStatus.Completed;
            }
            Save();
        }

        //greska ne sme pozivati ovaj save.
        public void Add(Operation operation)
        {
            int id = ++this._maxId;
            operation.Id = id;
            AddToCollections(operation);
            Save();
            OperationDoctorRepository.GetInstance().Save();
        }
        private void AddToCollections(Operation operation)
        {
            operation.Doctor.Operations.Add(operation);
            Operations.Add(operation);
            OperationsById.Add(operation.Id, operation);
        }

        public void Update(int id, OperationDTO operationDTO)
        {
            Operation operation = OperationsById[id];
            
            CheckIfDoctorIsAvailable(operationDTO, id);
            CheckIfPatientIsAvailable(operationDTO, id);
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
    }
}
