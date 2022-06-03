using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Users
{
    internal static class UserService
    {
        static UserRepository s_userRepository = UserRepository.GetInstance();

        public static void Add(UserDTO userDTO)
        {
            User user = new User(userDTO);
            s_userRepository.Add(user);
        }

        public static void Update(UserDTO userDTO)
        {
            User user = new User(userDTO);
            s_userRepository.Update(user);
        }

        public static void Delete(string username)
        {
            s_userRepository.Delete(username);
        }
        public static User GetByUsername(String username)
        {
            return s_userRepository.GetByUsername(username);
        }
        public static void ChangeBlockedStatus(User user)
        {
            s_userRepository.ChangeBlockedStatus(user);
        }
    }
}
