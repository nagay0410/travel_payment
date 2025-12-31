import { Routes } from '@angular/router';
import { TripListComponent } from './features/trips/trip-list/trip-list.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { TripCreateComponent } from './features/trips/trip-create/trip-create.component';
import { TripDashboardComponent } from './features/trips/trip-dashboard/trip-dashboard.component';
import { PaymentFormComponent } from './features/payments/payment-form/payment-form.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'trips', component: TripListComponent, canActivate: [authGuard] },
  { path: 'trips/create', component: TripCreateComponent, canActivate: [authGuard] },
  { path: 'trips/:id', component: TripDashboardComponent, canActivate: [authGuard] },
  { path: 'trips/:id/payments/create', component: PaymentFormComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
