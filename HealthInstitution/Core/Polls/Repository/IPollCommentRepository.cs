using HealthInstitution.Core.Polls.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Repository
{
    public interface IPollCommentRepository : IRepository<PollComment>
    {
        public void LoadFromFile();
        public void Save();
        public List<PollComment> GetAll();
        public PollComment GetById(int id);
        public void Add(PollComment pollComment);
        public void Update(int id, PollComment byPollComment);
        public void Delete(int id);

    }
}
