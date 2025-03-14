import { Component } from '@angular/core';
import { DeveloperService } from '../services/developer.service';
import { ActivatedRoute  } from '@angular/router';
import { Developer } from '../model/developer';
import { OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, tap, throwError } from 'rxjs';

@Component({
  imports: [CommonModule],
  templateUrl: './developer-details.component.html',
  styleUrl: './developer-details.component.css'
})
export class DeveloperDetailsComponent implements OnInit {
   
  id!: any;
  dev!: Developer|undefined;
  
  private developerService = inject(DeveloperService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  ngOnInit(): void {   
    this.id = this.route.snapshot.paramMap.get('id');
    this.getDeveloper(this.id);
  }

  getDeveloper(id: string) {    
    this.dev = this.developerService.getDeveloperById(id);
  }

  updateDeveloper(id: string) {
    console.log(id);
    this.router.navigate(['/dev-update', id]);
  }
  

  deleteDeveloper(id: string): void {
    this.developerService.deleteDeveloper(id).pipe(
      tap(() => console.log('Developer deleted successfully')),
      catchError((error) => {
        console.error("Error deleting developer", error);
        return throwError(() => error);
      })
    ).subscribe(
      {
        next: () => this.router.navigate(['/dev']),
        error: (err) => console.error('Navigation failed', err)
      }
    );
  }
  
}
