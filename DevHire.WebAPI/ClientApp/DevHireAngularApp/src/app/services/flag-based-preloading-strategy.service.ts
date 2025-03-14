import { Injectable } from '@angular/core';
import { PreloadingStrategy } from '@angular/router';
import { Route} from '@angular/router';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FlagBasedPreloadingStrategyService extends PreloadingStrategy {
  preload(route: Route, load: () => Observable<any>): Observable<any> {
     // Preload only if route has a 'data.preload' property set to true
    return  route.data && route.data?.["preload"] === true ? load() : of(null);
  }
}