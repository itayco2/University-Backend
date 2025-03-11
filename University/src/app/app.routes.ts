import { Routes } from '@angular/router';
import { HomeComponent } from './components/page-area/home/home.component';
import { ContactUsComponent } from './components/page-area/contact-us/contact-us.component';
import { Page404Component } from './components/page-area/page-404/page-404.component';
import { CourseListComponent } from './components/course-area/course-list/course-list.component';
import { AddCourseComponent } from './components/course-area/add-course/add-course.component';
import { authGuard } from './services/auth.guard';
import { LessonListComponent } from './components/lesson-area/lesson-list/lesson-list.component';
import { CourseDetailsComponent } from './components/course-area/course-details/course-details.component';
import { ProgressComponent } from './components/progress-area/progress/progress.component';
import { LessonDetailsComponent } from './components/lesson-area/lesson-details/lesson-details.component';
import { AddLessonComponent } from './components/lesson-area/add-lesson/add-lesson.component';

export const routes: Routes = [
    { path: "", redirectTo: "/home", pathMatch:"full" },
    { path: "home", component: HomeComponent},
    { path: "contact-us", component: ContactUsComponent},
    { path: "courses", component: CourseListComponent,  canActivate:[authGuard]},
    { path: "course/new", component: AddCourseComponent, canActivate:[authGuard], data : {roles: ['Admin', 'Professor']} },
    { path: 'course-details/:courseId/lessons', component: CourseDetailsComponent, canActivate:[authGuard] },
    { path: "lessons/:id", component: LessonListComponent ,canActivate:[authGuard]},
    { path: "lesson-details/:id", component: LessonDetailsComponent, canActivate:[authGuard]},
    { path: "lesson/new/:id", component: AddLessonComponent, canActivate:[authGuard], data : {roles: ['Admin', 'Professor']} },
    { path: "progress/:id", component: ProgressComponent , canActivate:[authGuard]},
    { path: "register", loadComponent: () => import("./components/user-area/register/register.component").then(m => m.RegisterComponent) }, 
    { path: "login", loadComponent: () => import("./components/user-area/login/login.component").then(m => m.LoginComponent) }, 



    { path: "**", component: Page404Component }


];
