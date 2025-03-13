import { patchState, signalStore, withComputed, withMethods, withState } from "@ngrx/signals";
import { computed } from "@angular/core";
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { environment } from "../../environments/environment";
import { EnrollmentModel } from "../models/enrollment.model";

export type EnrollmentState = {
    enrollments: EnrollmentModel[];
};

const initialState: EnrollmentState = {
    enrollments: []
};

export const EnrollmentStore = signalStore(
    {providedIn:"root"},
    withState(initialState),

    withMethods(store => ({

        addEnrollment(enrollment: EnrollmentModel): void{
            patchState(store,currentState => ({enrollments: [...currentState.enrollments,enrollment]}));
        }

    })),
    withComputed(store => ({
        count: computed(() => store.enrollments().length),
    })),

    environment.isDevelopment && withDevtools("EnrollmentStore")
);