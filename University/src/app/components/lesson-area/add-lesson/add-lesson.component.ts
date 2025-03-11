import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LessonService } from '../../../services/lesson.service';
import { NotifyService } from '../../../services/notify.service';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LessonModel } from '../../../models/lesson.model';

@Component({
  selector: 'app-add-lesson',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-lesson.component.html',
  styleUrl: './add-lesson.component.css'
})
export class AddLessonComponent implements OnInit {
  private lesson = new LessonModel();
  public lessonForm: FormGroup;
  public courseId: string = '';

  constructor(
    private lessonService: LessonService,
    private notifyService: NotifyService,
    private router: Router,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute 
  ) {}

  ngOnInit(): void {
    this.courseId = this.route.snapshot.paramMap.get('id') || '';
    console.log('Course ID:', this.courseId); 

    this.lessonForm = this.formBuilder.group({
      titleControl: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100),
      ]),
      videoUrlControl: new FormControl('', [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(1000),
      ]),
    });
  }

  public async send() {
    try {
      console.log('Title:', this.lessonForm.get('titleControl')?.value);  // Log the title
      console.log('Video URL:', this.lessonForm.get('videoUrlControl')?.value);  // Log the video URL
      console.log('Course ID:', this.courseId); // Log the course ID
  
      this.lesson.title = this.lessonForm.get('titleControl')?.value;
      this.lesson.videoUrl = this.lessonForm.get('videoUrlControl')?.value;
      this.lesson.courseId = this.courseId;
  
      if (!this.lesson.title || !this.lesson.videoUrl || !this.lesson.courseId) {
        this.notifyService.error('All fields are required');
        return;
      }
  
      await this.lessonService.addLesson(this.lesson);
      this.notifyService.success('Lesson has been added.');
  
      this.router.navigateByUrl(`/course-details/${this.courseId}/lessons`);
    } catch (err: any) {
      console.error('Error:', err);  // Log the full error for debugging
      this.notifyService.error('Error adding the lesson.');
    }
  }
  

  public canceled = false;
  public dialogCanceled() {
    this.canceled = true;
  }
}
