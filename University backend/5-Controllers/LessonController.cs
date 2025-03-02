using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace University_backend;

[ApiController]
public class LessonController : ControllerBase
{
    private readonly LessonService _lessonService;

    public LessonController(LessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet("api/lessons")]
    public IActionResult GetAllLessons()
    {
        List<Lessons> lessons = _lessonService.GetAllLessons();
        return Ok(lessons);
    }

    [HttpGet("api/lessons/{id}")]
    public IActionResult GetOneLesson([FromRoute] Guid id)
    {
        Lessons? dbLesson = _lessonService.GetOneLesson(id);
        if(dbLesson == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbLesson);
    }

    //[Authorize]
    [HttpPost("api/lessons")]
    public IActionResult AddLesson([FromForm] Lessons lesson)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));
        Lessons dbLesson = _lessonService.AddLesson(lesson);
        return Created("api/lessons/" + dbLesson.Id, dbLesson);
    }

}
