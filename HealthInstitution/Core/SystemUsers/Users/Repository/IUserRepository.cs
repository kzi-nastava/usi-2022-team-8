using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Users.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        public void LoadFromFile();
        public void Save();
        public List<User> GetAll();
        public User GetByUsername(String username);
        public void Add(User user);
        public void Update(User byUser);
        public void Delete(string username);
        public void ChangeBlockedStatus(User user);
    }
}
