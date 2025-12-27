import { Component, input, computed } from '@angular/core';
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
  title = input<string>('');
  dateRange = input<string>('');
  budget = input<number>(0);
  spent = input<number>(0);
  status = input<'計画中' | '実施中' | '完了'>('計画中');

  progress = computed(() => {
    const b = this.budget();
    return b > 0 ? (this.spent() / b) * 100 : 0;
  });
}
