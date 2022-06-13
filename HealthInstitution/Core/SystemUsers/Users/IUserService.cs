using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Users
{
    public interface IUserService
    {
        public void Add(UserDTO userDTO);
        public void Update(UserDTO userDTO);
        public void Delete(string username);
        public User GetByUsername(String username);
        public void ChangeBlockedStatus(User user);
        public bool IsUserFound(User user, string passwordInput);
        public bool IsUserBlocked(User user);
    }
}
