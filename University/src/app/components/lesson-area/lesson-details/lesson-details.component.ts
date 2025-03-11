import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LessonModel } from '../../../models/lesson.model';
import { LessonService } from '../../../services/lesson.service';
import { NotifyService } from '../../../services/notify.service';
import { ProgressService } from '../../../services/progress.service';
import { UserStore } from '../../../storage/user-store';  // Import UserStore to get the user ID
import { ProgressModel } from '../../../models/progress.model';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { AuthService } from '../../../services/auth.service';  // Import the new AuthService

@Component({
  selector: 'app-lesson-details',
  imports: [CommonModule, RouterModule],
  templateUrl: './lesson-details.component.html',
  styleUrls: ['./lesson-details.component.css']
})
export class LessonDetailsComponent implements OnInit {

  public lesson: LessonModel | null = null;  // Initialize lesson as null
  public userId: string = '';  // Store user ID
  public canDeleteLesson: boolean = false; // Flag to determine if the user can delete the lesson
  public userStore = inject(UserStore);  // Inject UserStore
  private authService = inject(AuthService);  // Inject AuthService to check roles

  public constructor(
    private activatedRoute: ActivatedRoute,
    private lessonService: LessonService,
    private router: Router,
    private notifyService: NotifyService,
    private progressService: ProgressService,
    private sanitizer: DomSanitizer
  ) { }

  public async ngOnInit() {
    try {
      const id = this.activatedRoute.snapshot.params['id'];

      // Fetch the lesson using the ID
      this.lesson = await this.lessonService.getOneLesson(id);

      // Get the user ID from the UserStore
      this.userId = this.userStore.user()?.id || '';  

      // Get roles from the AuthService and check if the user can delete the lesson
      const roles = this.authService.getUserRoles();
      this.canDeleteLesson = roles.includes('Admin') || roles.includes('Professor');
      console.log("roles" +roles);
      

      // Handle the case when user ID is not available
      if (!this.userId) {
        this.notifyService.error('User not logged in.');
      }
    } catch (err: any) {
      this.notifyService.error('Failed to load lesson details');
      console.error(err);
    }
  }

  // Method to get the safe video URL
  public getVideoUrl(videoUrl: string): SafeResourceUrl {
    const videoId = this.extractVideoId(videoUrl);
    const embedUrl = `https://www.youtube.com/embed/${videoId}`;
    return this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
  }

  // Helper method to extract the YouTube video ID
  private extractVideoId(url: string): string {
    const regex = /(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})/;
    const match = url.match(regex);
    return match ? match[1] : '';  
  }

  // Method to delete the lesson
  public async deleteMe(id: string) {
    try {
      const sure = confirm('Are you sure?');
      if (!sure) return;

      // Call the service to delete the lesson
      await this.lessonService.deleteLesson(id);
      this.notifyService.success('Lesson has been deleted.');

      // Navigate back to the courses page
      this.router.navigateByUrl('/courses');
    } catch (err: any) {
      this.notifyService.error('Failed to delete lesson');
      console.error(err);
    }
  }

  // Method to mark the lesson as watched
  public async markAsWatched() {
    if (!this.lesson) return;

    try {
      if (this.userId) {
        // Create a ProgressModel to track the user's progress
        const progress = new ProgressModel();
        progress.userId = this.userId;
        progress.lessonId = this.lesson.id;
        progress.watchedAt = new Date();  // Set the watched date

        // Call ProgressService to add progress
        await this.progressService.addProgress(progress);
        this.notifyService.success('Lesson marked as watched!');
      } else {
        this.notifyService.error('User ID is not available!');
      }
    } catch (err: any) {
      this.notifyService.error('Failed to mark lesson as watched');
      console.error(err);
    }
  }
}
