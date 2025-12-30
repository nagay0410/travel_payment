import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-expense-summary',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule],
  template: `
    <div class="summary-grid">
      <mat-card class="summary-card balance">
        <div class="icon-wrap"><mat-icon>account_balance_wallet</mat-icon></div>
        <div class="data">
          <h3>精算予定額</h3>
          <p [class.positive]="balance >= 0" [class.negative]="balance < 0">
            {{ balance | currency:'JPY' }}
          </p>
        </div>
      </mat-card>

      <mat-card class="summary-card spending">
        <div class="icon-wrap spending"><mat-icon>payments</mat-icon></div>
        <div class="data">
          <h3>あなたの支出</h3>
          <p>{{ mySpending | currency:'JPY' }}</p>
        </div>
      </mat-card>
    </div>
  `,
  styles: [`
    .summary-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
      margin-bottom: 24px;
    }
    .summary-card {
      display: flex;
      flex-direction: row;
      align-items: center;
      gap: 16px;
      padding: 20px;
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
    }
    .icon-wrap {
      width: 48px;
      height: 48px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #81c784;
      color: #1b5e20;
    }
    .icon-wrap.spending { background: #64b5f6; color: #0d47a1; }
    h3 { margin: 0; font-size: 14px; opacity: 0.7; }
    p { margin: 4px 0 0; font-size: 24px; font-weight: bold; }
    .negative { color: #ff5252; }
    .positive { color: #81c784; }
  `]
})
export class ExpenseSummaryComponent {
  @Input() tripId: string = '';

  // 暫定的なデータ
  balance: number = 1500;
  mySpending: number = 12500;
}
