import { CanActivateFn } from '@angular/router';
import { AuthService } from './services/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

let isAuthenticated: boolean = false;

export const routeGuard: CanActivateFn = (route, state) => {

  const  authService = inject(AuthService);
  const  router = inject(Router);

  authService.isAuthenticated().subscribe((response: boolean) => {      
    isAuthenticated = response;
  });

  if(!isAuthenticated){
    router.navigate(['login']);
  }
    return isAuthenticated;     
}
