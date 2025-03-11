import { Component, inject, OnInit } from '@angular/core';
import { ProgressService } from '../../../services/progress.service';
import { EnrollmentService } from '../../../services/enrollment.service';
import { CourseModel } from '../../../models/course.model';
import { ProgressModel } from '../../../models/progress.model';
import { UserStore } from '../../../storage/user-store';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CourseService } from '../../../services/course.service';

@Component({
  selector: 'app-progress',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './progress.component.html',
  styleUrls: ['./progress.component.css']
})
export class ProgressComponent implements OnInit {
  public courses: CourseModel[] = [];
  public userProgress: ProgressModel[] = [];
  public userId: string = '';

  private progressService = inject(ProgressService);
  private enrollmentService = inject(EnrollmentService);
  private userStore = inject(UserStore);
  private courseService = inject(CourseService);

  ngOnInit(): void {
    // Get userId from the user store
    const user = this.userStore.user();
    this.userId = user?.id || ''; 

    if (this.userId) {
      this.fetchUserProgress();
      this.fetchEnrolledCourses();
    }
  }

  private async fetchUserProgress() {
    try {
        console.log('Fetching progress for userId:', this.userId); 
        this.userProgress = await this.progressService.getProgressByUserId(this.userId);
    } catch (error) {
        console.error('Error fetching user progress:', error);
    }
}


  private async fetchEnrolledCourses(): Promise<void> {
    try {
      const enrollments = await this.enrollmentService.getEnrollmentsByUserId(this.userId);
      const courseIds = enrollments.map(e => e.courseId);

      // Fetch courses in parallel using Promise.all for better performance
      this.courses = await Promise.all(courseIds.map(courseId => this.courseService.getCourseById(courseId)));
    } catch (error) {
      console.error('Error fetching user-enrolled courses:', error);
    }
  }

  // Get the progress of a specific course
  public getCourseProgress(courseId: string): string {
    const progress = this.userProgress.find(p => p.lessonId === courseId);
    return progress ? `Completed at: ${progress.watchedAt}` : 'Not completed yet';
  }
}
