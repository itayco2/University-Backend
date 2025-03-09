using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace University_backend;

[ApiController]
public class UserController : ControllerBase, IDisposable
{
    private readonly UserService _userService;

    public UserController(UserService userService) => _userService = userService;

    [HttpPost("api/register")]
    public async Task<IActionResult> Register([FromBody] RegisterCredentials credentials)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));

        try
        {
            string token = await _userService.Register(credentials);
            return Content(token, "text/plain");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ValidationError(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", detail = ex.Message });
        }
    }

    [HttpPost("api/login")]
    public async Task<IActionResult> Login([FromBody] Credentials credentials)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));

        string? token = await _userService.Login(credentials);
        if (token == null) return Unauthorized(new UnauthorizedError("Incorrect email or password."));

        // Return the token as plain text instead of a JSON object
        return Content(token, "text/plain");
    }

    [HttpGet("api/users/{id}")]
    public async Task<IActionResult> GetOneUser([FromRoute] Guid id)
    {
        UserDto? dbUser = await _userService.GetOneUser(id);
        if (dbUser == null) return NotFound(new ResourceNotFound(id));

        return Ok(dbUser);
    }

    public void Dispose()
    {
        _userService.Dispose();
    }
}
