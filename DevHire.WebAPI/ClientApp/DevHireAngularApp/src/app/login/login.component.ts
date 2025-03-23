import { Component } from '@angular/core';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { RouterOutlet , RouterLink} from '@angular/router';
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { LoginUser } from '../model/loginUser';
import { AuthService } from '../services/auth.service';
import { tap, catchError, throwError } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
@Component({
  imports: [ RouterOutlet , RouterLink, ReactiveFormsModule],  
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  
  private router = inject(Router);
  private authService = inject(AuthService);
  private formBuilder = inject(FormBuilder);
  loginUser!: LoginUser;


  loginForm = this.formBuilder.group({
    emailControl: ['', Validators.required],
    passwordControl: ['', Validators.required]
  });
  

  onSubmit():void{
    this.loginUser = new LoginUser(this.loginForm.value.emailControl ?? '', this.loginForm.value.passwordControl ?? '')
    this.login(this.loginUser);   
  }

  login(loginUser: LoginUser) {
    this.authService.login(loginUser).pipe(
          tap((response: any) => {
            console.log('Login successfully', response);
            this.authService.currentUserName = response.email;           
            localStorage.setItem('AccessToken', response.jwtToken); 
            localStorage.setItem('RefreshToken', response.refreshToken);         
          }),
          catchError((error) => {
            console.error("Error in Login", error);        
            return throwError(() => new Error(error));
          })
        ).subscribe(
          {
            next: () => this.router.navigate(['/dev']),
            error: (err) => console.error('Navigation failed', err)
          }
        );
      }
  
  cancelLogin(){
    this.router.navigate(['/register']);
  }

}
