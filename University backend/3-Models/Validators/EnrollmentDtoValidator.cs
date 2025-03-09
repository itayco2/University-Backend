using FluentValidation;
using System;

namespace University_backend
{
    public class EnrollmentDtoValidator : AbstractValidator<EnrollmentDto>
    {
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentDtoValidator(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;

            RuleFor(enrollment => enrollment.UserId)
                .NotEmpty().WithMessage("Missing user id.")
                .MustAsync(async (userId, cancellation) => await UserExists(userId))
                .WithMessage("User does not exist.");

            RuleFor(enrollment => enrollment.CourseId)
                .NotEmpty().WithMessage("Missing course id.")
                .MustAsync(async (courseId, cancellation) => await CourseExists(courseId))
                .WithMessage("Course does not exist.");

            RuleFor(enrollment => new { enrollment.UserId, enrollment.CourseId })
                .MustAsync(async (data, cancellation) => !await IsUserAlreadyEnrolled(data.UserId, data.CourseId))
                .WithMessage("User is already enrolled in this course.");
        }

        private async Task<bool> UserExists(Guid userId)
        {
            bool userExists = await _enrollmentService.UserExists(userId);
            return userExists;
        }

        private async Task<bool> CourseExists(Guid courseId)
        {
            bool courseExists = await _enrollmentService.CourseExists(courseId);
            return courseExists;
        }

        private async Task<bool> IsUserAlreadyEnrolled(Guid userId, Guid courseId)
        {
            bool isEnrolled = await _enrollmentService.IsUserAlreadyEnrolled(userId, courseId);
            return isEnrolled;
        }
    }
}
