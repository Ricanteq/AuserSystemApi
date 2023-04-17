using Microsoft.AspNetCore.Mvc;
using UserSystemApi.Models;
using UserSystemApi.Services;

namespace UserSystemApi.Controllers;

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

    [HttpPost("login")]
    public async Task<ActionResult<User>> LoginUser(LoginRequest request)
    {
        var user = await _userService.LoginUser(request.Email, request.Password);
        if (user == null) return Unauthorized();

        return user;
    }

    
    }

