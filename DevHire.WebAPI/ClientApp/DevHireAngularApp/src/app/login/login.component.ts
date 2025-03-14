import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { RouterOutlet , RouterLink} from '@angular/router';

@Component({
  imports: [ RouterOutlet , RouterLink],
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
