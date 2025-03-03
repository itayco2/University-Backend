using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace University_backend;

[ApiController]
public class UserController : ControllerBase, IDisposable
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("api/register")]
    public IActionResult Register([FromBody] User user)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));

        if (_userService.IsEmailTaken(user.Email)) return BadRequest(new ValidationError($"Email {user.Email} already taken."));

        string token = _userService.Register(user);
        return Created("", token);
    }

    [HttpPost("api/login")]
    public IActionResult Login([FromBody] Credentials credentials)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));
        string? token = _userService.Login(credentials);
        if (token == null) return Unauthorized(new UnauthorizedError("Incorrect email or password."));
        return Ok(token);
    }

    [HttpGet("api/users/{id}")]
    public IActionResult GetOneUser([FromRoute] Guid id)
    {
        User? dbUser = _userService.GetOneUser(id);
        if (dbUser == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbUser);
    }

    public void Dispose()
    {
        _userService.Dispose();
    }
}
