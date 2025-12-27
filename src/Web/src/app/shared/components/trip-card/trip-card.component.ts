import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-trip-card',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatProgressBarModule],
  templateUrl: './trip-card.component.html',
  styleUrl: './trip-card.component.css'
})
export class TripCardComponent {
  @Input() title: string = '';
  @Input() dateRange: string = '';
  @Input() budget: number = 0;
  @Input() spent: number = 0;
  @Input() status: '計画中' | '実施中' | '完了' = '計画中';

  get progress(): number {
    return this.budget > 0 ? (this.spent / this.budget) * 100 : 0;
  }
}
