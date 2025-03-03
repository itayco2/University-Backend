using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace University_backend;

[ApiController]
public class CoursesController : ControllerBase
{
    private readonly CourseService _courseService;

    public CoursesController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("api/courses")]
    public IActionResult GetAllCourses()
    {
        List<Course> courses = _courseService.GetAllCourses();
        return Ok(courses);
    }

    [HttpGet("api/courses/{id}")]
    public IActionResult GetOneCourse([FromRoute] Guid id)
    {
        Course? dbCourses = _courseService.GetOneCourse(id);
        if (dbCourses == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbCourses);
    }

    //[Authorize]
    [HttpPost("api/courses")]
    public IActionResult AddCourse([FromForm] Course course)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));
        Course dbCourse = _courseService.AddCourse(course);
        return Created("api/courses/" + dbCourse.Id, dbCourse);
    }

    //[Authorize]
    [HttpPut("api/courses/{id}")]
    public IActionResult UpdateFullCourse([FromRoute] Guid id, [FromForm] Course course)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetFirstError()));
        course.Id = id;
        Course? dbCourses = _courseService.UpdateFullCourse(course);
        if (dbCourses == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbCourses);

    }

    //[Authorize]
    [HttpDelete("api/courses/{id}")]
    public IActionResult DeleteCourse([FromRoute] Guid id)
    {
        bool success = _courseService.DeleteLesson(id);
        if (!success) return NotFound(new ResourceNotFound(id));
        return NoContent(); // 204
    }
}
