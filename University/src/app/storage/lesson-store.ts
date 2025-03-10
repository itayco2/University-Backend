import { computed } from "@angular/core";
import { LessonModel } from "../models/lesson.model"
import { patchState, signalStore, withComputed, withMethods, withState } from "@ngrx/signals";
import { environment } from "../../environments/environment";
import { withDevtools } from "@angular-architects/ngrx-toolkit";

export type LessonState ={
    lessons: LessonModel[];
};

const initialState: LessonState = {
    lessons: []
};

export const LessonStore = signalStore(
    {providedIn:"root"},

    withState(initialState),
    
    withMethods(store => ({

        initLessons(lessons: LessonModel[]):void{
            patchState(store, _currentState => ({lessons}));   
        },

        addLesson(lesson:LessonModel):void{
            patchState(store,currentState => ({lessons: [...currentState.lessons,lesson]}));
        },

        updateLesson(lesson: LessonModel): void{
            patchState(store,currentState => ({lessons: currentState.lessons.map(l => l.id === lesson.id ? lesson : l)}));
        },

        deleteLesson(id: string):void{
            patchState(store,currentState => ({lessons: currentState.lessons.filter(l => l.id !== id )}));
        }
   
    })),
    withComputed(store => ({
        // Product count: 
        count: computed(() => store.lessons().length),
    })),

    // Adding reports to debug tool: 
    environment.isDevelopment && withDevtools("LessonStore") 
    
)