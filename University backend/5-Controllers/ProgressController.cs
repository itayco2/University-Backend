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
