import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserStore } from '../storage/user-store';
import { jwtDecode } from 'jwt-decode';
import { UserModel } from '../models/user.model';
import { environment } from '../../environments/environment';
import { firstValueFrom } from 'rxjs';
import { CredentialsModel } from '../models/credentials.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

    private http = inject(HttpClient);
    private userStore = inject(UserStore);

    public constructor(){
        const token = localStorage.getItem("token");
        if(!token) return;
        const payload = jwtDecode<{user: UserModel}>(token);
        const dbUser = payload.user;
        this.userStore.initUser(dbUser);
    }
    public async register(user: UserModel): Promise<void>{
        //Creating observable
        const token$ = this.http.post<string>(environment.registerUrl,user);

        //Getting the token
        const token = await firstValueFrom(token$);

        //Reading the token and converting him to user
        const payload = jwtDecode<{user: UserModel}>(token);

        //Put the user into a const
        const dbUser = payload.user;

        //Sending the user to the initUser
        this.userStore.initUser(dbUser);

        //Saving the token in the local storage
        localStorage.setItem("token",token);
        
    }

    public async login(credentials: CredentialsModel): Promise<void>{
        //Creating observable
        const token$ = this.http.post<string>(environment.loginUrl,credentials);

        //Getting the token
        const token = await firstValueFrom(token$);

        //Reading the token and converting him to user
        const payload = jwtDecode<{user: UserModel}>(token);

        //Put the user into a const
        const dbUser = payload.user;

        //Sending the user to the initUser
        this.userStore.initUser(dbUser);


        //Saving the token in the local storage
        localStorage.setItem("token",token);

    }

    //Logout
    public logout(): void{
        this.userStore.logoutUser();

        //Removing the token in the local storage
        localStorage.removeItem("token");
    }
}



