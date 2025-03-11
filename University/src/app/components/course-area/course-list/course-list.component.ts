import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { inject } from '@angular/core';
import { CourseModel } from '../../../models/course.model';
import { CourseService } from '../../../services/course.service';
import { EnrollmentService } from '../../../services/enrollment.service';
import { Router, RouterModule } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';
import { EnrollmentModel } from '../../../models/Enrollment.model';
import { UserStore } from '../../../storage/user-store';

@Component({
  selector: 'app-course-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  public courses: CourseModel[] = [];

  // Using `inject` to inject UserStore
  private userStore = inject(UserStore); // Instead of constructor injection
  public enrolledCourses: EnrollmentModel[] = [];  // To store user's enrolled courses


  constructor(
    private courseService: CourseService,
    private enrollmentService: EnrollmentService, 
    private router: Router,
    private notifyService: NotifyService
  ) {}

  // Fetch courses on initialization
  public async ngOnInit() {
    try {
      this.courses = await this.courseService.getAllCourses();
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
      this.enrolledCourses.push(enrollment);

      this.notifyService.success(`Successfully enrolled in ${course.title}!`);
    } catch (err: any) {
      this.notifyService.error(err);
    }
  }
}
