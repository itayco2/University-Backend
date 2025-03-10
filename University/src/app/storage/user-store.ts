import { patchState, signalStore, withComputed, withMethods, withState } from "@ngrx/signals";
import { UserModel } from "../models/user.model";
import { computed } from "@angular/core";
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { environment } from "../../environments/environment";

export type UserState = {
    user: UserModel | null;
};

const initialState: UserState = {
    user: null
};

export const UserStore = signalStore(
    { providedIn: "root" },
    withState(initialState),
    withMethods(store => ({
        initUser(user: UserModel): void {
            try {
                patchState(store, _currentState => ({ user: user as UserModel }));
            } catch (error) {
                console.error("Failed to initialize user:", error);
            }
        },

        logoutUser(): void {
            try {
                patchState(store, _currentState => ({ user: null as UserModel }));
            } catch (error) {
                console.error("Failed to logout user:", error);
            }
        }
    })),

    withComputed(store => ({
        fullname: computed(() => `${store.user()?.name}`)
    })),

    environment.isDevelopment && withDevtools("UserStore")
);