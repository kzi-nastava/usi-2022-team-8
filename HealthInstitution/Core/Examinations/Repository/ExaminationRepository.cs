using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations.Repository
{
    internal class ExaminationRepository
    {
        public String fileName { get; set; }
        public int maxId { get; set; }
        public List<Examination> examinations { get; set; }
        public Dictionary<int, Examination> examinationsById { get; set; }
        
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private ExaminationRepository(String fileName)
        {
            this.fileName = fileName;
            this.examinations = new List<Examination>();
            this.examinationsById = new Dictionary<int, Examination>();
            this.maxId = 0;
            this.LoadExaminations();
        }
        private static ExaminationRepository instance = null;
        public static ExaminationRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new ExaminationRepository(@"..\..\..\Data\JSON\examinations.json");
                }
                return instance;
            }
        }
        public void LoadExaminations()
        {
            var roomsById = RoomRepository.GetInstance().roomById;
            var doctorsByUsername = DoctorRepository.GetInstance().doctorsByUsername;
            var medicalRecordsByUsername = MedicalRecordRepository.GetInstance().medicalRecordsByUsername();
            var examinations = JArray.Parse(File.ReadAllText(this.fileName));
            foreach (var examination in examinations)
            {
                int id = (int)examination["id"];
                ExaminationStatus status;
                Enum.TryParse(examination["status"].ToString(), out status);
                DateTime appointment = (DateTime)examination["appointment"];
                int roomId = (int)examination["room"];
                Room room = roomsById[roomId];
                String doctorUsername = (String)examination["doctor"];
                Doctor doctor = doctorsByUsername[doctorUsername];
                String patientUsername = (String)examination["medicalRecord"];
                MedicalRecord medicalRecord = medicalRecordsByUsername[patientUsername];
                String anamnesis = (String)examination["anamnesis"];

                Examination loadedExamination = new Examination(id, status, appointment, room, doctor, medicalRecord);

                if (id > maxId) { maxId = id; }

                this.examinations.Add(loadedExamination);
                this.examinationsById.Add(id, loadedExamination);
            }


        }

        public List<dynamic> ShortenExamination()
        {
            List<dynamic> reducedExaminations = new List<dynamic>();
            foreach (Examination examination in this.examinations)
            {
                reducedExaminations.Add(new
                {
                    id = examination.id,
                    status = examination.status,
                    appointment = examination.appointment,
                    room = examination.room.id,
                    doctor = examination.doctor.username,
                    medicalRecord = examination.medicalRecord.patient.username,
                    anamnesis = examination.anamnesis
                });
            }
            return reducedExaminations
        ;}

        public void SaveExaminations()
        {
            var allExaminations = JsonSerializer.Serialize(ShortenExamination(), options);
            File.WriteAllText(this.fileName, allExaminations);
        }

        public List<Examination> GetExaminations()
        {
            return this.examinations;
        }

        public Examination GetExaminationById(int id)
        {
            if (examinationsById.ContainsKey(id))
            {
                return examinationsById[id];
            }
            return null;
        }

        public void AddExamination(DateTime appointment, Room room, Doctor doctor, MedicalRecord medicalRecord)
        {
            int id = this.maxId++;
            Examination examination = new Examination(id, ExaminationStatus.Scheduled, appointment, room, doctor, medicalRecord);
            this.examinations.Add(examination);
            this.examinationsById.Add(id, examination);
            SaveExaminations();
        }

        public void UpdateExamination(int id, DateTime appointment, MedicalRecord medicalRecord)
        {
            Examination examination = GetExaminationById(id);
            examination.appointment = appointment;
            examination.medicalRecord = medicalRecord;
            SaveExaminations();
        }

        public void DeleteExamination(int id)
        {
            Examination examination = GetExaminationById(id);
            this.examinations.Remove(examination);
            this.examinationsById.Remove(id);
            SaveExaminations();
        }
    }
}
