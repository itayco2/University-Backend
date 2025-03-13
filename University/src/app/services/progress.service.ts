import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { ProgressModel } from '../models/progress.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProgressService {

  constructor(private http: HttpClient) {}

  // Create progress (mark a lesson as watched)
  createProgress(progress: ProgressModel): Observable<ProgressModel> {
    return this.http.post<ProgressModel>(`${environment.progressUrl}`, progress);
  }

  getCourseProgress(userId: string, courseId: string): Observable<any> {
    return this.http.get<any>(`${environment.progressUrl}/user/${userId}/course/${courseId}/progress`).pipe(
        tap(response => {
            console.log('API Response:', response); // Log the response to check the structure
        })
    );
}

}
