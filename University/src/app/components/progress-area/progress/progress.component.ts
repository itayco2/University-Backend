import { Component, inject, OnInit } from '@angular/core';
import { ProgressService } from '../../../services/progress.service';
import { EnrollmentService } from '../../../services/enrollment.service';
import { UserStore } from '../../../storage/user-store';
import { CourseService } from '../../../services/course.service';
import { CourseModel } from '../../../models/course.model';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-progress',
    imports: [CommonModule, RouterModule],
    templateUrl: './progress.component.html',
    styleUrls: ['./progress.component.css']
})
export class ProgressComponent implements OnInit {

    public courses: CourseModel[] = [];
    public userId: string = '';
    public courseProgress: { courseId: string, totalLessons: number, watchedLessons: number, progressPercentage: number }[] = [];

    private progressService = inject(ProgressService);
    private enrollmentService = inject(EnrollmentService);
    private userStore = inject(UserStore);
    private courseService = inject(CourseService);

    async ngOnInit(): Promise<void> {
        const user = this.userStore.user();
        this.userId = user?.id || '';

        if (this.userId) {
            try {
                // Fetch enrollments and courses
                const enrollments = await this.enrollmentService.getEnrollmentsByUserId(this.userId);
                const courses = await this.courseService.getAllCourses();

                // Filter out the courses the user is enrolled in
                this.courses = courses.filter(course =>
                    enrollments.some(enrollment => enrollment.courseId === course.id)
                );

                // Fetch the course progress for each course
                for (const course of this.courses) {
                    const progressData = await this.progressService.getCourseProgress(this.userId, course.id).toPromise();

                    // Extract data and calculate progress percentage
                    const totalLessons = progressData.totalLessons || 0;
                    const watchedLessons = progressData.watchedLessons || 0;

                    // Only calculate if totalLessons is greater than 0
                    let progressPercentage = 0;
                    if (totalLessons > 0) {
                        progressPercentage = (watchedLessons / totalLessons) * 100;
                    }

                    // Push progress data to courseProgress array
                    this.courseProgress.push({
                        courseId: course.id,
                        totalLessons,
                        watchedLessons,
                        progressPercentage: Math.round(progressPercentage)
                    });
                }

                console.log('Course Progress:', this.courseProgress); // Debugging the progress data

            } catch (error) {
                console.error('Error loading enrollments, courses or progress:', error);
            }
        }
    }

    getCourseCompletionPercentage(courseId: string): number {
        const progress = this.courseProgress.find(p => p.courseId === courseId);
        return progress ? progress.progressPercentage : 0;
    }
}
