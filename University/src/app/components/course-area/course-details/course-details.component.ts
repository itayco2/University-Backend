import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CourseService } from '../../../services/course.service';
import { LessonService } from '../../../services/lesson.service'; // Assuming you have a service for lessons
import { LessonModel } from '../../../models/lesson.model';
import { NotifyService } from '../../../services/notify.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service'; // Add AuthService to check roles

@Component({
  selector: 'app-course-details',
  imports: [CommonModule, RouterModule],
  templateUrl: './course-details.component.html',
  styleUrls: ['./course-details.component.css']
})
export class CourseDetailsComponent implements OnInit {
  public courseId: string = '';
  public lessons: LessonModel[] = [];
  public canAddLesson: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private lessonService: LessonService,
    private notifyService: NotifyService,
    private authService: AuthService 
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.courseId = params.get('courseId')!;
      this.getLessonsForCourse();
      this.checkUserRole(); 
    });
  }

  private async getLessonsForCourse() {
    try {
      this.lessons = await this.lessonService.getLessonsForCourse(this.courseId);
    } catch (err: any) {
      this.notifyService.error('Failed to load lessons for this course');
      console.error('Error fetching lessons:', err);
    }
  }

  private checkUserRole(): void {
    const roles = this.authService.getUserRoles(); 
    this.canAddLesson = roles.includes('Admin') || roles.includes('Professor');
  }
}
