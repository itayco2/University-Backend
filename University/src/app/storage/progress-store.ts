import { computed } from "@angular/core";
import { patchState, signalStore, withComputed, withMethods, withState } from "@ngrx/signals";
import { environment } from "../../environments/environment";
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { ProgressModel } from "../models/progress.model";

export type ProgressState = {
    progress: ProgressModel[];
};

const initialState: ProgressState = {
    progress: [] as ProgressModel[], // Explicitly typing the array
};

export const ProgressStore = signalStore(
    { providedIn: "root" },

    withState(initialState),

    withMethods(store => ({
        initProgress(progress: ProgressModel[]): void {
            patchState(store, _currentState => ({ progress: progress as ProgressModel[] })); // Type casting here
        },

        addProgress(newProgress: ProgressModel): void {
            patchState(store, _currentState => ({
                progress: [..._currentState.progress, newProgress] as ProgressModel[] // Type casting here
            }));
        },

        updateProgress(updatedProgress: ProgressModel): void {
            patchState(store, _currentState => ({
                progress: _currentState.progress.map(p => 
                    p.Id === updatedProgress.Id ? updatedProgress : p
                ) as ProgressModel[] // Type casting here
            }));
        },

        clearProgress(): void {
            patchState(store, _ => ({ progress: [] as ProgressModel[] })); // Type casting here
        }
    })),

    withComputed(store => ({
        count: computed(() => store.progress().length), // Computed count based on progress length

        completedLessonIds: computed(() => 
            store.progress().map(progress => progress.lessonId) // Mapping lessonId to an array
        ),
    })),

    environment.isDevelopment && withDevtools("ProgressStore") // Dev tools for debugging
);
