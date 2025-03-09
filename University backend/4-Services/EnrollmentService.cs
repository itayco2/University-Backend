using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University_backend
{
    public class EnrollmentService : IDisposable
    {
        private readonly UniversityContext _db;
        private readonly IMapper _mapper;

        public EnrollmentService(UniversityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> UserExists(Guid userId)
        {
            return await _db.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> CourseExists(Guid courseId)
        {
            return await _db.Courses.AnyAsync(c => c.Id == courseId);
        }

        public async Task<bool> IsUserAlreadyEnrolled(Guid userId, Guid courseId)
        {
            return await _db.Enrollments
                .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        public async Task<List<EnrollmentDto>> GetAllEnrollmentsAsync()
        {
            List<EnrollmentDto> enrollmentDtos = await _db.Enrollments
                .AsNoTracking()
                .ProjectTo<EnrollmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return enrollmentDtos;
        }

        public async Task<EnrollmentDto> CreateEnrollmentAsync(EnrollmentDto enrollmentDto)
        {
            Enrollment enrollment = _mapper.Map<Enrollment>(enrollmentDto);

            bool userExists = await UserExists(enrollment.UserId);
            bool courseExists = await CourseExists(enrollment.CourseId);

            if (!userExists || !courseExists)
                throw new InvalidOperationException("Invalid User or Course.");

            bool isUserAlreadyEnrolled = await IsUserAlreadyEnrolled(enrollment.UserId, enrollment.CourseId);

            if (isUserAlreadyEnrolled)
                throw new InvalidOperationException("User is already enrolled in this course.");

            _db.Enrollments.Add(enrollment);
            await _db.SaveChangesAsync();

            EnrollmentDto enrolledDto = _mapper.Map<EnrollmentDto>(enrollment);
            return enrolledDto;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
