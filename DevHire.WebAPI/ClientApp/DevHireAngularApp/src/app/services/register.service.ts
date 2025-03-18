import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Register } from '../model/register';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  private baseUrl = 'https://localhost:7176/api/account';

  private httpClient = inject(HttpClient);

  constructor() { }

   registration(register: Register): Observable<Register> {
       return this.httpClient.post<Register>(this.baseUrl, register);
     }
}
