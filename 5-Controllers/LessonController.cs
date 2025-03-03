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
        List<Lesson> lessons = _lessonService.GetAllLessons();
        return Ok(lessons);
    }

    [HttpGet("api/lessons/{id}")]
    public IActionResult GetOneLesson([FromRoute] Guid id)
    {
        Lesson? dbLesson = _lessonService.GetOneLesson(id);
        if(dbLesson == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbLesson);
    }

    //[Authorize]
    [HttpPost("api/lessons")]
    public IActionResult AddLesson([FromForm] Lesson lesson)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));
        Lesson dbLesson = _lessonService.AddLesson(lesson);
        return Created("api/lessons/" + dbLesson.Id, dbLesson);
    }

    //[Authorize]
    [HttpPut("api/lessons/{id}")]
    public IActionResult UpdateFullLesson([FromRoute] Guid id, [FromForm] Lesson lesson)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetFirstError()));
        lesson.Id = id;
        Lesson? dbLesson = _lessonService.UpdateFullLesson(lesson);
        if(dbLesson == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbLesson);

    }

    //[Authorize]
    [HttpDelete("api/lessons/{id}")]
    public IActionResult DeleteLesson([FromRoute] Guid id)
    {
        bool success = _lessonService.DeleteLesson(id);
        if (!success) return NotFound(new ResourceNotFound(id));
        return NoContent(); // 204
    }


}
