import { patchState, signalStore, withComputed, withMethods, withState } from "@ngrx/signals";
import { CourseModel } from "../models/course.model"
import { computed } from "@angular/core";
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { environment } from "../../environments/environment";

export type CourseState = {
    courses: CourseModel[];
};

const initialState: CourseState = {
    courses: []
};

export const CourseStore = signalStore(
    {providedIn:"root"},
    
    withState(initialState),

    withMethods(store => ({

        initCourse(courses: CourseModel[]): void {
            patchState(store, _currentState => ({ courses })); 
        },

        addCourse(course: CourseModel): void{
            patchState(store,currentState => ({courses: [...currentState.courses,course]}));
        },

         // Delete existing product from global state: 
           deleteCourse(id: string): void {
             patchState(store, currentState => ({ courses: currentState.courses.filter(c => c.id !== id) })); // Duplicate products but without the deleted one.
          },  
    })),

    // Create computed values: 
    withComputed(store => ({
        count: computed(() => store.courses().length),
    })),

    // Adding reports to debug tool: 
    environment.isDevelopment && withDevtools("CourseStore") 


    
);