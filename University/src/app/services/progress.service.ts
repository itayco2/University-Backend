import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ProgressModel } from '../models/progress.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProgressService {

  private http = inject(HttpClient);

  public async addProgress(progress: ProgressModel): Promise<void> {
    try {
      const formData = progress.toFormData();
      await this.http.post(`${environment.progressUrl}/add`, formData).toPromise();
      console.log('Progress added successfully');
    } catch (err) {
      console.error('Error adding progress:', err);
      throw new Error('Failed to add progress');
    }
  }

  public async getProgressByUserId(userId: string): Promise<ProgressModel[]> {
    try {
        const response = await this.http.get<ProgressModel[]>(`${environment.progressUrl}/user/${userId}`).toPromise();
        console.log(`Fetched progress for user ${userId}:`, response);
        return response;
    } catch (err) {
        console.error(`Error fetching progress for user ${userId}:`, err);
        throw new Error('Failed to fetch user progress');
    }
}


  public async getCourseProgress(userId: string, courseId: string): Promise<number> {
    try {
      const response = await this.http.get<{ progress: number }>(`${environment.progressUrl}/${userId}/course/${courseId}`).toPromise();
      console.log(`Progress for course ${courseId} for user ${userId}:`, response);
      return response.progress;
    } catch (err) {
      console.error(`Error fetching progress for course ${courseId} for user ${userId}:`, err);
      throw new Error('Failed to fetch course progress');
    }
  }
}
