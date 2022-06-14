using HealthInstitution.Core.SystemUsers.Users.Model;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.SystemUsers.Users.Repository;

public class UserRepository : IUserRepository
{
    private String _fileName= @"..\..\..\Data\JSON\users.json";
    public List<User> Users { get; set; }
    public Dictionary<String, User> UsersByUsername { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };
    public UserRepository()
    {
        this.Users = new List<User>();
        this.UsersByUsername = new Dictionary<string, User>();
        this.LoadFromFile();
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
    public Dictionary<string, User> GetAllByUsername()
    {
        return UsersByUsername;
    }

    public User GetByUsername(String username)
    {
        if (this.UsersByUsername.ContainsKey(username))
            return this.UsersByUsername[username];
        return null;
    }

    public void Add(User user)
    {
        this.Users.Add(user);
        this.UsersByUsername[user.Username] = user;
        Save();
    }

    public void Update(User byUser)
    {
        User user = GetByUsername(byUser.Username);
        user.Password = byUser.Password;
        user.Name = byUser.Name;
        user.Surname = byUser.Surname;
        this.UsersByUsername[byUser.Username]=user;
        Save();
    }

    public void Delete(string username)
    {
        User user = GetByUsername(username);
        this.Users.Remove(user);
        this.UsersByUsername.Remove(username);
        Save();
    }
    public void ChangeBlockedStatus(User user)
    {
        if (user.Blocked == BlockState.NotBlocked)
            user.Blocked = BlockState.BlockedBySecretary;
        else
            user.Blocked = BlockState.NotBlocked;
        Save();

    }
}