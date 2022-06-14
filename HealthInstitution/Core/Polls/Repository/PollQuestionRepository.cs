using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Repository
{
    public class PollQuestionRepository : IPollQuestionRepository
    {
        private String _fileName = @"..\..\..\Data\JSON\pollQuestions.json";
        private IDoctorRepository _doctorRepository;

        public List<string> HospitalQuestions { get; set; }
        public List<string> DoctorQuestions { get; set; }

        private int _maxId;
        public List<PollQuestion> PollQuestions { get; set; }
        public Dictionary<int, PollQuestion> PollQuestionById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public PollQuestionRepository(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            this.PollQuestions = new List<PollQuestion>();
            this.PollQuestionById = new Dictionary<int, PollQuestion>();
            this._maxId = 0;
            this.HospitalQuestions = new List<string> {   "Rate quality of service",
                                                          "Rate hygiene",
                                                          "Rate satisfaction with service",
                                                          "How likely would you recommend this hospital",
                                                          "Rate overall experience" };
            this.DoctorQuestions = new List<string> {     "Rate quality of service",
                                                          "Rate competence of the doctor",
                                                          "Rate satisfaction with service",
                                                          "How likely would you recommend this doctor",
                                                          "Rate overall experience" };
            this.LoadFromFile();
        }

       
        private List<int> JToken2Ints(JToken tokens)
        {
            List<int> items = new List<int>();
            foreach (string token in tokens)
                items.Add(Int32.Parse(token));
            return items;
        }

        private PollQuestion Parse(JToken? pollQuestion)
        {
            Dictionary<string, Doctor> doctorByUsername = _doctorRepository.GetAllByUsername();
            
            int id = (int)pollQuestion["id"];
            string question = (string)pollQuestion["question"];
            string doctorUsername = (string)pollQuestion["forDoctor"];
            Doctor? forDoctor;
            if (doctorUsername == "")
               forDoctor = null;
            else forDoctor = doctorByUsername[doctorUsername];
            List<int> grades = JToken2Ints(pollQuestion["grades"]);

            return new PollQuestion(id, question, forDoctor, grades);
        }

        public void LoadFromFile()
        {
            var pollQuestions = JArray.Parse(File.ReadAllText(_fileName));

            foreach (var pollQuestion in pollQuestions)
            {
                PollQuestion loadedPollQuestion = Parse(pollQuestion);
                int id = loadedPollQuestion.Id;

                if (id > _maxId)
                {
                    _maxId = id;
                }

                this.PollQuestions.Add(loadedPollQuestion);
                this.PollQuestionById[id] = loadedPollQuestion;
            }
        }

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedPollQuestions = new List<dynamic>();
            foreach (var pollQuestion in this.PollQuestions)
            {
                reducedPollQuestions.Add(new
                {
                    id = pollQuestion.Id,
                    question = pollQuestion.Question,
                    forDoctor = (pollQuestion.ForDoctor == null) ? "" : pollQuestion.ForDoctor.Username,
                    grades = pollQuestion.Grades
                });
            }
            return reducedPollQuestions;
        }
        public void Save()
        {
            var allPollQuestions = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allPollQuestions);
        }

        public List<PollQuestion> GetAll()
        {
            return this.PollQuestions;
        }

        public Dictionary<int, PollQuestion> GetAllById()
        {
            return this.PollQuestionById;
        }
        public PollQuestion GetById(int id)
        {
            if (PollQuestionById.ContainsKey(id))
                return PollQuestionById[id];
            return null;
        }

        public void Add(PollQuestion pollQuestion)
        {
            this._maxId++;
            int id = this._maxId;
            pollQuestion.Id = id;

            this.PollQuestions.Add(pollQuestion);
            this.PollQuestionById.Add(pollQuestion.Id, pollQuestion);
            Save();
        }

        public void Update(int id, PollQuestion byPollQuestion)
        {
            PollQuestion pollQuestion = GetById(id);
            pollQuestion.Question = byPollQuestion.Question;
            pollQuestion.ForDoctor = byPollQuestion.ForDoctor;
            pollQuestion.Grades = byPollQuestion.Grades;
            Save();
        }


        public void Delete(int id)
        {
            PollQuestion pollQuestion = GetById(id);
            this.PollQuestions.Remove(pollQuestion);
            this.PollQuestionById.Remove(id);
            Save();
        }

        public List<string> GetHospitalQuestions()
        {
            return this.HospitalQuestions;
        }

        public List<string> GetDoctorQuestions()
        {
            return this.DoctorQuestions;
        }

        public List<PollQuestion> GetHospitalGradeByQuestion()
        {
            return PollQuestions.FindAll(question => question.ForDoctor == null);
        }

        public List<PollQuestion> GetDoctorGradeByQuestion(Doctor doctor)
        {
            return PollQuestions.FindAll(question => question.ForDoctor == doctor);
        }
    }
}
