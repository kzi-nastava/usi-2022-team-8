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
    public class OperationRepository : IOperationRepository
    {
        private String _fileName = @"..\..\..\Data\JSON\operations.json";

        private IRoomRepository _roomRepository;
        private IMedicalRecordRepository _medicalRecordRepository;
        private IOperationDoctorRepository _operationDoctorRepository;
        public int _maxId { get; set; }
        public List<Operation> Operations { get; set; }
        public Dictionary<int, Operation> OperationsById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public OperationRepository(IRoomRepository roomRepository, IMedicalRecordRepository medicalRecordRepository, IOperationDoctorRepository operationDoctorRepository)
        {
            _roomRepository = roomRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _operationDoctorRepository = operationDoctorRepository;
            this.Operations = new List<Operation>();
            this.OperationsById = new Dictionary<int, Operation>();
            this._maxId = 0;
            this.LoadFromFile();
            this.ChangeStatus();
        }

        private Operation Parse(JToken? operation)
        {
            Dictionary<int, Room> roomsById = _roomRepository.GetAllById();
            Dictionary<String, MedicalRecord> medicalRecordsByUsername = _medicalRecordRepository.GetAllByUsername();

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

        private void ChangeStatus()
        {
            foreach (var operation in this.Operations)
            {
                if ((operation.Status == ExaminationStatus.Scheduled) && (operation.Appointment <= DateTime.Now))
                    operation.Status = ExaminationStatus.Completed;
            }
            Save();
        }
        public int GetMaxId()
        {
            return _maxId;
        }
        public List<Operation> GetAll()
        {
            return this.Operations;
        }

        public Dictionary<int, Operation> GetAllById()
        {
            return this.OperationsById;
        }
        public Operation GetById(int id)
        {
            if (OperationsById.ContainsKey(id))
            {
                return OperationsById[id];
            }
            return null;
        }

        private void AddToCollections(Operation operation)
        {
            operation.Doctor.Operations.Add(operation);
            Operations.Add(operation);
            OperationsById.Add(operation.Id, operation);
        }

        public void Add(Operation operation)
        {
            int id = ++this._maxId;
            operation.Id = id;
            AddToCollections(operation);
            Save();
            _operationDoctorRepository.Save();
        }

        public void Update(int id, Operation byOperation)
        {
            Operation operation = GetById(id);
            operation.Appointment = byOperation.Appointment;
            operation.MedicalRecord = byOperation.MedicalRecord;
            operation.Duration = byOperation.Duration;
            this.OperationsById[id] = operation;
            Save();
        }

        //ispraviti 
        public void Delete(int id)
        {
            Operation operation = OperationsById[id];
            this.Operations.Remove(operation);
            this.OperationsById.Remove(id);
            Save();
            _operationDoctorRepository.Save();
        }

        public void SwapOperationValue(Operation operation)
        {
            var oldOperation = this.OperationsById[operation.Id];
            this.Operations.Remove(oldOperation);
            oldOperation.Doctor.Operations.Remove(oldOperation);
            AddToCollections(operation);
            Save();
        }

        public List<Operation> GetByPatient(String patientUsername)
        {
            List<Operation> patientOperations = new List<Operation>();
            foreach (var operation in this.GetAll())
                if (operation.MedicalRecord.Patient.Username == patientUsername)
                    patientOperations.Add(operation);
            return patientOperations;
        }

        public List<Operation> GetByDoctor(String doctorUsername)
        {
            List<Operation> doctorOperations = new List<Operation>();
            foreach (var operation in this.GetAll())
                if (operation.Doctor.Username == doctorUsername)
                    doctorOperations.Add(operation);
            return doctorOperations;
        }
    }
}
