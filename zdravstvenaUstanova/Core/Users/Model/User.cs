namespace zdravstvenaUstanova.Core.Users.Model;

public class User
{
    public UserType type { get; set; }
    public String username { get; set; }
    public String password { get; set; }
    
    public String name { get; set; }
    public String surname { get; set; }
    public BlockState blocked { get; set; }

    public User(UserType type, string username, string password, string name, string surname, BlockState blocked)
    {
        this.type = type;
        this.username = username;
        this.password = password;
        this.name = name;
        this.surname = surname;
        this.blocked = blocked;
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