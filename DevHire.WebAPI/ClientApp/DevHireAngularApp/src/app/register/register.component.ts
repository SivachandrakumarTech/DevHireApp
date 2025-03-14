import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { inject } from '@angular/core';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule], 
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {


   private formBuilder = inject(FormBuilder);
   private router = inject(Router);

   
   registerForm = this.formBuilder.group({
    firstNameControl:[''],
    lastNameControl: [''],
    userNameControl: [''],
    emailControl: [''],
    passwordControl: ['']
  })


  onSubmit() {

  }

  resetRegister() {

  }
}
