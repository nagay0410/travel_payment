import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <span class="badge" [ngClass]="status()">
      {{ status() }}
    </span>
  `,
  styles: [`
    .badge {
      padding: 2px 10px;
      border-radius: 12px;
      font-size: 11px;
      font-weight: 600;
      text-transform: uppercase;
    }
    .Pending { background: #ffd54f; color: #6d4c41; }
    .Completed { background: #81c784; color: #1b5e20; }
    .計画中 { background: #64b5f6; color: #0d47a1; }
    .実施中 { background: #81c784; color: #1b5e20; }
    .完了 { background: #e0e0e0; color: #424242; }
  `]
})
export class StatusBadgeComponent {
  status = input<string>('');
}
