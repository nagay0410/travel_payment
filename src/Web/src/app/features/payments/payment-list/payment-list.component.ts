import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PaymentService, Payment } from '../../../core/services/payment.service';

@Component({
  selector: 'app-payment-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterModule],
  template: `
    <div class="payment-list-container">
      <div class="header">
        <h2>支払い履歴</h2>
        <button mat-flat-button color="accent" [routerLink]="['/trips', tripId, 'payments', 'create']">
          <mat-icon>add</mat-icon> 支払いを追加
        </button>
      </div>

      <div class="table-scroll">
        <table mat-table [dataSource]="paymentService.payments()" class="mat-elevation-z0">
          <ng-container matColumnDef="date">
            <th mat-header-cell *matHeaderCellDef>日付</th>
            <td mat-cell *matCellDef="let p">{{ p.paymentDate | date:'MM/dd' }}</td>
          </ng-container>

          <ng-container matColumnDef="description">
            <th mat-header-cell *matHeaderCellDef>内容</th>
            <td mat-cell *matCellDef="let p">
              <div class="desc-cell">
                <span class="category-tag">{{ p.categoryName }}</span>
                <span>{{ p.description }}</span>
              </div>
            </td>
          </ng-container>

          <ng-container matColumnDef="payer">
            <th mat-header-cell *matHeaderCellDef>支払い者</th>
            <td mat-cell *matCellDef="let p">{{ p.paidByUserName }}</td>
          </ng-container>

          <ng-container matColumnDef="amount">
            <th mat-header-cell *matHeaderCellDef>金額</th>
            <td mat-cell *matCellDef="let p" class="amount-cell">{{ p.amount | currency:'JPY' }}</td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      </div>

      <div *ngIf="paymentService.payments().length === 0" class="empty-state">
        <p>支払い記録がまだありません。</p>
      </div>
    </div>
  `,
  styles: [`
    .payment-list-container {
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      border-radius: 12px;
      padding: 0 0 20px 0;
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
      overflow: hidden;
    }
    .header { padding: 20px; display: flex; justify-content: space-between; align-items: center; }
    h2 { margin: 0; font-size: 20px; color: white; }
    .table-scroll { overflow-x: auto; }
    table { width: 100%; background: transparent !important; }
    th { color: rgba(255, 255, 255, 0.6) !important; font-size: 13px; border-bottom: 1px solid rgba(255, 255, 255, 0.1) !important; text-align: left; padding: 0 16px; }
    td { color: white !important; border-bottom: 1px solid rgba(255, 255, 255, 0.05) !important; padding: 0 16px; }
    .desc-cell { display: flex; flex-direction: column; gap: 4px; padding: 12px 0; }
    .category-tag {
      font-size: 10px;
      background: rgba(255, 255, 255, 0.1);
      padding: 2px 6px;
      border-radius: 4px;
      width: fit-content;
      color: #81c784;
    }
    .amount-cell { font-weight: bold; text-align: right !important; }
    .empty-state { text-align: center; padding: 40px; opacity: 0.5; }
  `]
})
export class PaymentListComponent implements OnInit {
  @Input() tripId: string = '';
  displayedColumns: string[] = ['date', 'description', 'payer', 'amount'];

  constructor(
    public paymentService: PaymentService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    if (!this.tripId) {
      this.tripId = this.route.snapshot.params['id'];
    }
    if (this.tripId) {
      this.paymentService.getPaymentsByTripId(this.tripId).subscribe();
    }
  }
}
