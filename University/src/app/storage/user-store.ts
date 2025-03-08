import { patchState, signalStore, withComputed, withMethods, withState } from "@ngrx/signals";
import { UserModel } from "../models/user.model";
import { computed } from "@angular/core";
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { environment } from "../../environments/environment";

export type UserState={
    user:UserModel;
};

const initialState: UserState ={
    user:null
};

export const UserStore = signalStore(
    {providedIn:"root"},
    withState(initialState),
    withMethods(store =>({
        initUser(user:UserModel):void{
            patchState(store, _currentState => ({user}));
        },

        logoutUser():void{ //Logout user.
            patchState(store, _currentState => ({user : null as UserModel}));
        }
    })),

    withComputed(store => ({
        fullname: computed(() => `${store.user()?.name}`)
    })),
    
        // Adding reports to debug tool: 
       environment.isDevelopment && withDevtools("UserStore")
);