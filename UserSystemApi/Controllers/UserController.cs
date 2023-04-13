using Microsoft.AspNetCore.Mvc;
using UserSystemApi.Models;

namespace UserSystemApi.Services;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Create(User user)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName)) return BadRequest("Firstname cannot be empty");
        if (user.LastName == null) return BadRequest("Lastname cannot be empty");
        if (user.Email == null) return BadRequest("Email cannot be empty");
        if (user.Password == null) return BadRequest("Password cannot be empty");

        return Ok(_userService.CreateUser(user));
    }


    [HttpGet]
    public ActionResult<List<User>> GetAllUsers()
    {
        return _userService.GetAllUsers();
    }


    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null) return NotFound();

        return user;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User updatedUser)
    {
        var userToUpdate = _userService.GetUserById(id);
        if (userToUpdate == null) return NotFound();

        userToUpdate.FirstName = updatedUser.FirstName ?? userToUpdate.FirstName;
        userToUpdate.LastName = updatedUser.LastName;
        userToUpdate.Email = updatedUser.Email;
        userToUpdate.Password = updatedUser.Password;

        return Ok(userToUpdate);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        _userService.DeleteUser(id);
        return NoContent();
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}