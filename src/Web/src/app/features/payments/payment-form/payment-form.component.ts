import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { PaymentService } from '../../../core/services/payment.service';
import { CurrencyInputComponent } from '../../../shared/components/currency-input/currency-input.component';

@Component({
  selector: 'app-payment-form',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule, 
    RouterModule,
    MatCardModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatButtonModule, 
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatSnackBarModule,
    CurrencyInputComponent
  ],
  template: `
    <div class="container">
      <mat-card class="form-card">
        <mat-card-header>
          <mat-card-title>{{ isEdit ? '支払いを編集' : '支払いを登録' }}</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="paymentForm" (ngSubmit)="onSubmit()">
            <app-currency-input formControlName="amount"></app-currency-input>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>支払い内容</mat-label>
              <input matInput formControlName="description" placeholder="例: 昼食代">
              <mat-error *ngIf="paymentForm.get('description')?.hasError('required')">内容は必須です</mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>カテゴリ</mat-label>
              <mat-select formControlName="categoryId">
                <mat-option *ngFor="let cat of categories" [value]="cat.id">
                  {{ cat.name }}
                </mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>日付</mat-label>
              <input matInput [matDatepicker]="picker" formControlName="paymentDate">
              <mat-datepicker-toggle matIconSuffix [for]="picker"></mat-datepicker-toggle>
              <mat-datepicker #picker></mat-datepicker>
            </mat-form-field>

            <div class="participant-section">
              <h3>精算対象メンバー（全員均等）</h3>
              <p class="hint">立て替えた金額を割り振るメンバーを選択してください</p>
              <div class="participant-list">
                <mat-checkbox *ngFor="let m of members" [checked]="true" disabled>
                  {{ m.name }}
                </mat-checkbox>
              </div>
            </div>

            <div class="actions">
              <button mat-button type="button" [routerLink]="['/trips', tripId]">キャンセル</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="paymentForm.invalid">
                保存する
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .container { display: flex; justify-content: center; padding: 20px; }
    .form-card {
      width: 100%;
      max-width: 500px;
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
      padding: 20px;
    }
    .full-width { width: 100%; margin-bottom: 16px; }
    .participant-section { margin: 24px 0; }
    h3 { margin: 0; font-size: 16px; }
    .hint { font-size: 12px; opacity: 0.6; margin: 4px 0 12px; }
    .participant-list { display: flex; flex-wrap: wrap; gap: 16px; }
    .actions { display: flex; justify-content: flex-end; gap: 12px; margin-top: 24px; }
  `]
})
export class PaymentFormComponent implements OnInit {
  paymentForm: FormGroup;
  tripId: string = '';
  isEdit: boolean = false;

  // 暫定的なマスタデータ
  categories = [
    { id: '1', name: '交通費' },
    { id: '2', name: '食費' },
    { id: '3', name: '宿泊費' },
    { id: '4', name: '娯楽費' },
    { id: '5', name: 'その他' }
  ];

  members = [
    { id: 'a', name: 'あなた' },
    { id: 'b', name: '友人A' },
    { id: 'c', name: '友人B' }
  ];

  constructor(
    private fb: FormBuilder,
    private paymentService: PaymentService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.paymentForm = this.fb.group({
      amount: [0, [Validators.required, Validators.min(1)]],
      description: ['', [Validators.required]],
      categoryId: ['', [Validators.required]],
      paymentDate: [new Date(), [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.tripId = this.route.snapshot.params['id'];
    // 編集モード判定などは今後追加
  }

  onSubmit(): void {
    if (this.paymentForm.valid) {
      const paymentData = {
        ...this.paymentForm.value,
        tripId: this.tripId,
        participantUserIds: this.members.map(m => m.id)
      };
      
      this.paymentService.createPayment(paymentData).subscribe({
        next: () => {
          this.snackBar.open('支払いを登録しました', '閉じる', { duration: 3000 });
          this.router.navigate(['/trips', this.tripId]);
        },
        error: () => {
          this.snackBar.open('登録に失敗しました', '閉じる', { duration: 3000 });
        }
      });
    }
  }
}
