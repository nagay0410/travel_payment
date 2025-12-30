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
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { trigger, transition, style, animate } from '@angular/animations';
import { PaymentService } from '../../../core/services/payment.service';
import { CurrencyInputComponent } from '../../../shared/components/currency-input/currency-input.component';
import { ImageUploadComponent } from '../../../shared/components/image-upload/image-upload.component';

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
    MatIconModule,
    MatSnackBarModule,
    CurrencyInputComponent,
    ImageUploadComponent
  ],
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(10px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ],
  template: `
    <div class="container" [@fadeIn]>
      <mat-card class="form-card glass-morphism">
        <mat-card-header>
          <mat-card-title>
            <mat-icon>{{ isEdit ? 'edit' : 'add_circle' }}</mat-icon>
            {{ isEdit ? '支払いを編集' : '支払いを登録' }}
          </mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="paymentForm" (ngSubmit)="onSubmit()">
            
            <div class="form-section">
              <app-currency-input formControlName="amount"></app-currency-input>
            </div>

            <div class="form-section">
              <mat-label class="section-label">レシート・領収書</mat-label>
              <app-image-upload (imageSelected)="onImageSelected($event)"></app-image-upload>
            </div>

            <div class="form-section row">
              <mat-form-field appearance="outline" class="flex-2">
                <mat-label>内容</mat-label>
                <input matInput formControlName="description" placeholder="例: 昼食代">
                <mat-error *ngIf="paymentForm.get('description')?.hasError('required')">内容は必須です</mat-error>
              </mat-form-field>

              <mat-form-field appearance="outline" class="flex-1">
                <mat-label>カテゴリ</mat-label>
                <mat-select formControlName="categoryId">
                  <mat-option *ngFor="let cat of categories" [value]="cat.id">
                    {{ cat.name }}
                  </mat-option>
                </mat-select>
              </mat-form-field>
            </div>

            <div class="form-section">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>日付</mat-label>
                <input matInput [matDatepicker]="picker" formControlName="paymentDate">
                <mat-datepicker-toggle matIconSuffix [for]="picker"></mat-datepicker-toggle>
                <mat-datepicker #picker></mat-datepicker>
              </mat-form-field>
            </div>

            <div class="participant-section">
              <div class="section-header">
                <h3>精算対象メンバー</h3>
                <span class="badge">全員均等</span>
              </div>
              <p class="hint">この支払いを割り振るメンバーを選択してください</p>
              <div class="participant-list">
                <div class="participant-item" *ngFor="let m of members">
                  <mat-checkbox [checked]="true" disabled>
                    {{ m.name }}
                  </mat-checkbox>
                </div>
              </div>
            </div>

            <div class="actions">
              <button mat-button type="button" [routerLink]="['/trips', tripId]">キャンセル</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="paymentForm.invalid || isSubmitting" class="submit-btn" [class.loading]="isSubmitting">
                <mat-icon *ngIf="!isSubmitting">save</mat-icon>
                <span>{{ isSubmitting ? '保存中...' : '保存する' }}</span>
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .container { display: flex; justify-content: center; padding: 40px 20px; min-height: 80vh; }
    .form-card {
      width: 100%;
      max-width: 600px;
      padding: 32px;
    }
    .glass-morphism {
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(20px);
      border: 1px solid rgba(255, 255, 255, 0.1);
      border-radius: 24px;
    }
    mat-card-title {
      display: flex;
      align-items: center;
      gap: 12px;
      font-size: 24px;
      font-weight: 600;
      margin-bottom: 24px;
    }
    .form-section { margin-bottom: 24px; }
    .section-label {
      display: block;
      margin-bottom: 8px;
      font-size: 14px;
      opacity: 0.8;
    }
    .row {
      display: flex;
      gap: 16px;
    }
    .flex-2 { flex: 2; }
    .flex-1 { flex: 1; }
    .full-width { width: 100%; }
    
    .participant-section {
      background: rgba(255, 255, 255, 0.03);
      padding: 20px;
      border-radius: 16px;
      margin-bottom: 32px;
    }
    .section-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 8px;
    }
    .badge {
      font-size: 11px;
      background: rgba(156, 136, 255, 0.2);
      color: #9c88ff;
      padding: 2px 8px;
      border-radius: 12px;
      border: 1px solid rgba(156, 136, 255, 0.3);
    }
    .participant-list { display: flex; flex-wrap: wrap; gap: 12px; }
    .participant-item {
      background: rgba(255, 255, 255, 0.05);
      padding: 4px 12px;
      border-radius: 8px;
    }
    .actions { display: flex; justify-content: flex-end; gap: 16px; }
    .submit-btn {
      padding: 0 24px;
      height: 48px;
      border-radius: 12px;
    }
  `]
})
export class PaymentFormComponent implements OnInit {
  paymentForm: FormGroup;
  tripId: string = '';
  isEdit: boolean = false;
  isSubmitting: boolean = false;
  receiptImage: string = '';

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
  }

  onImageSelected(base64: string): void {
    this.receiptImage = base64;
  }

  onSubmit(): void {
    if (this.paymentForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      const paymentData = {
        ...this.paymentForm.value,
        tripId: this.tripId,
        participantUserIds: this.members.map(m => m.id),
        receiptImage: this.receiptImage
      };
      
      this.paymentService.createPayment(paymentData).subscribe({
        next: () => {
          this.snackBar.open('支払いを登録しました', '閉じる', { duration: 3000 });
          this.router.navigate(['/trips', this.tripId]);
          this.isSubmitting = false;
        },
        error: () => {
          this.snackBar.open('登録に失敗しました', '閉じる', { duration: 3000 });
          this.isSubmitting = false;
        }
      });
    }
  }
}
