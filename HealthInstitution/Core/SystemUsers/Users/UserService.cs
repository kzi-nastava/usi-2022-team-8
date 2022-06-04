using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using System.Windows;

namespace HealthInstitution.Core.SystemUsers.Users
{
    public static class UserService
    {
        static UserRepository s_userRepository = UserRepository.GetInstance();
        public static bool IsUsernameExist(string username)
        {
            return s_userRepository.GetByUsername(username) != null;
        }
        public static void Add(UserDTO userDTO)
        {
            User user = new User(userDTO);
            s_userRepository.Add(user);
        }

        public static void Update(UserDTO userDTO)
        {
            User user = new User(userDTO);
            s_userRepository.Update(user);
        }

        public static void Delete(string username)
        {
            s_userRepository.Delete(username);
        }

        public static User GetByUsername(String username)
        {
            return s_userRepository.GetByUsername(username);
        }

        public static void ChangeBlockedStatus(User user)
        {
            s_userRepository.ChangeBlockedStatus(user);
        }
        public static bool IsUserFound(User user, string passwordInput)
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

        public static bool IsUserBlocked(User user)
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