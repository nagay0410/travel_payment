import { Routes } from '@angular/router';
import { TripListComponent } from './features/trips/trip-list/trip-list.component';
import { LoginComponent } from './features/auth/login/login.component';

export const routes: Routes = [
  { path: 'trips', component: TripListComponent },
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/trips', pathMatch: 'full' }
];
