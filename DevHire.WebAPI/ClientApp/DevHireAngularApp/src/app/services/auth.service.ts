import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { RegisterUser } from '../model/registerUser';
import { Tokens } from '../model/tokens';
import { Observable, tap } from 'rxjs';
import { LoginUser } from '../model/loginUser';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://localhost:7176/api/v1/auth';
  public currentUserName!: string;
  token!: Tokens;

  private httpClient = inject(HttpClient);

  constructor() { }

   register(registerUser: RegisterUser): Observable<any> {    
       return this.httpClient.post<any>(this.baseUrl + '/register', registerUser);
     }

     
  login(login: LoginUser): Observable<any> {
    return this.httpClient.post<any>(this.baseUrl + '/login', login);
  }
  
  logout(){
    return this.httpClient.get(this.baseUrl + '/logout');
  }

  refreshToken() {

    var accessToken = localStorage.getItem('AccessToken'); 
    var refreshToken = localStorage.getItem('RefreshToken'); 

   this.token = new Tokens(accessToken ?? '', refreshToken ?? '')

    return this.httpClient.post<any>(this.baseUrl + '/refresh', this.token).pipe(
      tap((response: any) => {
        localStorage.setItem('AccessToken', response.jwtToken);
        localStorage.setItem('RefreshToken', response.refreshToken);
      })
    );
  }
}
