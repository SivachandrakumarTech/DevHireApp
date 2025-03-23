import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { RegisterUser } from '../model/registerUser';
import { AuthService } from '../services/auth.service';
import { catchError, tap, throwError } from 'rxjs';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ RouterOutlet , RouterLink, ReactiveFormsModule],  
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

   registerUser!: RegisterUser;   
   private authService = inject(AuthService);
   private formBuilder = inject(FormBuilder);
   private router = inject(Router);

   //Declare and Initialize the Form Group
   registerForm = this.formBuilder.group({
    firstNameControl:['', Validators.required],
    lastNameControl: ['', Validators.required],   
    emailControl: ['', Validators.required],
    passwordControl: ['', Validators.required],
    confirmPasswordControl: ['', Validators.required],
    userTypeControl: ['', Validators.required]
  })

  onSubmit() {
    this.registerUser = new RegisterUser (
      this.registerForm.value.firstNameControl ?? '',
      this.registerForm.value.lastNameControl ?? '',    
      this.registerForm.value.emailControl ?? '',
      this.registerForm.value.passwordControl ?? '',
      this.registerForm.value.confirmPasswordControl ?? '',
      this.registerForm.value.userTypeControl ?? '');
      
      this.registration(this.registerUser);
  }

  resetRegister() {
  }

  cancelRegister(){
    this.router.navigate(['/login']);
  }

   registration(registerUser: RegisterUser): void {
      this.authService.register(registerUser).pipe(
        tap((response: any) => {
          console.log('User saved successfully', response)
          this.authService.currentUserName = response.email;           
          localStorage.setItem('AccessToken', response.jwtToken);   
          localStorage.setItem('RefreshToken', response.refreshToken); 
        }),
        catchError((error) => {
          console.error("Error saving User", error);        
          return throwError(() => new Error(error));
        })
      ).subscribe(
        {
          next: () => this.router.navigate(['/dev']),
          error: (err) => console.error('Navigation failed', err)
        }
      );
    }
}
