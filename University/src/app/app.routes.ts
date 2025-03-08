import { Routes } from '@angular/router';
import { HomeComponent } from './components/page-area/home/home.component';
import { ContactUsComponent } from './components/page-area/contact-us/contact-us.component';
import { Page404Component } from './components/page-area/page-404/page-404.component';
import { CourseListComponent } from './components/course-area/course-list/course-list.component';
import { AddCourseComponent } from './components/course-area/add-course/add-course.component';
import { authGuard } from './services/auth.guard';

export const routes: Routes = [
    { path: "", redirectTo: "/home", pathMatch:"full" },
    { path: "home", component: HomeComponent},
    { path: "contact-us", component: ContactUsComponent},
    { path: "courses", component: CourseListComponent},
    { path: "course/new", component: AddCourseComponent},
    { path: "register", loadComponent: () => import("./components/user-area/register/register.component").then(m => m.RegisterComponent) }, 
    { path: "login", loadComponent: () => import("./components/user-area/login/login.component").then(m => m.LoginComponent) }, 



    { path: "**", component: Page404Component }


];
