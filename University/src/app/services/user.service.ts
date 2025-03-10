import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserStore } from '../storage/user-store';
import { jwtDecode } from 'jwt-decode';
import { UserModel } from '../models/user.model';
import { environment } from '../../environments/environment';
import { firstValueFrom } from 'rxjs';
import { CredentialsLoginModel, CredentialsModel } from '../models/credentials.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

    private http = inject(HttpClient);
    private userStore = inject(UserStore);

    public constructor() {
        const token = localStorage.getItem("token");
        if(!token) return;
        const payload = jwtDecode<{ user: UserModel }>(token);
        const dbUser = payload.user;
        this.userStore.initUser(dbUser);
    }

    public async register(user: UserModel): Promise<void> {
        const token$ = this.http.post<string>(environment.registerUrl, user,{responseType: 'text' as 'json'});
        const token = await firstValueFrom(token$);
        const payload = jwtDecode<{ user: UserModel }>(token);
        const dbUser = payload.user;
        this.userStore.initUser(dbUser);

        localStorage.setItem("token", token);
    }

    public async login(credentials: CredentialsLoginModel): Promise<void> {
        const token$ = this.http.post<string>(environment.loginUrl, credentials,{responseType: 'text' as 'json'});
        const token = await firstValueFrom(token$);
        const payload = jwtDecode<{ user: UserModel }>(token);
        const dbUser = payload.user;
        this.userStore.initUser(dbUser);
        localStorage.setItem("token", token);
    }

    public logout(): void {
        this.userStore.logoutUser();
        localStorage.removeItem("token");
    }

}



