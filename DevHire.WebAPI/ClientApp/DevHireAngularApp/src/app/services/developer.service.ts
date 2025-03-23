import { Injectable } from '@angular/core';
import { Developer } from '../model/developer';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable ,throwError} from 'rxjs';
import { map , catchError } from 'rxjs/operators';
import { inject } from '@angular/core';


@Injectable({
  providedIn:'root',
})
export class DeveloperService {

  //ASP.NET Web API
  private baseUrl = 'https://localhost:7176/api/v1/developers';
  developers!: Developer[];
  private httpClient = inject(HttpClient);

 //observable.pipe(operator1(), operator2(), operator3())

  getAllDevelopers(): Observable<Developer[]> 
  { 
    /*  // Retrieve token from localStorage
     const token = localStorage.getItem('token');

     // Initialize headers properly
      let headers = new HttpHeaders();

     if (token) {
      headers = headers.set("Authorization", `Bearer ${token}`);
    }
    */
    return this.httpClient.get<Developer[]>(this.baseUrl).pipe
     (
      map(response => {
        this.developers = response; 
        return response;
      }),  
      catchError(this.handleError)
     )
  }

   getDeveloperById(id: string): Developer {     
    const developer = this.developers.find(developer => developer.id === id);
    if (!developer) {
      throw new Error(`Developer with id ${id} not found`);
    }
    return developer;
   }  

   createDeveloper(developer: Developer): Observable<Developer> {
     return this.httpClient.post<Developer>(this.baseUrl, developer);
   }

   updateDeveloper(id: string, developer: Developer): Observable<Developer> {
    return this.httpClient.put<Developer>(`${this.baseUrl}/${id}`, developer);
  }

    deleteDeveloper(id: string): Observable<void>{      
      return this.httpClient.delete<void>(`${this.baseUrl}/${id}`);
   }

    handleError(error: HttpErrorResponse) {
    return throwError(() => new Error(error.message));
   }


}
