import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LessonModel } from '../../../models/lesson.model';
import { LessonService } from '../../../services/lesson.service';
import { NotifyService } from '../../../services/notify.service';

@Component({
  selector: 'app-lesson-list',
  imports: [CommonModule,RouterModule],
  templateUrl: './lesson-details.component.html',
  styleUrl: './lesson-details.component.css'
})
export class LessonListComponent implements OnInit {

    public lesson: LessonModel;

    public constructor(
        private activatedRoute: ActivatedRoute,
        private lessonService: LessonService,
        private router: Router,
        private notifyService: NotifyService
    ){}

    public async ngOnInit(){
        try{
            const id = +this.activatedRoute.snapshot.params["id"];
            // this.lesson = await this.lessonService.getOneLesson(id);
        }
        catch(err : any){
            this.notifyService.error(err);
        }
    }

    public async deleteMe(id: string) {
        try {
            const sure = confirm("Are you sure?");
            if(!sure) return;
            await this.lessonService.deleteLesson(id);
            this.notifyService.success("Lesson has been deleted.");
            this.router.navigateByUrl("/lessons");
        }
        catch (err: any) {
            this.notifyService.error(err);
        }
    }
}
