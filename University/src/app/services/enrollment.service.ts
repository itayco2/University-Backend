import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { EnrollmentStore } from '../storage/enrollment-store';
import { firstValueFrom } from 'rxjs';
import { EnrollmentModel } from '../models/Enrollment.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {

    private http = inject(HttpClient);
    private enrollmentStore = inject(EnrollmentStore);


    public async getAllEnrollments(): Promise<EnrollmentModel[]>{
        if(this.enrollmentStore.count()) return this.enrollmentStore.enrollments();
        const dbEnrollment$ = this.http.get<EnrollmentModel[]>(environment.enrollmentUrl);
        const dbEnrollment = await firstValueFrom(dbEnrollment$);
        this.enrollmentStore.initEnrollment(dbEnrollment);
        return dbEnrollment;
    }

    public async addEnrollment(enrollment : EnrollmentModel): Promise<void>{
        const dbEnrollment$ = this.http.post<EnrollmentModel>(environment.enrollmentUrl, EnrollmentModel.toFormData(enrollment));
        const dbEnrollment = await firstValueFrom(dbEnrollment$);
        this.enrollmentStore.addEnrollment(dbEnrollment)
    }

    public async getEnrollmentsByUserId(userId: string): Promise<EnrollmentModel[]> {
        const dbEnrollment$ = this.http.get<EnrollmentModel[]>(`${environment.enrollmentUrl}/user/${userId}`);
        return await firstValueFrom(dbEnrollment$);
    }
    



}
