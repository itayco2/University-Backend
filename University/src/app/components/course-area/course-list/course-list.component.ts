import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CourseModel } from '../../../models/course.model';
import { CourseService } from '../../../services/course.service';
import { Router, RouterModule } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';

@Component({
  selector: 'app-course-list',
  imports: [CommonModule,RouterModule],
  templateUrl: './course-list.component.html',
  styleUrl: './course-list.component.css'
})
export class CourseListComponent implements OnInit {

    public courses: CourseModel[] = [];
    
    public constructor(
        private courseService:CourseService,
        private router:Router,
        private notifyService:NotifyService
    ){}

    public async ngOnInit() {
        try {
            this.courses = await this.courseService.getAllCourses();
        }
        catch (err: any) {
            this.notifyService.error(err);
        }
    }

   
    enroll(courseTitle: string) {
        this.notifyService.success(`You have successfully enrolled to ${courseTitle} course`);
    }
    
}
