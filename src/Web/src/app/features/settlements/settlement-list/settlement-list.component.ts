import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { SettlementService, Settlement } from '../../../core/services/settlement.service';

@Component({
  selector: 'app-settlement-list',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatDividerModule, MatSnackBarModule],
  template: `
    <div class="settlement-container">
      <div class="header">
        <h2>精算状況</h2>
        <button mat-raised-button color="primary" (click)="onComplete()" *ngIf="settlementService.calculations().length > 0">
          精算を完了する
        </button>
      </div>

      <div class="settlement-grid">
        <mat-card *ngFor="let s of settlementService.calculations()" class="settlement-card">
          <mat-card-content>
            <div class="settlement-flow">
              <div class="user-from">
                <span class="name">{{ s.fromUserName }}</span>
                <span class="action">支払う</span>
              </div>
              <div class="arrow">
                <mat-icon>trending_flat</mat-icon>
                <span class="amount">{{ s.amount | currency:'JPY' }}</span>
              </div>
              <div class="user-to">
                <span class="name">{{ s.toUserName }}</span>
                <span class="action">受け取る</span>
              </div>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <div *ngIf="settlementService.calculations().length === 0" class="empty-state">
        <mat-icon>check_circle</mat-icon>
        <p>精算が必要な貸し借りは現在ありません。</p>
      </div>
    </div>
  `,
  styles: [`
    .settlement-container {
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      border-radius: 12px;
      padding: 24px;
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
    }
    .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
    h2 { margin: 0; }
    .settlement-grid { display: flex; flex-direction: column; gap: 12px; }
    .settlement-card {
      background: rgba(255, 255, 255, 0.03);
      border: 1px solid rgba(255, 255, 255, 0.05);
      color: white;
    }
    .settlement-flow {
      display: flex;
      align-items: center;
      justify-content: space-around;
      padding: 8px 0;
    }
    .user-from, .user-to { display: flex; flex-direction: column; align-items: center; gap: 4px; }
    .name { font-weight: bold; font-size: 16px; }
    .action { font-size: 11px; opacity: 0.6; }
    .arrow { display: flex; flex-direction: column; align-items: center; color: #81c784; }
    .amount { font-weight: bold; font-size: 18px; margin-top: -4px; }
    .empty-state { text-align: center; padding: 40px; opacity: 0.5; }
    .empty-state mat-icon { font-size: 48px; width: 48px; height: 48px; margin-bottom: 8px; color: #81c784; }
  `]
})
export class SettlementListComponent implements OnInit {
  @Input() tripId: string = '';

  constructor(
    public settlementService: SettlementService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    if (this.tripId) {
      this.settlementService.calculate(this.tripId).subscribe();
    }
  }

  onComplete(): void {
    if (confirm('精算を完了（締め切り）してもよろしいですか？')) {
      this.settlementService.completeSettlement(this.tripId).subscribe({
        next: () => {
          this.snackBar.open('精算を完了しました', '閉じる', { duration: 3000 });
          // 更新処理など
        },
        error: () => {
          this.snackBar.open('エラーが発生しました', '閉じる', { duration: 3000 });
        }
      });
    }
  }
}
