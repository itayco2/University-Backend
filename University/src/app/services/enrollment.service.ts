import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { EnrollmentStore } from '../storage/enrollment-store';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';
import { EnrollmentModel } from '../models/enrollment.model';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {

    private http = inject(HttpClient);
    private enrollmentStore = inject(EnrollmentStore);


    public async addEnrollment(enrollment : EnrollmentModel): Promise<void>{
        const dbEnrollment$ = this.http.post<EnrollmentModel>(environment.enrollmentUrl, EnrollmentModel.toFormData(enrollment));
        const dbEnrollment = await firstValueFrom(dbEnrollment$);
        this.enrollmentStore.addEnrollment(dbEnrollment)
    }

    public async getEnrollmentsByUserId(userId: string): Promise<EnrollmentModel[]> {
        const dbEnrollment$ = this.http.get<EnrollmentModel[]>(`${environment.enrollmentUrl}/user/${userId}`);
        return await firstValueFrom(dbEnrollment$);
    }

    public async userAlreadyEnrolled(userId: string, courseId: string): Promise<boolean> {
        if (!userId || !courseId) {
            throw new Error("Invalid user or course ID.");
        }
    
        const dbEnrollment$ = this.http.get<boolean>(
            `${environment.enrollmentUrl}/is-enrolled/${userId}/${courseId}`
        );
    
        return await firstValueFrom(dbEnrollment$);
    }
    



}
