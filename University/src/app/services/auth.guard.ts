import { inject } from '@angular/core';
import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';
import { NotifyService } from './notify.service';
import { jwtDecode } from 'jwt-decode';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state) => {
  const token = localStorage.getItem('token');

  if (token) {
    try {
      const decodedToken: any = jwtDecode(token); 
      const userRole = decodedToken.role; 

      const requiredRoles = route.data['roles']; 

      if (requiredRoles && !requiredRoles.includes(userRole)) {
        const notifyService = inject(NotifyService);
        const router = inject(Router);

        notifyService.error('You do not have permission to access this page.');
        router.navigateByUrl('/home');

        return false; 
      }

      return true;
    } catch (error) {
      const notifyService = inject(NotifyService);
      const router = inject(Router);

      notifyService.error('Error decoding the token');
      router.navigateByUrl('/login');
      return false; 
    }
  }

  const notifyService = inject(NotifyService);
  const router = inject(Router);

  notifyService.error('You must be logged in to watch courses.');
  router.navigateByUrl('/login');
  return false;
};
