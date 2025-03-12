    import { Component, inject, OnInit } from '@angular/core';
    import { ProgressModel } from '../../../models/progress.model';
    import { CourseModel } from '../../../models/course.model';
    import { ProgressService } from '../../../services/progress.service';
    import { EnrollmentService } from '../../../services/enrollment.service';
    import { UserStore } from '../../../storage/user-store';
    import { CourseService } from '../../../services/course.service';
    import { EnrollmentModel } from '../../../models/enrollment.model';
    import { CommonModule } from '@angular/common';

    @Component({
    selector: 'app-progress',
    imports: [CommonModule],
    templateUrl: './progress.component.html',
    styleUrls: ['./progress.component.css']
    })
    export class ProgressComponent implements OnInit {

        public courses: CourseModel[] = [];
        public userProgress: ProgressModel[] = [];
        public userId: string = '';

        private progressService = inject(ProgressService);
        private enrollmentService = inject(EnrollmentService);
        private userStore = inject(UserStore);
        private courseService = inject(CourseService);

        ngOnInit(): void {
            const user = this.userStore.user();
            this.userId = user?.id || ''; 
        
            if (this.userId) {
                this.enrollmentService.getEnrollmentsByUserId(this.userId).then((enrollments: EnrollmentModel[]) => {
                    const courseIds = enrollments.map((enrollment: EnrollmentModel) => enrollment.courseId);
                    
                    Promise.all(courseIds.map(courseId => this.courseService.getCourseById(courseId)))
                        .then((courses: CourseModel[]) => {
                            this.courses = courses;
                            
                            this.courses.forEach((course: CourseModel) => {
                                this.progressService.getProgressByUserIdAndProgressId(this.userId, course.id).subscribe((progress: ProgressModel) => {
                                    this.userProgress.push(progress);
                                });
                            });
                        });
                });
            }
        }
    }
