using HealthInstitution.Core.Polls.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Repository
{
    public interface IPollCommentRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<PollComment> GetAll();
        public Dictionary<int, PollComment> GetAllById();
        public PollComment GetById(int id);
        public void Add(PollComment pollComment);
        public void Update(int id, PollComment byPollComment);
        public void Delete(int id);

    }
}
