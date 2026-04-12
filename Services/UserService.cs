using PaperNest_API.Models;
using PaperNest_API.Repository;

namespace PaperNest_API.Services
{
    public class UserService
    {
        public static void Add(User user)
        {
            UserRepository.userRepository.Add(user);
        }

        public static IEnumerable<User> GetAll()
        {
            return UserRepository.userRepository;
        }
         
        public static User? GetById(Guid id)
        {
            return UserRepository.userRepository?.FirstOrDefault(u => u.Id == id);
        }

        public static void Update(Guid id, User user)
        {
            var existingUser = GetById(id);

            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Password = user.Password;
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
            }
        }

        public static bool ResetPassword(string username, string newPassword)
        {
            var user = UserRepository.userRepository.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return false;
            }

            user.Password = newPassword;
            return true;
        }

        public static void Delete(Guid id)
        {
            var existingUser = GetById(id);

            if (existingUser != null)
            {
                UserRepository.userRepository.Remove(existingUser); 
            }
        }
    }
}