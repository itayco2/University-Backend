import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CourseStore } from '../storage/course-store';
import { CourseModel } from '../models/course.model';
import { environment } from '../../environments/environment';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  private http = inject(HttpClient);
  private courseStore = inject(CourseStore);

  public async getAllCourses(): Promise<CourseModel[]>{
    if(this.courseStore.count()) return this.courseStore.courses();
    const dbCourse$ = this.http.get<CourseModel[]>(environment.courseUrl);
    const dbCourse = await firstValueFrom(dbCourse$);
    this.courseStore.initCourse(dbCourse);
    return dbCourse;
  }

  public async getOneCourse(id: string): Promise<CourseModel>{
   const dbCourse = this.courseStore.courses().find( c => c.id === id);
   if(dbCourse) return dbCourse;
   const courses$ = this.http.get<CourseModel>(environment.courseUrl + id);
   const dbCourses = await firstValueFrom(courses$);
   return dbCourses;
  }

  public async getCourseById(courseId: string): Promise<CourseModel> {
    const course$ = this.http.get<CourseModel>(`${environment.courseUrl}/${courseId}`);
    return await firstValueFrom(course$);
  }
  

  public async addCourse(course : CourseModel): Promise<void>{
    const dbCourse$ = this.http.post<CourseModel>(environment.courseUrl, CourseModel.toFormData(course));
    const dbCourse = await firstValueFrom(dbCourse$);
    this.courseStore.addCourse(dbCourse)
  }

  public async updateCourse(course : CourseModel): Promise<void>{
    const dbCourse$ = this.http.put<CourseModel>(environment.courseUrl, CourseModel.toFormData(course));
    const dbCourse = await firstValueFrom(dbCourse$);
    this.courseStore.updateCourse(dbCourse)
  }

  public async deleteCourse(id: string): Promise<void>{
    const observable$ = this.http.delete(environment.courseUrl + id);
    await firstValueFrom(observable$);
    this.courseStore.deleteCourse(id);
   }
}
