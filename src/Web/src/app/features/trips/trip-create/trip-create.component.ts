import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { TripService } from '../../../core/services/trip.service';

@Component({
  selector: 'app-trip-create',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],
  template: `
    <div class="container">
      <mat-card class="form-card">
        <mat-card-header>
          <mat-card-title>新しい旅行を作成</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="tripForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>旅行名</mat-label>
              <input matInput formControlName="name" placeholder="例: 北海道旅行">
              <mat-error *ngIf="tripForm.get('name')?.hasError('required')">旅行名は必須です</mat-error>
            </mat-form-field>

            <div class="date-range">
              <mat-form-field appearance="outline">
                <mat-label>開始日</mat-label>
                <input matInput [matDatepicker]="startPicker" formControlName="startDate">
                <mat-datepicker-toggle matIconSuffix [for]="startPicker"></mat-datepicker-toggle>
                <mat-datepicker #startPicker></mat-datepicker>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>終了日</mat-label>
                <input matInput [matDatepicker]="endPicker" formControlName="endDate">
                <mat-datepicker-toggle matIconSuffix [for]="endPicker"></mat-datepicker-toggle>
                <mat-datepicker #endPicker></mat-datepicker>
              </mat-form-field>
            </div>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>予算（任意）</mat-label>
              <input matInput type="number" formControlName="budget" placeholder="30000">
              <span matPrefix>￥&nbsp;</span>
            </mat-form-field>

            <div class="actions">
              <button mat-button type="button" routerLink="/trips">キャンセル</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="tripForm.invalid">
                作成する
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .container {
      display: flex;
      justify-content: center;
      padding-top: 40px;
    }
    .form-card {
      width: 100%;
      max-width: 600px;
      padding: 20px;
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
    }
    .full-width { width: 100%; margin-bottom: 16px; }
    .date-range {
      display: flex;
      gap: 16px;
      margin-bottom: 16px;
    }
    .date-range mat-form-field { flex: 1; }
    .actions {
      display: flex;
      justify-content: flex-end;
      gap: 12px;
      margin-top: 24px;
    }
  `]
})
export class TripCreateComponent {
  tripForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private tripService: TripService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.tripForm = this.fb.group({
      name: ['', [Validators.required]],
      startDate: [new Date(), [Validators.required]],
      endDate: [new Date(), [Validators.required]],
      budget: [0]
    });
  }

  onSubmit(): void {
    if (this.tripForm.valid) {
      this.tripService.createTrip(this.tripForm.value).subscribe({
        next: (res) => {
          this.snackBar.open('旅行を作成しました', '閉じる', { duration: 3000 });
          this.router.navigate(['/trips']);
        },
        error: (err) => {
          this.snackBar.open('旅行の作成に失敗しました', '閉じる', { duration: 3000 });
        }
      });
    }
  }
}
