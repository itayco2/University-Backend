using FluentValidation;
using System;

namespace University_backend
{
    public class ProgressDtoValidator : AbstractValidator<ProgressDto>
    {
        private readonly ProgressService _progressService;

        public ProgressDtoValidator(ProgressService progressService)
        {
            _progressService = progressService;

            RuleFor(progress => progress.UserId)
                .NotEmpty().WithMessage("Missing user id.")
                .MustAsync(async (userId, cancellation) => await UserExists(userId))
                .WithMessage("User does not exist.");

            RuleFor(progress => progress.LessonId)
                .NotEmpty().WithMessage("Missing lesson id.")
                .MustAsync(async (lessonId, cancellation) => await LessonExists(lessonId))
                .WithMessage("Lesson does not exist.");

            RuleFor(progress => new { progress.UserId, progress.LessonId })
                .MustAsync(async (data, cancellation) => !await ProgressExists(data.UserId, data.LessonId))
                .WithMessage("Progress already recorded for this lesson.");
        }

        private async Task<bool> UserExists(Guid userId)
        {
            bool userExists = await _progressService.UserExists(userId);
            return userExists;
        }

        private async Task<bool> LessonExists(Guid lessonId)
        {
            bool lessonExists = await _progressService.LessonExists(lessonId);
            return lessonExists;
        }

        private async Task<bool> ProgressExists(Guid userId, Guid lessonId)
        {
            bool progressExists = await _progressService.ProgressExists(userId, lessonId);
            return progressExists;
        }
    }
}
