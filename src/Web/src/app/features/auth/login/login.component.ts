import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  template: `
    <div class="auth-container">
      <mat-card class="auth-card">
        <mat-card-header>
          <mat-card-title>ログイン</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>メールアドレス</mat-label>
              <input matInput formControlName="email" type="email" placeholder="example@mail.com">
              <mat-error *ngIf="loginForm.get('email')?.hasError('required')">メールアドレスは必須です</mat-error>
              <mat-error *ngIf="loginForm.get('email')?.hasError('email')">有効なメールアドレスを入力してください</mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>パスワード</mat-label>
              <input matInput formControlName="password" type="password">
              <mat-error *ngIf="loginForm.get('password')?.hasError('required')">パスワードは必須です</mat-error>
            </mat-form-field>

            <button mat-raised-button color="primary" class="full-width" type="submit" [disabled]="loginForm.invalid">
              ログイン
            </button>
          </form>
        </mat-card-content>
        <mat-card-footer class="auth-footer">
          <p>アカウントをお持ちでないですか？ <a routerLink="/register">新規登録</a></p>
        </mat-card-footer>
      </mat-card>
    </div>
  `,
  styles: [`
    .auth-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: calc(100vh - 100px);
    }
    .auth-card {
      width: 100%;
      max-width: 400px;
      padding: 20px;
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
    }
    .full-width {
      width: 100%;
      margin-bottom: 16px;
    }
    .auth-footer {
      text-align: center;
      padding: 16px;
      font-size: 14px;
    }
    a { color: #81c784; text-decoration: none; }
  `]
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      this.authService.login(email, password).subscribe({
        next: (res) => {
          this.snackBar.open('ログインしました', '閉じる', { duration: 3000 });
          this.router.navigate(['/trips']);
        },
        error: (err) => {
          this.snackBar.open('ログインに失敗しました', '閉じる', { duration: 3000 });
        }
      });
    }
  }
}
