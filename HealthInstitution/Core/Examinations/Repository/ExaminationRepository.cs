using HealthInstitution.Core.Appointments.Model;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
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
        public List<Examination> examinations { get; set; }
        public Dictionary<String, Examination> examinationsByUsername { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private ExaminationRepository(String fileName)
        {
            this.fileName = fileName;
            this.examinations = new List<Examination>();
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
            var examinations = JsonSerializer.Deserialize<List<Examination>>(File.ReadAllText(@"..\..\..\Data\JSON\examinations.json"), options);
            foreach (Examination examination in examinations)
            {
                this.examinations.Add(examination);
            }
        }

        public void SaveExaminations()
        {
            var allPatients = JsonSerializer.Serialize(this.examinations, options);
            File.WriteAllText(this.fileName, allPatients);
        }

        public List<Examination> GetExaminations()
        {
            return this.examinations;
        }

        public Examination GetExaminationById(int id)
        {
            foreach (Examination examination in examinations)
            {
                if (examination.id == id)
                    return examination;
            }
            return null;
        }
        public Examination GetExaminationByDoctorUsername(String username)
        {
            foreach (Examination examination in examinations)
            {
                if (examination.doctor.username == username)
                    return examination;
            }
            return null;
        }

        public void AddExamination(int id, Appointment appointment, Room room, Doctor doctor, MedicalRecord medicalRecord)
        {
            Examination examination = new Examination(id, appointment, room, doctor, medicalRecord);
            this.examinations.Add(examination);
            SaveExaminations();
        }

        public void UpdateExamination(int id, Appointment appointment, MedicalRecord medicalRecord)
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
            SaveExaminations();
        }
    }
}
