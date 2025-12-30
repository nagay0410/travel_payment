import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TripService, Trip } from '../../../core/services/trip.service';
import { TripCardComponent } from '../../../shared/components/trip-card/trip-card.component';

@Component({
  selector: 'app-trip-list',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButtonModule, MatIconModule, TripCardComponent],
  template: `
    <div class="header">
      <h1>旅行一覧</h1>
      <button mat-raised-button color="primary" routerLink="/trips/create">
        <mat-icon>add</mat-icon> 新しい旅行を作成
      </button>
    </div>

    <div class="trip-grid" *ngIf="tripService.trips().length > 0; else emptyState">
      <app-trip-card
        *ngFor="let trip of tripService.trips()"
        [title]="trip.name"
        [dateRange]="(trip.startDate | date:'yyyy/MM/dd') + ' - ' + (trip.endDate | date:'yyyy/MM/dd')"
        [budget]="trip.budget"
        [spent]="trip.totalSpent"
        [status]="trip.status"
        [routerLink]="['/trips', trip.id]"
      ></app-trip-card>
    </div>

    <ng-template #emptyState>
      <div class="empty-state">
        <mat-icon class="large-icon">flight</mat-icon>
        <p>まだ参加している旅行がありません。</p>
      </div>
    </ng-template>
  `,
  styles: [`
    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 24px;
    }
    .trip-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 20px;
    }
    .empty-state {
      text-align: center;
      padding: 60px;
      color: rgba(255, 255, 255, 0.5);
    }
    .large-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      margin-bottom: 16px;
    }
    h1 { margin: 0; color: white; }
  `]
})
export class TripListComponent implements OnInit {
  constructor(public tripService: TripService) {}

  ngOnInit(): void {
    this.tripService.getTrips().subscribe();
  }
}
