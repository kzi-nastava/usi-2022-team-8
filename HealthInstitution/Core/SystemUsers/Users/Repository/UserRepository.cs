using HealthInstitution.Core.SystemUsers.Users.Model;

namespace HealthInstitution.Core.SystemUsers.Users.Repository;

public class UserRepository
{
    public List<User> users { get; set; }
    public Dictionary<String, User> usersByUsername { get; set; }
}