using HealthInstitution.Core.Polls.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Repository
{
    public interface IPollQuestionRepository: IRepository<PollQuestion>
    {
        public void LoadFromFile();
        public void Save();
        public List<PollQuestion> GetAll();
        public PollQuestion GetById(int id);
        public void Add(PollQuestion pollQuestion);
        public void Update(int id, PollQuestion byPollQuestion);
        public void Delete(int id);
    }
}
