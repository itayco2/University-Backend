import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { UserStore } from '../../../storage/user-store';
import { UserService } from '../../../services/user.service';
import { NotifyService } from '../../../services/notify.service';

@Component({
  selector: 'app-user-menu',
  imports: [CommonModule,RouterLink],
  templateUrl: './user-menu.component.html',
  styleUrl: './user-menu.component.css'
})
export class UserMenuComponent {
    public userStore = inject(UserStore);
    public userService = inject(UserService);
    public notifyService = inject(NotifyService);
    public router = inject(Router);

    menuOpen: boolean = false; // To track whether the dropdown is open

    toggleMenu(): void {
        this.menuOpen = !this.menuOpen;
    }


    public logMeOut(): void {
        localStorage.clear();
        sessionStorage.clear();
        this.userService.logout();
        this.notifyService.success("Have a nice day!");
        this.router.navigate(['/home']);
    }
    
}
