using HealthInstitution.Core.SystemUsers.Users.Model;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.SystemUsers.Users.Repository;

public class UserRepository
{
    public String fileName { get; set; }
    public List<User> users { get; set; }
    public Dictionary<String, User> usersByUsername { get; set; }

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };
    private UserRepository(String fileName)
    {
        this.fileName = fileName;
        this.users = new List<User>();
        this.usersByUsername = new Dictionary<string, User>();
        this.LoadUsers();
    }
    private static UserRepository instance = null;
    public static UserRepository GetInstance()
    {
        {
            if (instance == null)
            {
                instance = new UserRepository(@"..\..\..\Data\JSON\users.json");
            }
            return instance;
        }
    }
    public void LoadUsers()
    {
        var users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(@"..\..\..\Data\JSON\users.json"), options);
        foreach (User user in users)
        {
            this.users.Add(user);
            this.usersByUsername[user.username] = user;
        }
    }

    public void SaveUsers()
    {
        var allUsers = JsonSerializer.Serialize(this.users, options);
        File.WriteAllText(this.fileName, allUsers);
    }

    public List<User> GetUsers()
    {
        return this.users;
    }

    public User GetUserByUsername(String username)
    {
        if (this.usersByUsername.ContainsKey(username))
            return this.usersByUsername[username];
        return null;
    }

    public void AddUser(UserType type, string username, string password, string name, string surname)
    {
        User user = new User(type, username, password, name, surname);
        this.users.Add(user);
        this.usersByUsername[username] = user;
        SaveUsers();
    }

    public void UpdateUser(string username, string password, string name, string surname)
    {
        User user = GetUserByUsername(username);
        user.password = password;
        user.name = name;
        user.surname = surname;
        this.usersByUsername[username]=user;
        SaveUsers();
    }

    public void DeleteUser(string username)
    {
        User user = GetUserByUsername(username);
        this.users.Remove(user);
        this.usersByUsername.Remove(username);
        SaveUsers();
    }
}