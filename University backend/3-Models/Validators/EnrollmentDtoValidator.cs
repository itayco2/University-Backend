using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace University_backend
{
    public class EnrollmentDtoValidator : AbstractValidator<EnrollmentDto>
    {
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentDtoValidator(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;

            RuleFor(enrollment => enrollment.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .MustAsync(UserExistsAsync)
                .WithMessage("User does not exist.");

            RuleFor(enrollment => enrollment.CourseId)
                .NotEmpty().WithMessage("Course ID is required.")
                .MustAsync(CourseExistsAsync)
                .WithMessage("Course does not exist.");

            RuleFor(enrollment => new { enrollment.UserId, enrollment.CourseId })
                .MustAsync(EnrollmentDoesNotExistAsync)
                .WithMessage("User is already enrolled in this course.");
        }

        private async Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken)
        {
            bool userExists = await _enrollmentService.UserExists(userId);
            return userExists;
        }

        private async Task<bool> CourseExistsAsync(Guid courseId, CancellationToken cancellationToken)
        {
            bool courseExists = await _enrollmentService.CourseExists(courseId);
            return courseExists;
        }

        private async Task<bool> EnrollmentDoesNotExistAsync(dynamic data, CancellationToken cancellationToken)
        {
            bool isAlreadyEnrolled = await _enrollmentService.IsUserAlreadyEnrolled(data.UserId, data.CourseId);
            return !isAlreadyEnrolled;
        }
    }
}
