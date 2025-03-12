using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace University_backend;

[ApiController]
public class EnrollmentController : ControllerBase
{
    private readonly EnrollmentService _enrollmentService;

    public EnrollmentController(EnrollmentService enrollmentService) => _enrollmentService = enrollmentService;


    [HttpGet("api/enrollments")]
    public async Task<IActionResult> GetAllEnrollmentsAsync()
    {
        List<EnrollmentDto> enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
        return Ok(enrollments);
    }

    [HttpPost("api/enrollments")]
    public async Task<IActionResult> CreateEnrollmentAsync([FromForm] EnrollmentDto enrollmentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ValidationError(ModelState.GetAllErrors()));
        try
        {
            enrollmentDto = await _enrollmentService.CreateEnrollmentAsync(enrollmentDto);
            return Created("api/enrollments/" + enrollmentDto.Id, enrollmentDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ValidationError(ex.Message));
        }
    }

    [HttpGet("api/enrollments/is-enrolled/{userId}/{courseId}")]
    public async Task<IActionResult> IsUserEnrolled([FromRoute] Guid userId, [FromRoute] Guid courseId)
    {
        if (userId == Guid.Empty || courseId == Guid.Empty)
        {
            return BadRequest("Invalid user or course ID.");
        }

        bool isEnrolled = await _enrollmentService.IsUserAlreadyEnrolled(userId, courseId);
        return Ok(isEnrolled);
    }






}