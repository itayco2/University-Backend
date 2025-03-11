import { inject } from '@angular/core';
import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';
import { NotifyService } from './notify.service';
import * as jwt_decode from 'jwt-decode';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state) => {
  const token = localStorage.getItem('token');

  if (token) {
    try {
      const decodedToken: any = jwt_decode(token); 
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

  notifyService.error('You are not logged in.');
  router.navigateByUrl('/login');
  return false;
};
