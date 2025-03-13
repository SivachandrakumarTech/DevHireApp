import { Component, OnInit, OnChanges, SimpleChanges  } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Developer } from '../developer';
import { inject } from '@angular/core';
import { DeveloperService } from '../developer.service';
import { FormGroup } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { catchError, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-developer-update',
  imports: [ReactiveFormsModule],
  templateUrl: './developer-update.component.html',
  styleUrl: './developer-update.component.css'
})
export class DeveloperUpdateComponent implements OnInit, OnChanges {

  id!: any;
  dev!: Developer;

  private route = inject(ActivatedRoute);
  private developerService = inject(DeveloperService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);

  developerForm = this.formBuilder.group({
    firstNameControl:[''],
    lastNameControl: [''],
    yearsOfExperienceControl: [0],
    favoriteLanguageControl: ['']
  })


  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getDeveloper(this.id);    
  }

    // Update form when `dev` changes
    ngOnChanges(changes: SimpleChanges) {
      if (changes['dev'] && this.dev) {
        this.developerForm.patchValue({
          firstNameControl: this.dev.firstName,
          lastNameControl: this.dev.lastName,
          yearsOfExperienceControl: this.dev.yearsOfExperience,
          favoriteLanguageControl: this.dev.favoriteLanguage
        });
      }
    }

  getDeveloper(id: string) {    
    this.dev = this.developerService.getDeveloperById(id);
    this.developerForm.patchValue({
      firstNameControl: this.dev.firstName,
      lastNameControl: this.dev.lastName,
      yearsOfExperienceControl: this.dev.yearsOfExperience,
      favoriteLanguageControl: this.dev.favoriteLanguage
    });
  }

  onSubmit() {
    if (this.developerForm.valid) {
         const updatedDeveloper: Developer = {
          id: this.id,  // Assuming `id` is available
          firstName: this.developerForm.value.firstNameControl || '',
          lastName: this.developerForm.value.lastNameControl || '',
          yearsOfExperience: this.developerForm.value.yearsOfExperienceControl || 0,
          favoriteLanguage: this.developerForm.value.favoriteLanguageControl || ''
        };
      console.log('Updated Developer Data:', this.developerForm.value);
      this.updateDeveloper(this.id, updatedDeveloper);
    }
  }

  updateDeveloper(id: string, developer: Developer): void {
      this.developerService.updateDeveloper(id, developer).pipe(
        tap((data: Developer) => console.log('Developer updated successfully', data)),
        catchError((error) => {
          console.error("Error updating developer", error);
          return throwError(() => error);
        })
      ).subscribe(
        {
          next: () => this.router.navigate(['/dev']),
          error: (err) => console.error('Navigation failed', err)
        }
      );
    }
 
  resetForm() {
    if (this.dev) {
      this.developerForm.reset({
        firstNameControl: this.dev.firstName,
        lastNameControl: this.dev.lastName,
        yearsOfExperienceControl: this.dev.yearsOfExperience,
        favoriteLanguageControl: this.dev.favoriteLanguage
      });
    }
  }

}
