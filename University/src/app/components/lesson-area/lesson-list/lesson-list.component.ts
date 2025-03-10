import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LessonModel } from '../../../models/lesson.model';
import { LessonService } from '../../../services/lesson.service';
import { NotifyService } from '../../../services/notify.service';

@Component({
  selector: 'app-lesson-list',
  imports: [CommonModule,RouterModule],
  templateUrl: './lesson-list.component.html',
  styleUrl: './lesson-list.component.css'
})
export class LessonListComponent implements OnInit {

    public lessons: LessonModel[] = [];

    public constructor(
        private activatedRoute: ActivatedRoute,
        private lessonService: LessonService,
        private router: Router,
        private notifyService: NotifyService
    ){}

    public async ngOnInit(){
        try{
          this.lessons = await this.lessonService.getAllLessons();
        }
        catch(err : any){
            this.notifyService.error(err);
        }
    }

   
}
