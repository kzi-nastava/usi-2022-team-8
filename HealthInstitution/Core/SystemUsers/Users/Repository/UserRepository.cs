using HealthInstitution.Core.SystemUsers.Users.Model;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.SystemUsers.Users.Repository;

public class UserRepository
{
    private String _fileName;
    public List<User> Users { get; set; }
    public Dictionary<String, User> UsersByUsername { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };
    private UserRepository(String fileName)
    {
        this._fileName = fileName;
        this.Users = new List<User>();
        this.UsersByUsername = new Dictionary<string, User>();
        this.LoadFromFile();
    }
    private static UserRepository s_instance = null;
    public static UserRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new UserRepository(@"..\..\..\Data\JSON\users.json");
            }
            return s_instance;
        }
    }
    public void LoadFromFile()
    {
        var users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(@"..\..\..\Data\JSON\users.json"), _options);
        foreach (User user in users)
        {
            this.Users.Add(user);
            this.UsersByUsername[user.Username] = user;
        }
    }

    public void Save()
    {
        var allUsers = JsonSerializer.Serialize(this.Users, _options);
        File.WriteAllText(this._fileName, allUsers);
    }

    public List<User> GetAll()
    {
        return this.Users;
    }

    public User GetByUsername(String username)
    {
        if (this.UsersByUsername.ContainsKey(username))
            return this.UsersByUsername[username];
        return null;
    }

    public void Add(UserDTO userDTO)
    {
        UserType type = userDTO.Type;
        String username = userDTO.Username;
        String password = userDTO.Password;
        String name = userDTO.Name;
        String surname = userDTO.Surname;

        User user = new User(type, username, password, name, surname);
        this.Users.Add(user);
        this.UsersByUsername[username] = user;
        Save();
    }

    public void Update(UserDTO userDTO)
    {
        User user = GetByUsername(userDTO.Username);
        user.Password = userDTO.Password;
        user.Name = userDTO.Name;
        user.Surname = userDTO.Surname;
        this.UsersByUsername[userDTO.Username]=user;
        Save();
    }

    public void Delete(string username)
    {
        User user = GetByUsername(username);
        this.Users.Remove(user);
        this.UsersByUsername.Remove(username);
        Save();
    }
}