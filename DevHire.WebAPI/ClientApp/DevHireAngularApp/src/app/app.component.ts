import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterOutlet , RouterLink} from '@angular/router';
import { AuthService } from './services/auth.service';
import { inject } from '@angular/core';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import {tap, catchError, throwError} from 'rxjs';


@Component({
  selector: 'app-root',
  imports: [ RouterLink, RouterOutlet, FormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'DevHire App';
   
  public authService = inject(AuthService);
  private router = inject(Router);

  constructor(){   
  }

  ngOnInit(): void {
   
  }
  
   logout(){       
    this.authService.logout().pipe(
      tap(() => {
        console.log('Logged out successfully');
        this.authService.currentUserName = '';
        localStorage.removeItem("AccessToken"); // Removing the Access Token once user logout
        localStorage.removeItem("RefreshToken"); // Removing the Refresh Token once user logout   
      }),
      catchError((error) => {
        console.error("Error in Login", error);        
        return throwError(() => new Error(error));
      })
    ).subscribe(
      {
        next: () => this.router.navigate(['/login']),
        error: (err) => console.error('Navigation failed', err)
      }
    );
   }              
}
