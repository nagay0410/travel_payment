import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { Trip } from '../../../../../core/services/trip.service';

@Component({
  selector: 'app-trip-header',
  standalone: true,
  imports: [CommonModule, MatProgressBarModule],
  template: `
    <div class="trip-header">
      <div class="title-section">
        <h1>{{ trip?.name }}</h1>
        <div class="date-range">{{ trip?.startDate | date:'yyyy/MM/dd' }} - {{ trip?.endDate | date:'yyyy/MM/dd' }}</div>
      </div>
      
      <div class="budget-section" *ngIf="trip?.budget">
        <div class="budget-info">
          <span>予算進捗 ({{ (trip?.totalSpent || 0) | currency:'JPY' }} / {{ trip?.budget | currency:'JPY' }})</span>
          <span>{{ progress | number:'1.0-0' }}%</span>
        </div>
        <mat-progress-bar mode="determinate" [value]="progress" 
          [color]="progress > 90 ? 'warn' : 'primary'">
        </mat-progress-bar>
      </div>
    </div>
  `,
  styles: [`
    .trip-header {
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      padding: 24px;
      border-radius: 12px;
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
    }
    h1 { margin: 0; font-size: 28px; }
    .date-range { opacity: 0.7; margin-top: 4px; }
    .budget-section { margin-top: 20px; }
    .budget-info { display: flex; justify-content: space-between; margin-bottom: 8px; font-size: 14px; }
    mat-progress-bar { height: 10px; border-radius: 5px; }
  `]
})
export class TripHeaderComponent {
  @Input() trip: Trip | null = null;

  get progress(): number {
    if (!this.trip || !this.trip.budget) return 0;
    return (this.trip.totalSpent / this.trip.budget) * 100;
  }
}
