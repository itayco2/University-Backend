using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace University_backend
{
    public class CourseService : IDisposable
    {
        private readonly UniversityContext _db;
        private readonly IMapper _mapper;

        public CourseService(UniversityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> GetAllCoursesAsync()
        {
            return await _db.Courses
                .AsNoTracking()
                .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CourseDto?> GetOneCourseAsync(Guid id)
        {
            return await _db.Courses
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<CourseDto> AddCourseAsync(CourseDto courseDto)
        {
            var course = _mapper.Map<Course>(courseDto);

            if (course.CreatedAt == default)
            {
                course.CreatedAt = DateTime.UtcNow;
            }

            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            var dbCourse = await _db.Courses.FindAsync(id);
            if (dbCourse == null) return false;

            _db.Courses.Remove(dbCourse);
            await _db.SaveChangesAsync();
            return true;
        }
        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
