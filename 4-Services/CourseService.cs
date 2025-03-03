using Microsoft.EntityFrameworkCore;

namespace University_backend;

public class CourseService : IDisposable
{

    private readonly UniversityContext _db;

    public CourseService(UniversityContext db)
    {
        _db = db;
    }

    public List<Course> GetAllCourses()
    {
        return _db.Courses.AsNoTracking().ToList();
    }

    public Course? GetOneCourse(Guid id)
    {
        return _db.Courses.AsNoTracking().SingleOrDefault(c => c.Id == id);
    }

    public Course AddCourse(Course course)
    {
        _db.Courses.Add(course);
        _db.SaveChanges();
        return course;
    }

    public Course? UpdateFullCourse(Course course)
    {
        Course? dbCourse = GetOneCourse(course.Id);
        if (dbCourse == null) return null;
        _db.Courses.Attach(course);
        _db.Entry(course).State = EntityState.Modified;
        _db.SaveChanges();
        return course;
    }

    public bool DeleteLesson(Guid id)
    {
        Course? dbCourse = GetOneCourse(id);
        if (dbCourse == null) return false;
        _db.Courses.Remove(dbCourse);
        _db.SaveChanges();
        return true;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
