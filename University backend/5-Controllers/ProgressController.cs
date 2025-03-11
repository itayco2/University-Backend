using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // POST: api/progress
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

        // GET: api/progress/{id}
        [HttpGet("api/progress/{id}")]
        public async Task<IActionResult> GetProgressById(Guid id)
        {
            ProgressDto? progressDto = await _progressService.GetProgressByIdAsync(id);
            if (progressDto == null)
                return NotFound(new ResourceNotFound(id));
            return Ok(progressDto);
        }

        // GET: api/progress/{userId}/course/{courseId}
        [HttpGet("api/progress/{userId}/course/{courseId}")]
        public async Task<IActionResult> GetCourseProgress(Guid userId, Guid courseId)
        {
double progress = await _progressService.GetProgressByUserIdAndCourseIdAsync(userId, courseId);
            return Ok(new { progress });
        }

        // GET: api/progress/user/{userId}
        [HttpGet("api/progress/user/{userId}")]
        public async Task<IActionResult> GetProgressByUserId([FromRoute] Guid userId)
        {
            List<ProgressDto> progress = await _progressService.GetProgressByUserIdAsync(userId);
            if (progress == null || !progress.Any())
            {
                return NotFound(new { Message = "No progress found for the specified user." });
            }
            return Ok(progress);
        }

        // GET: api/progress/{userId}/{id}
        [HttpGet("api/progress/{userId}/{id}")]
        public async Task<IActionResult> GetProgressByUserIdAndProgressIdAsync(Guid userId, Guid id)
        {
            ProgressDto? progressDto = await _progressService.GetProgressByUserIdAndProgressIdAsync(userId, id);
            if (progressDto == null)
                return NotFound(new ResourceNotFound(id));
            return Ok(progressDto);
        }
    }
}
