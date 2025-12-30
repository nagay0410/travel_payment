import { Routes } from '@angular/router';
import { TripListComponent } from './features/trips/trip-list/trip-list.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { TripCreateComponent } from './features/trips/trip-create/trip-create.component';
import { TripDashboardComponent } from './features/trips/trip-dashboard/trip-dashboard.component';

export const routes: Routes = [
  { path: 'trips', component: TripListComponent },
  { path: 'trips/create', component: TripCreateComponent },
  { path: 'trips/:id', component: TripDashboardComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: '/trips', pathMatch: 'full' }
];
