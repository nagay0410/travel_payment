import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

export interface SettlementConfirmData {
  fromUserName: string;
  toUserName: string;
  amount: number;
}

@Component({
  selector: 'app-confirm-settlement-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule
  ],
  template: `
    <div class="dialog-container glass-morphism">
      <h2 mat-dialog-title>
        <mat-icon color="primary">handshake</mat-icon>
        精算内容の確認
      </h2>
      
      <mat-dialog-content>
        <div class="settlement-info">
          <div class="info-item">
            <span class="label">支払う人</span>
            <span class="value">{{ data.fromUserName }}</span>
          </div>
          
          <div class="arrow">
            <mat-icon>arrow_downward</mat-icon>
            <div class="amount-badge">{{ data.amount | currency:'JPY':'symbol':'1.0-0' }}</div>
          </div>
          
          <div class="info-item">
            <span class="label">受け取る人</span>
            <span class="value">{{ data.toUserName }}</span>
          </div>
        </div>

        <mat-divider></mat-divider>

        <p class="warning-text">
          この操作を完了すると、精算済みとして記録されます。<br>
          実際に支払いが完了した後に実行してください。
        </p>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="onCancel()">キャンセル</button>
        <button mat-raised-button color="primary" (click)="onConfirm()">精算を完了する</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-container {
      padding: 8px;
      color: white;
    }
    h2 {
      display: flex;
      align-items: center;
      gap: 12px;
      font-size: 20px;
    }
    .settlement-info {
      padding: 24px 0;
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 16px;
    }
    .info-item {
      text-align: center;
    }
    .label {
      display: block;
      font-size: 12px;
      opacity: 0.6;
      margin-bottom: 4px;
    }
    .value {
      font-size: 18px;
      font-weight: 600;
    }
    .arrow {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 4px;
      color: #9c88ff;
    }
    .amount-badge {
      background: rgba(156, 136, 255, 0.2);
      padding: 4px 16px;
      border-radius: 20px;
      border: 1px solid rgba(156, 136, 255, 0.3);
      font-size: 20px;
      font-weight: 700;
    }
    mat-divider { margin: 16px 0; border-color: rgba(255, 255, 255, 0.1); }
    .warning-text {
      font-size: 13px;
      opacity: 0.8;
      text-align: center;
      line-height: 1.6;
    }
    mat-dialog-actions {
      padding: 16px 0 8px;
    }
  `]
})
export class ConfirmSettlementDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmSettlementDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SettlementConfirmData
  ) {}

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}
