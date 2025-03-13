import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterOutlet , RouterLink} from '@angular/router';
import { AuthService } from './auth.service';
import { inject } from '@angular/core';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-root',
  imports: [ RouterLink, RouterOutlet, FormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'DevHire App';
   
  private authService = inject(AuthService);
  private router = inject(Router);

  isAuthenticated!: boolean;

  constructor(){   
  }

  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((response: boolean) => {      
      this.isAuthenticated = response;
    });
  }
  
   logout(){   
      this.authService.logout();   
      this.router.navigate(['']);
   }          
}
