import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LessonStore } from '../storage/lesson-store';
import { LessonModel } from '../models/lesson.model';
import { environment } from '../../environments/environment';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LessonService {

  private http = inject(HttpClient);
  private lessonStore = inject(LessonStore);

  public async getAllLessons(): Promise<LessonModel[]> {
    if (this.lessonStore.count()) return this.lessonStore.lessons();
    const dbLesson$ = this.http.get<LessonModel[]>(environment.lessonUrl);
    const dbLesson = await firstValueFrom(dbLesson$);
    this.lessonStore.initLessons(dbLesson);
    return dbLesson;
  }

public async getLessonsForCourse(courseId: string): Promise<LessonModel[]> {
    const lessons = this.lessonStore.lessons();
    if (lessons.length) return lessons.filter(lesson => lesson.courseId === courseId);
    const lessons$ = this.http.get<LessonModel[]>(`${environment.lessonUrl}?courseId=${courseId}`);
    const dbLessons = await firstValueFrom(lessons$);
    this.lessonStore.initLessons(dbLessons);
    return dbLessons.filter(lesson => lesson.courseId === courseId);
}

  public async getOneLesson(id: string): Promise<LessonModel> {
    const dbLesson = this.lessonStore.lessons().find(l => l.id === id);
    if (dbLesson) return dbLesson;
    const lessons$ = this.http.get<LessonModel>(environment.lessonUrl + id);
    const dbLessons = await firstValueFrom(lessons$);
    return dbLessons;
  }

public async addLesson(lesson: LessonModel): Promise<void> {
    const dbLesson$ = this.http.post<LessonModel>(environment.lessonUrl, lesson, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  
    const dbLesson = await firstValueFrom(dbLesson$);
    this.lessonStore.addLesson(dbLesson);
  }

  public async deleteLesson(id: string): Promise<void> {
    const dbLesson$ = this.http.delete<LessonModel>(environment.lessonUrl + id);
    await firstValueFrom(dbLesson$);
    this.lessonStore.deleteLesson(id);
  }
}