using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using System.Windows;

namespace HealthInstitution.Core.SystemUsers.Users
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public bool IsExist(string username)
        {
            return _userRepository.GetByUsername(username) != null;
        }
        public void Add(UserDTO userDTO)
        {
            User user = new User(userDTO);
            _userRepository.Add(user);
        }

        public void Update(UserDTO userDTO)
        {
            User user = new User(userDTO);
            _userRepository.Update(user);
        }

        public void Delete(string username)
        {
            _userRepository.Delete(username);
        }

        public User GetByUsername(String username)
        {
            return _userRepository.GetByUsername(username);
        }

        public void ChangeBlockedStatus(User user)
        {
            _userRepository.ChangeBlockedStatus(user);
        }
        public bool IsUserFound(User user, string passwordInput)
        {
            if (user == null)
            {
                System.Windows.MessageBox.Show("Username doesn't exist!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (user.Password != passwordInput)
            {
                System.Windows.MessageBox.Show("Username and password don't match!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool IsUserBlocked(User user)
        {
            if (user.Blocked != BlockState.NotBlocked)
            {
                System.Windows.MessageBox.Show("Account is blocked!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }
    }
}