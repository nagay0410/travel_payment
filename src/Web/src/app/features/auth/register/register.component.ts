import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
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
          <mat-card-title>ユーザー登録</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="registerForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>ユーザー名</mat-label>
              <input matInput formControlName="username" placeholder="山田 太郎">
              <mat-error *ngIf="registerForm.get('username')?.hasError('required')">ユーザー名は必須です</mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>メールアドレス</mat-label>
              <input matInput formControlName="email" type="email" placeholder="example@mail.com">
              <mat-error *ngIf="registerForm.get('email')?.hasError('required')">メールアドレスは必須です</mat-error>
              <mat-error *ngIf="registerForm.get('email')?.hasError('email')">有効なメールアドレスを入力してください</mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>パスワード</mat-label>
              <input matInput formControlName="password" type="password">
              <mat-error *ngIf="registerForm.get('password')?.hasError('required')">パスワードは必須です</mat-error>
              <mat-error *ngIf="registerForm.get('password')?.hasError('minlength')">パスワードは6文字以上で入力してください</mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>パスワード（確認）</mat-label>
              <input matInput formControlName="confirmPassword" type="password">
              <mat-error *ngIf="registerForm.get('confirmPassword')?.hasError('required')">確認用パスワードは必須です</mat-error>
              <mat-error *ngIf="registerForm.hasError('passwordMismatch')">パスワードが一致しません</mat-error>
            </mat-form-field>

            <button mat-raised-button color="primary" class="full-width" type="submit" [disabled]="registerForm.invalid">
              登録
            </button>
          </form>
        </mat-card-content>
        <mat-card-footer class="auth-footer">
          <p>すでにアカウントをお持ちですか？ <a routerLink="/login">ログイン</a></p>
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
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService
  ) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { passwordMismatch: true };
    }
    return null;
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const { username, email, password } = this.registerForm.value;
      this.authService.register({ username, email, password }).subscribe({
        next: () => {
          this.snackBar.open('ユーザー登録が完了しました。ログインしてください。', '閉じる', { duration: 3000 });
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.error(err);
          this.snackBar.open('登録に失敗しました。もう一度お試しください。', '閉じる', { duration: 3000 });
        }
      });
    }
  }
}
