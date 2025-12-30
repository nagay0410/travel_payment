import { Routes } from '@angular/router';
import { TripListComponent } from './features/trips/trip-list/trip-list.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';

export const routes: Routes = [
  { path: 'trips', component: TripListComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: '/trips', pathMatch: 'full' }
];
