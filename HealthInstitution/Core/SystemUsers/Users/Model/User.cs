using System.Text.Json;
namespace HealthInstitution.Core.SystemUsers.Users.Model;

public class User
{
    public UserType type { get; set; }
    public String username { get; set; }
    public String password { get; set; }
    [field: NonSerializedAttribute()] public String name { get; set; }
    [field: NonSerializedAttribute()] public String surname { get; set; }
    [field: NonSerializedAttribute()] public BlockState blocked { get; set; }

    public User(UserType type, string username, string password, string name, string surname)
    {
        this.type = type;
        this.username = username;
        this.password = password;
        this.name = name;
        this.surname = surname;
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