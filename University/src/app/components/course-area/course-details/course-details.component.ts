import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CourseService } from '../../../services/course.service';
import { LessonService } from '../../../services/lesson.service'; // Assuming you have a service for lessons
import { LessonModel } from '../../../models/lesson.model';
import { NotifyService } from '../../../services/notify.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-course-details',
  imports: [CommonModule,RouterModule],
  templateUrl: './course-details.component.html',
  styleUrls: ['./course-details.component.css']
})
export class CourseDetailsComponent implements OnInit {
  public courseId: string = '';
  public lessons: LessonModel[] = [];

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private lessonService: LessonService,
    private notifyService: NotifyService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.courseId = params.get('courseId')!;
      this.getLessonsForCourse();
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
}
