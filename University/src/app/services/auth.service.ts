import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private cachedRoles: string[] | null = null;

  constructor() {}

  getUserRoles(): string[] {
    if (this.cachedRoles !== null) {
      return this.cachedRoles;
    }

    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decodedToken = JSON.parse(atob(token.split('.')[1]));
        const roles = decodedToken?.user?.role;
        this.cachedRoles = roles ? [roles] : [];
        return this.cachedRoles;
      } catch (error) {
        console.error('Error decoding token:', error);
        this.cachedRoles = [];
        return this.cachedRoles;
      }
    }
    this.cachedRoles = [];
    return this.cachedRoles;
  }

  hasRole(requiredRoles: string[]): boolean {
    const roles = this.getUserRoles();

    return requiredRoles.some(role => roles.includes(role));
  }

  clearRolesCache(): void {
    this.cachedRoles = null;
  }

  logout(): void {
    localStorage.removeItem('token');
    this.clearRolesCache();
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
    this.clearRolesCache();
  }
}