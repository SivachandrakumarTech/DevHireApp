import { HttpRequest } from "@angular/common/http";
import { HttpHandlerFn } from "@angular/common/http";
import { Observable } from "rxjs";
import { HttpEvent } from "@angular/common/http";

export function loggingInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {    
    console.log("Logging Interceptor", req.url);
    return next(req);
  }