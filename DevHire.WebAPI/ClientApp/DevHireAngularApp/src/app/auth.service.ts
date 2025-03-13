import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authResponse = new BehaviorSubject(false);
  private authenticated = this.authResponse.asObservable();
  private router = inject(Router);

  constructor() { }

  login():void {
    this.authResponse.next(true);  
  }

  logout():void{
    this.authResponse.next(false);    
  }

  isAuthenticated():Observable<boolean>{
    return this.authenticated;
  }
}
