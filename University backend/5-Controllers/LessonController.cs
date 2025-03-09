using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("api/lessons")]
    public async Task<IActionResult> AddLesson([FromBody] LessonDto lessonDto)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetAllErrors()));

        LessonDto dbLesson = await _lessonService.AddLesson(lessonDto);
        return Created($"api/lessons/{dbLesson.Id}", dbLesson);
    }

    [HttpPut("api/lessons/{id}")]
    public async Task<IActionResult> UpdateFullLesson([FromRoute] Guid id, [FromBody] LessonDto lessonDto)
    {
        if (!ModelState.IsValid) return BadRequest(new ValidationError(ModelState.GetFirstError()));

        lessonDto.Id = id;
        LessonDto? dbLesson = await _lessonService.UpdateFullLesson(lessonDto);

        if (dbLesson == null) return NotFound(new ResourceNotFound(id));
        return Ok(dbLesson);
    }

    [HttpDelete("api/lessons/{id}")]
    public async Task<IActionResult> DeleteLesson([FromRoute] Guid id)
    {
        bool success = await _lessonService.DeleteLesson(id);
        if (!success) return NotFound(new ResourceNotFound(id));
        return NoContent();
    }
}