using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace University_backend
{
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly ProgressService _progressService;
        private readonly IMapper _mapper;

        public ProgressController(ProgressService progressService, IMapper mapper)
        {
            _progressService = progressService;
            _mapper = mapper;
        }

        [HttpPost("api/progress")]
        public async Task<IActionResult> CreateProgressAsync([FromBody] ProgressDto progressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationError(ModelState.GetAllErrors()));

            try
            {
                ProgressDto createdProgressDto = await _progressService.CreateProgressAsync(progressDto);
                return CreatedAtAction(nameof(GetProgressById), new { id = createdProgressDto.Id }, createdProgressDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ValidationError(ex.Message));
            }
        }

        [HttpGet("api/progress/{id}")]
        public async Task<IActionResult> GetProgressById(Guid id)
        {
            ProgressDto? progressDto = await _progressService.GetProgressByIdAsync(id);
            if (progressDto == null)
                return NotFound(new ResourceNotFound(id));
            return Ok(progressDto);
        }


        [HttpGet("api/progress/user/{userId}/course/{courseId}/progress")]
        public async Task<IActionResult> GetCourseProgressAsync([FromRoute] Guid userId, [FromRoute] Guid courseId)
        {
            var (totalLessons, watchedLessons) = await _progressService.GetCourseProgressAsync(userId, courseId);

            return Ok(new
            {
                TotalLessons = totalLessons,
                WatchedLessons = watchedLessons
            });
        }





    }
}
