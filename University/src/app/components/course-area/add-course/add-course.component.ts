import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CourseModel } from '../../../models/course.model';
import { CourseService } from '../../../services/course.service';
import { Router } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-add-course',
  imports: [ReactiveFormsModule,CommonModule,MatFormFieldModule,MatButtonModule,MatInputModule],
  templateUrl: './add-course.component.html',
  styleUrl: './add-course.component.css'
})
export class AddCourseComponent implements OnInit {

    private course = new CourseModel();
    public courseForm: FormGroup;

    public constructor(
        private courseService:CourseService,
        private router:Router,
        private formBuilder: FormBuilder, 
        private notifyService: NotifyService
    ){}

    public ngOnInit(): void {
        this.courseForm = this.formBuilder.group({
            titleControl: new FormControl("", [Validators.required, Validators.minLength(2), Validators.maxLength(100)]),
            descriptionControl: new FormControl("", [Validators.required, Validators.minLength(10), Validators.maxLength(1000)]),
        });
    }

    public async send() {
        try {
            this.course.title = this.courseForm.get("titleControl").value;
            this.course.description = this.courseForm.get("descriptionControl").value;
            await this.courseService.addCourse(this.course);
            this.notifyService.success("Course has been added.");
            this.router.navigateByUrl("/courses");
        }
        catch (err: any) {
            this.notifyService.error(err);
        }
    }

    public canceled = false;
    public dialogCanceled() {
        this.canceled = true;
    }

}
