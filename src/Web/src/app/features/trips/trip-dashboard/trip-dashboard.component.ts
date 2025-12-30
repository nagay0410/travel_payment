import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { TripService } from '../../../core/services/trip.service';
import { TripHeaderComponent } from './components/trip-header/trip-header.component';
import { MemberSidebarComponent } from './components/member-sidebar/member-sidebar.component';
import { ExpenseSummaryComponent } from './components/expense-summary/expense-summary.component';
import { PaymentListComponent } from '../../payments/payment-list/payment-list.component';
import { SettlementListComponent } from '../../settlements/settlement-list/settlement-list.component';

@Component({
  selector: 'app-trip-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    TripHeaderComponent,
    MemberSidebarComponent,
    ExpenseSummaryComponent,
    PaymentListComponent,
    SettlementListComponent
  ],
  template: `
    <div class="dashboard-container" *ngIf="tripService.currentTrip() as trip">
      <app-trip-header [trip]="trip"></app-trip-header>
      
      <div class="dashboard-content">
        <div class="main-column">
          <app-expense-summary [tripId]="trip.id"></app-expense-summary>
          <app-settlement-list [tripId]="trip.id"></app-settlement-list>
          <app-payment-list [tripId]="trip.id"></app-payment-list>
        </div>
        
        <aside class="side-column">
          <app-member-sidebar [tripId]="trip.id"></app-member-sidebar>
        </aside>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-container {
      display: flex;
      flex-direction: column;
      gap: 24px;
    }
    .dashboard-content {
      display: grid;
      grid-template-columns: 1fr 300px;
      gap: 24px;
    }
    @media (max-width: 768px) {
      .dashboard-content {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class TripDashboardComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    public tripService: TripService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.tripService.getTripById(id).subscribe();
      }
    });
  }
}
