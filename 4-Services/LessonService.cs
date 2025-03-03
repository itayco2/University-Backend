using Microsoft.EntityFrameworkCore;

namespace University_backend;

public class LessonService : IDisposable
{


    private readonly UniversityContext _db;

    public LessonService(UniversityContext db)
    {
        _db = db;
    }

    public List<Lesson> GetAllLessons()
    {
        return _db.Lessons.AsNoTracking().ToList();
    }

    public Lesson? GetOneLesson(Guid id)
    {
        return _db.Lessons.AsNoTracking().SingleOrDefault(l => l.Id == id);
    }

    public Lesson AddLesson(Lesson lesson)
    {
        _db.Lessons.Add(lesson);
        _db.SaveChanges();
        return lesson;
    }

    public Lesson? UpdateFullLesson( Lesson lesson)
    {
        Lesson? dbLesson = GetOneLesson(lesson.Id);
        if (dbLesson == null) return null;
        _db.Lessons.Attach(lesson);
        _db.Entry(lesson).State = EntityState.Modified;
        _db.SaveChanges();
        return lesson;

    }

    public bool DeleteLesson(Guid id)
    {
        Lesson? dbLesson = GetOneLesson(id);
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

