import { Component } from '@angular/core';
import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { DeveloperComponent } from './developer/developer.component';
import { DeveloperDetailsComponent } from './developer-details/developer-details.component';
import { DeveloperCreateComponent } from './developer-create/developer-create.component';
import { LoginComponent } from './login/login.component';
import { AuthService } from './services/auth.service';
import { inject } from '@angular/core';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { DataResolverService } from './services/data-resolver.service';
import { RegisterComponent } from './register/register.component';

export const routes: Routes = [
  {
    path: "login",
    title: "Login",
    component: LoginComponent
  },
  {
    path: "register",
    title: "Register",
    component: RegisterComponent
  },
    {
     path:'dev', 
     title: "Developer",
     loadComponent: () => import('./developer/developer.component').then(m => m.DeveloperComponent),
     resolve: {data: DataResolverService} 
    },
    {
     path: 'dev/:id',
     title: "Developer Details",
     loadComponent: () => import('./developer-details/developer-details.component').then(m => m.DeveloperDetailsComponent)
   // canActivate:[routeGuard]
    },
    {
    path:"dev-create", 
    title: "Create Developer", 
    loadComponent:()=> import('./developer-create/developer-create.component').then(m => m.DeveloperCreateComponent), 
    data: { preload: true }// canActivate:[routeGuard]
   },
   {
     path: "dev-update/:id",
    title: "Update Developer",
    loadComponent: () => import('./developer-update/developer-update.component').then(m => m.DeveloperUpdateComponent)  
   },
    {
    path:'', 
    redirectTo:'login', 
    pathMatch:'full'
    },
    {
     path:'**', 
     component:PageNotFoundComponent
    }
]
