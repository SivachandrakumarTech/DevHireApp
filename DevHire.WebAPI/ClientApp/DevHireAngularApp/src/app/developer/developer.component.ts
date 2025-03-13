import { Component} from '@angular/core';
import { Developer } from '../developer';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OnInit } from '@angular/core';
import { inject } from '@angular/core';
import { DeveloperService } from '../developer.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  imports: [CommonModule,RouterLink],
  templateUrl: './developer.component.html',
  styleUrl: './developer.component.css'
})
export class DeveloperComponent implements OnInit {

  developers!:Developer[];
  private developerService = inject(DeveloperService);
  private route = inject(ActivatedRoute);
  
ngOnInit(): void {
  this.developers = this.route.snapshot.data['data'];
  
 //Moved the data fetching to resolver
/*  this.developerService.getAllDevelopers().subscribe(
  (data: Developer[]) => {
    this.developers = data;
  },
  (error: any) => {
    console.error('Error fetching developers in Data Resolver', error);
  }
); */
}
}
