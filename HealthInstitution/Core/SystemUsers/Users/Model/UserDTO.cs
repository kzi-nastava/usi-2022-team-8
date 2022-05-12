using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Users.Model
{
    public class UserDTO
    {
        public UserType Type { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }

        public UserDTO(UserType type, String username, String password, String name, String surname)
        {
            this.Type = type;
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Surname = surname;
        }
    }
}
