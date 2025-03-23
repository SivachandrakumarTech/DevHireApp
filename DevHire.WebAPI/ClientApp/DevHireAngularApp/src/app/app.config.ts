import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { withPreloading } from '@angular/router';
import { PreloadAllModules } from '@angular/router';
import { NoPreloading } from '@angular/router';
import { FlagBasedPreloadingStrategyService } from './services/flag-based-preloading-strategy.service';
import { loggingInterceptor } from './interceptors/loggingInterceptor';
import { authInterceptor } from './interceptors/authInterceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes, withPreloading(FlagBasedPreloadingStrategyService)), 
    provideClientHydration(withEventReplay()),
    provideHttpClient(withInterceptors([authInterceptor, loggingInterceptor]))  
   ]
};
