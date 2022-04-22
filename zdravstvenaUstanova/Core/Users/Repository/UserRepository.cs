
using zdravstvenaUstanova.Core.Users.Model;

namespace zdravstvenaUstanova.Core.Users.Repository;

public class UserRepository
{
    public List<User> users { get; set; }
    public Dictionary<String, User> usersByUsername { get; set; }
}