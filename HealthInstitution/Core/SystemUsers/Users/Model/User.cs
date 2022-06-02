using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.SystemUsers.Users.Model;

public class User
{
    public UserType Type { get; set; }
    public String Username { get; set; }
    public String Password { get; set; }
    public String Name { get; set; }
    public String Surname { get; set; }
    public BlockState Blocked { get; set; }

    [JsonConstructor]
    public User(UserType type, String username, String password, String name, String surname)
    {
        this.Type = type;
        this.Username = username;
        this.Password = password;
        this.Name = name;
        this.Surname = surname;
        this.Blocked = BlockState.NotBlocked;
    }
}

public enum UserType
{
    Manager,
    Doctor,
    Secretary,
    Patient
}
public enum BlockState
{
    BlockedBySystem,
    BlockedBySecretary,
    NotBlocked
}