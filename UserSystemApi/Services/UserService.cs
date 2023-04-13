using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using UserSystemApi.Models;

namespace UserSystemApi.Services
{
    public class UserService
    {
        private static readonly List<User> _userList = new();

        public async Task<User> CreateUser(User user)
        {
            user.Id = _userList.Count + 1;
            var newUser = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = HashPassword(user.Password)
            };
            _userList.Add(newUser);
            return await Task.FromResult(newUser);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public List<User> GetAllUsers()
        {
            return _userList;
        }

        public User GetUserById(int id)
        {
            return _userList.FirstOrDefault(u => u.Id == id);
        }

        public async Task<bool> VerifyPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public async Task UpdateUser(User user)
        {
            var existingUser = _userList.FirstOrDefault(u => u.Id == user.Id);
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Password = HashPassword(user.Password);
        }

        public void DeleteUser(int id)
        {
            var user = _userList.FirstOrDefault(u => u.Id == id);
            _userList.Remove(user);
        }
    }
}