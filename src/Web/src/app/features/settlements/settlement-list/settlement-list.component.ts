import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { SettlementService, Settlement } from '../../../core/services/settlement.service';
import { ConfirmSettlementDialogComponent } from '../confirm-settlement-dialog/confirm-settlement-dialog.component';

@Component({
  selector: 'app-settlement-list',
  standalone: true,
  imports: [
    CommonModule, 
    MatCardModule, 
    MatButtonModule, 
    MatIconModule, 
    MatDividerModule, 
    MatSnackBarModule,
    MatDialogModule
  ],
  template: `
    <div class="settlement-container">
      <div class="header">
        <div class="title-with-icon">
          <mat-icon>calculate</mat-icon>
          <h2>精算状況</h2>
        </div>
        <button mat-raised-button color="primary" (click)="onComplete()" *ngIf="settlementService.calculations().length > 0" class="complete-btn">
          <mat-icon>check_circle</mat-icon>
          <span>精算を完了する</span>
        </button>
      </div>

      <div class="settlement-grid">
        <mat-card *ngFor="let s of settlementService.calculations()" class="settlement-card glass-morphism">
          <mat-card-content>
            <div class="settlement-flow">
              <div class="user-item">
                <div class="avatar-placeholder">{{ s.fromUserName.charAt(0) }}</div>
                <div class="user-info">
                  <span class="name">{{ s.fromUserName }}</span>
                  <span class="action">支払う</span>
                </div>
              </div>
              
              <div class="connection">
                <div class="amount-badge">{{ s.amount | currency:'JPY':'symbol':'1.0-0' }}</div>
                <div class="line">
                  <mat-icon>arrow_forward</mat-icon>
                </div>
              </div>
              
              <div class="user-item">
                <div class="avatar-placeholder secondary">{{ s.toUserName.charAt(0) }}</div>
                <div class="user-info">
                  <span class="name">{{ s.toUserName }}</span>
                  <span class="action">受け取る</span>
                </div>
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
      backdrop-filter: blur(20px);
      border-radius: 20px;
      padding: 32px;
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
    }
    .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
    .title-with-icon { display: flex; align-items: center; gap: 12px; }
    .title-with-icon mat-icon { color: #9c88ff; }
    h2 { margin: 0; font-size: 20px; font-weight: 600; }
    
    .complete-btn {
      height: 48px;
      border-radius: 12px;
      padding: 0 24px;
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .settlement-grid { display: flex; flex-direction: column; gap: 16px; }
    .settlement-card {
      background: rgba(255, 255, 255, 0.05);
      border: 1px solid rgba(255, 255, 255, 0.1);
      border-radius: 16px;
      color: white;
    }
    .settlement-flow {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 8px;
    }
    .user-item { display: flex; align-items: center; gap: 16px; flex: 1; }
    .avatar-placeholder {
      width: 40px;
      height: 40px;
      background: #9c88ff;
      border-radius: 12px;
      display: flex;
      justify-content: center;
      align-items: center;
      font-weight: bold;
    }
    .avatar-placeholder.secondary { background: #4834d4; }
    .user-info { display: flex; flex-direction: column; }
    .name { font-weight: bold; font-size: 15px; }
    .action { font-size: 11px; opacity: 0.6; }
    
    .connection {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 8px;
      flex: 1.5;
    }
    .amount-badge {
      background: rgba(156, 136, 255, 0.15);
      color: white;
      padding: 4px 16px;
      border-radius: 20px;
      font-weight: 700;
      font-size: 16px;
      border: 1px solid rgba(156, 136, 255, 0.2);
    }
    .line {
      width: 100%;
      height: 2px;
      background: linear-gradient(90deg, transparent, rgba(156, 136, 255, 0.5), transparent);
      display: flex;
      justify-content: center;
      align-items: center;
    }
    .line mat-icon { font-size: 16px; width: 16px; height: 16px; color: #9c88ff; }

    .empty-state { text-align: center; padding: 60px 40px; opacity: 0.4; }
    .empty-state mat-icon { font-size: 64px; width: 64px; height: 64px; margin-bottom: 16px; color: #81c784; }
  `]
})
export class SettlementListComponent implements OnInit {
  @Input() tripId: string = '';

  constructor(
    public settlementService: SettlementService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    if (this.tripId) {
      this.settlementService.calculate(this.tripId).subscribe();
    }
  }

  onComplete(): void {
    const calculations = this.settlementService.calculations();
    if (calculations.length === 0) return;

    // 代表として1件目のダイアログを表示（本来は一覧全体の確認ダイアログが望ましいが、要件に従いリッチ化）
    // 合計金額などを表示する専用ダイアログへの拡張も検討可能
    const first = calculations[0];
    
    const dialogRef = this.dialog.open(ConfirmSettlementDialogComponent, {
      width: '400px',
      data: {
        fromUserName: first.fromUserName,
        toUserName: first.toUserName,
        amount: first.amount
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.settlementService.completeSettlement(this.tripId).subscribe({
          next: () => {
            this.snackBar.open('精算を完了しました', '閉じる', { duration: 3000 });
            this.settlementService.calculate(this.tripId).subscribe();
          },
          error: () => {
            this.snackBar.open('エラーが発生しました', '閉じる', { duration: 3000 });
          }
        });
      }
    });
  }
}
