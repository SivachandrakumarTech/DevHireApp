import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { DeveloperService } from '../developer.service';
import { inject } from '@angular/core';
import { Developer } from '../developer';
import { Router } from '@angular/router';
import { catchError, tap, throwError } from 'rxjs';

@Component({
  imports: [ReactiveFormsModule],
  templateUrl: './developer-create.component.html',
  styleUrl: './developer-create.component.css'
})
export class DeveloperCreateComponent {

    developer!: Developer;

    private developerService = inject(DeveloperService);
    private formBuilder = inject(FormBuilder);
    private router = inject(Router);

    developerForm = this.formBuilder.group({
    firstNameControl:[''],
    lastNameControl: [''],
    yearsOfExperienceControl: [''],
    favoriteLanguageControl: ['']
   
    
  })

  onSubmit(){
    this.developer = new Developer("", this.developerForm.value.firstNameControl ?? '', this.developerForm.value.lastNameControl ?? '', Number(this.developerForm.value.yearsOfExperienceControl) ?? 0, 
      this.developerForm.value.favoriteLanguageControl ?? '');

    this.createDeveloper(this.developer);
  }

  createDeveloper(developer: Developer): void {
    this.developerService.createDeveloper(developer).pipe(
      tap((data: Developer) => console.log('Developer saved successfully', data)),
      catchError((error) => {
        console.error("Error saving developer", error);
        return throwError(() => error);
      })
    ).subscribe(
      {
        next: () => this.router.navigate(['/dev']),
        error: (err) => console.error('Navigation failed', err)
      }
    );
  }

  resetDeveloper() {
    this.developer.firstName = '';
    this.developer.lastName = '';
    this.developer.favoriteLanguage = '';
  }
}
