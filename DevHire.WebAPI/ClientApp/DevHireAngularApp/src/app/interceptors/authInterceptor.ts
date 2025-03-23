import { HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError, switchMap, catchError } from "rxjs";
import { inject } from "@angular/core";
import { AuthService } from "../services/auth.service"; // Import AuthService

export function authInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
    console.log("Authentication Interceptor", request);

    const authService = inject(AuthService); // Inject AuthService

    // Skip adding the Authorization header for login and register requests
    if (request.url.includes('/login') || request.url.includes('/register') || request.url.includes('/refresh') || request.url.includes('/logout')) {
        return next(request);
    }

    // Retrieve Access Token from localStorage
    let accessToken = localStorage.getItem('AccessToken');

    // If token exists, attach it to the request
    let clonedRequest = request;
    if (accessToken) {
        clonedRequest = request.clone({
            setHeaders: { Authorization: `Bearer ${accessToken}` }
        });
    }

    // Handle token expiration and refresh logic
    return next(clonedRequest).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401 && error.error?.error === "TokenExpired") {
                console.warn("Access token expired. Attempting refresh...");

                return authService.refreshToken().pipe(
                    switchMap(response => {
                        // Update stored token
                        localStorage.setItem('AccessToken', response.jwtToken);
                        localStorage.setItem('RefreshToken', response.refreshToken);

                        // Retry the failed request with the new access token
                        const newRequest = request.clone({
                            setHeaders: { Authorization: `Bearer ${response.jwtToken}` }
                        });
                        return next(newRequest);
                    }),
                    catchError(refreshError => {
                        console.error("Refresh token invalid, logging out...", refreshError);
                        authService.logout(); // Logout user if refresh fails
                         return throwError(() => refreshError);
                    })
                );
            }

            return throwError(() => error); // Pass other errors through
        })
    );
}