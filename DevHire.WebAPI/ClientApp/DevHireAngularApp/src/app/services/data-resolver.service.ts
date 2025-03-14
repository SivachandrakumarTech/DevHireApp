import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { inject } from '@angular/core';
import { DeveloperService } from './developer.service';
import { catchError, Observable, of, take, retry } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class DataResolverService implements Resolve<any>{

  private developerService = inject(DeveloperService);

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any>{
    return this.developerService.getAllDevelopers().pipe(
    retry(3),
    catchError(error => { console.error('Error fetching developers in Data Resolver');
      return of(null);
  })
    );
  }
}

