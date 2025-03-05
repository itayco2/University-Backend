using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace University_backend;

[ApiController]
public class CoursesController : ControllerBase
{
    private readonly CourseService _courseService;

    public CoursesController(CourseService courseService) => _courseService = courseService;


    [HttpGet("api/courses")]
    public async Task<IActionResult> GetAllCoursesAsync()
    {
        List<CourseDto> courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("api/courses/{id}")]
    public async Task<IActionResult> GetOneCourseAsync([FromRoute] Guid id)
    {
        CourseDto? dbCourse = await  _courseService.GetOneCourseAsync(id);
        if (dbCourse == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbCourse);
    }

    [HttpPost("api/courses")]
    public async Task<IActionResult> AddCourseAsync([FromForm] CourseDto courseDto)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));
        courseDto = await _courseService.AddCourseAsync(courseDto);
        return Created("api/courses/" + courseDto.Id, courseDto);
    }


    [HttpPut("api/courses/{id}")]
    public async Task<IActionResult> UpdateFullCourseAsync([FromRoute] Guid id, [FromForm] CourseDto courseDto)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetFirstError()));
        courseDto.Id = id;
        CourseDto? updatedCourseDto = await _courseService.UpdateFullCourseAsync(courseDto);
        if (updatedCourseDto == null) return NotFound(new ResourceNotFound(id));
        return Ok(updatedCourseDto);

    }

    [HttpDelete("api/courses/{id}")]
    public async Task<IActionResult> DeleteCourseAsync([FromRoute] Guid id)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetFirstError()));
        bool success = await _courseService.DeleteCourseAsync(id);
        if (!success) return NotFound(new ResourceNotFound(id));
        return NoContent();
    }
}
