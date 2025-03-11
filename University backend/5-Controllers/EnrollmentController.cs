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




}