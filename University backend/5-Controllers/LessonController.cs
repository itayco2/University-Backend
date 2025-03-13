using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace University_backend;

[ApiController]
public class LessonController : ControllerBase
{
    private readonly LessonService _lessonService;

    public LessonController(LessonService lessonService) => _lessonService = lessonService;

    [HttpGet("api/lessons")]
    public async Task<IActionResult> GetAllLessons()
    {
        List<LessonDto> lessons = await _lessonService.GetAllLessons();
        return Ok(lessons);
    }

    [HttpGet("api/lessons/{id}")]
    public async Task<IActionResult> GetOneLesson([FromRoute] Guid id)
    {
        LessonDto? dbLesson = await _lessonService.GetOneLesson(id);
        if (dbLesson == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbLesson);
    }

    [HttpGet("api/courses/{courseId}/lessons")]
    public async Task<IActionResult> GetLessonsForCourse([FromRoute] Guid courseId)
    {
        List<LessonDto> lessons = await _lessonService.GetLessonsForCourse(courseId);
        return Ok(lessons);
    }

    [HttpPost("api/lessons")]
    public async Task<IActionResult> AddLesson([FromBody] LessonDto lessonDto)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));

        LessonDto dbLesson = await _lessonService.AddLesson(lessonDto);
        return Created($"api/lessons/{dbLesson.Id}", dbLesson);
    }

    [HttpDelete("api/lessons/{id}")]
    public async Task<IActionResult> DeleteLesson([FromRoute] Guid id)
    {
        bool success = await _lessonService.DeleteLesson(id);
        if (!success) return NotFound(new ResourceNotFound(id));
        return NoContent();
    }
}