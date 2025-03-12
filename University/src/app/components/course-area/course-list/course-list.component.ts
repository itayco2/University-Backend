import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { inject } from '@angular/core';
import { CourseModel } from '../../../models/course.model';
import { CourseService } from '../../../services/course.service';
import { EnrollmentService } from '../../../services/enrollment.service';
import { Router, RouterModule } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';
import { UserStore } from '../../../storage/user-store';
import { EnrollmentModel } from '../../../models/enrollment.model';

@Component({
  selector: 'app-course-list',
  standalone: true, 
  imports: [CommonModule, RouterModule],
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  public courses: CourseModel[] = [];
  public enrolledCourses: EnrollmentModel[] = [];

  private userStore = inject(UserStore); // Using inject instead of constructor injection

  constructor(
    private courseService: CourseService,
    private enrollmentService: EnrollmentService, 
    private router: Router,
    private notifyService: NotifyService
  ) {}

  public async ngOnInit() {
    try {
      this.courses = await this.courseService.getAllCourses();
      await this.loadEnrollments(); 
    } catch (err: any) {
      this.notifyService.error(err);
    }
  }

  private async loadEnrollments() {
    try {
      const user = this.userStore.user();
      if (!user) {
        this.notifyService.error('User not found');
        return;
      }
      for (let course of this.courses) {
        const isEnrolled = await this.enrollmentService.userAlreadyEnrolled(user.id, course.id);
        if (isEnrolled) {
          this.enrolledCourses.push({ userId: user.id, courseId: course.id, id: '', enrolledAt: new Date() });
        }
      }
    } catch (err: any) {
      this.notifyService.error(err);
    }
  }

  public isEnrolled(course: CourseModel): boolean {
    return this.enrolledCourses.some(enrollment => enrollment.courseId === course.id);
  }

  public async enroll(course: CourseModel) {
    try {
      const user = this.userStore.user();
      if (!user) {
        this.notifyService.error('User not found');
        return;
      }

      const enrollment: EnrollmentModel = {
        id: '', 
        userId: user.id, 
        courseId: course.id, 
        enrolledAt: new Date()
      };

      await this.enrollmentService.addEnrollment(enrollment);
      this.enrolledCourses.push(enrollment); // Update the list

      this.notifyService.success(`Successfully enrolled in ${course.title}!`);
    } catch (err: any) {
      this.notifyService.error(err);
    }
  }
}
