import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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

  // Get progress by ID
  getProgressById(id: string): Observable<ProgressModel> {
    return this.http.get<ProgressModel>(`${environment.progressUrl}/${id}`);
  }

  // Get progress by user ID and progress ID
  getProgressByUserIdAndProgressId(userId: string, progressId: string): Observable<ProgressModel> {
    return this.http.get<ProgressModel>(`${environment.progressUrl}/${userId}/${progressId}`);
  }
}
