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
    public class PollCommentRepository : IPollCommentRepository
    {
        private String _fileName;

        private int _maxId;
        public List<PollComment> PollComments { get; set; }
        public Dictionary<int, PollComment> PollCommentById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        private PollCommentRepository(String fileName)
        {
            this._fileName = fileName;
            this.PollComments = new List<PollComment>();
            this.PollCommentById = new Dictionary<int, PollComment>();
            this._maxId = 0;
            this.LoadFromFile();
        }
        private static PollCommentRepository s_instance = null;
        public static PollCommentRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new PollCommentRepository(@"..\..\..\Data\JSON\pollComments.json");
                }
                return s_instance;
            }
        }

        private PollComment Parse(JToken? pollComment)
        {
            Dictionary<string, Doctor> doctorByUsername = DoctorRepository.GetInstance().DoctorsByUsername;

            int id = (int)pollComment["id"];
            string comment = (string)pollComment["comment"];
            string doctorUsername = (string)pollComment["forDoctor"];
            Doctor? forDoctor;
            if (doctorUsername == "")
                forDoctor = null;
            else forDoctor = doctorByUsername[doctorUsername];
            
            return new PollComment(id, comment, forDoctor);
        }

        public void LoadFromFile()
        {
            var pollComments = JArray.Parse(File.ReadAllText(_fileName));

            foreach (var pollComment in pollComments)
            {
                PollComment loadedPollComment = Parse(pollComment);
                int id = loadedPollComment.Id;

                if (id > _maxId)
                {
                    _maxId = id;
                }

                this.PollComments.Add(loadedPollComment);
                this.PollCommentById[id] = loadedPollComment;
            }
        }

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedPollComments = new List<dynamic>();
            foreach (var pollComment in this.PollComments)
            {
                reducedPollComments.Add(new
                {
                    id = pollComment.Id,
                    comment = pollComment.Comment,
                    forDoctor = (pollComment.ForDoctor == null) ? "" : pollComment.ForDoctor.Username
                });
            }
            return reducedPollComments;
        }
        public void Save()
        {
            var allPollComments = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allPollComments);
        }

        public List<PollComment> GetAll()
        {
            return this.PollComments;
        }

        public PollComment GetById(int id)
        {
            if (PollCommentById.ContainsKey(id))
                return PollCommentById[id];
            return null;
        }

        public void Add(PollComment pollComment)
        {
            this._maxId++;
            int id = this._maxId;
            pollComment.Id = id;

            this.PollComments.Add(pollComment);
            this.PollCommentById.Add(pollComment.Id, pollComment);
            Save();
        }

        public void Update(int id, PollComment byPollComment)
        {
            PollComment pollComment = GetById(id);
            pollComment.Comment = byPollComment.Comment;
            pollComment.ForDoctor = byPollComment.ForDoctor;
            Save();
        }


        public void Delete(int id)
        {
            PollComment pollComment = GetById(id);
            this.PollComments.Remove(pollComment);
            this.PollCommentById.Remove(id);
            Save();
        }
    }
}
