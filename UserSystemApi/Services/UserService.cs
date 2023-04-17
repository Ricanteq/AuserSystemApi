using UserSystemApi.Models;

namespace UserSystemApi.Services;

public class UserService
{
    private static readonly List<User> _userList = new();

    public async Task<User> CreateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName)) throw new ArgumentException("Firstname cannot be empty");
        if (user.LastName == null) throw new ArgumentException("Lastname cannot be empty");
        if (user.Email == null) throw new ArgumentException("Email cannot be empty");
        if (user.Password == null) throw new ArgumentException("Password cannot be empty");
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

    public async Task<User?> LoginUser(string email, string password)
    {
        var user = _userList.FirstOrDefault(u => u.Email == email);
        if (user == null) return null;

        var passwordMatches = await VerifyPassword(user, password);
        return passwordMatches ? user : null;
    }
}