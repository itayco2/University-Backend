import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() {}

  // Get user roles from the JWT token
  getUserRoles(): string[] {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decodedToken = JSON.parse(atob(token.split('.')[1]));
        const roles = decodedToken?.user?.role;
        return roles ? [roles] : []; 
      } catch (error) {
        console.error('Error decoding token:', error);
        return []; 
      }
    }
    return [];
  }
  

  // Check if the user has any of the required roles
  hasRole(requiredRoles: string[]): boolean {
    const roles = this.getUserRoles();
    console.log('User roles:', roles);
    return requiredRoles.some(role => roles.includes(role));
  }
}
