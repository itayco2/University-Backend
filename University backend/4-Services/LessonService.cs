using Microsoft.EntityFrameworkCore;

namespace University_backend;

public class LessonService : IDisposable
{


    private readonly UniversityContext _db;

    public LessonService(UniversityContext db)
    {
        _db = db;
    }

    public List<Lessons> GetAllLessons()
    {
        return _db.Lessons.AsNoTracking().ToList();
    }

    public Lessons? GetOneLesson(Guid id)
    {
        return _db.Lessons.AsNoTracking().SingleOrDefault(l => l.Id == id);
    }

    public Lessons AddLesson(Lessons lesson)
    {
        _db.Lessons.Add(lesson);
        _db.SaveChanges();
        return lesson;
    }

    public Lessons? UpdateFullLesson( Lessons lesson)
    {
        Lessons? dbLesson = GetOneLesson(lesson.Id);
        if (dbLesson == null) return null;
        _db.Lessons.Attach(lesson);
        _db.Entry(lesson).State = EntityState.Modified;
        _db.SaveChanges();
        return lesson;

    }

    public bool DeleteLesson(Guid id)
    {
        Lessons? dbLesson = GetOneLesson(id);
        if (dbLesson == null) return false;
        _db.Lessons.Remove(dbLesson);
        _db.SaveChanges();
        return true;
    }

    public void Dispose()
    {
         _db.Dispose();
    }
}

