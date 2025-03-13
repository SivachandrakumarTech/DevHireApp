import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  private authService = inject(AuthService);
  private router = inject(Router);
  

  login():void{
    this.authService.login();
    this.router.navigate(['']);
  }

}
