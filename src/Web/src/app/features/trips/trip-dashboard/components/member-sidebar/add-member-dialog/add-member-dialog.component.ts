import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { UserService } from '../../../../../../core/services/user.service';
import { debounceTime, switchMap, finalize } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-add-member-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="dialog-container glass-morphism">
      <h2 mat-dialog-title>
        <mat-icon color="primary">person_add</mat-icon>
        メンバーを招待
      </h2>
      
      <mat-dialog-content>
        <p class="description">ユーザー名またはメールアドレスで検索して、旅行メンバーに追加します。</p>
        
        <form [formGroup]="searchForm">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>メンバーを検索</mat-label>
            <input
              matInput
              placeholder="ユーザー名を入力..."
              formControlName="query"
              [matAutocomplete]="auto"
            >
            <mat-icon matSuffix *ngIf="!isLoading">search</mat-icon>
            <mat-spinner matSuffix *ngIf="isLoading" diameter="20"></mat-spinner>
            
            <mat-autocomplete #auto="matAutocomplete" [displayWith]="displayFn" (optionSelected)="onUserSelected($event.option.value)">
              <mat-option *ngFor="let user of foundUsers" [value]="user">
                <div class="user-option">
                  <div class="avatar-mini">{{ user.name.charAt(0) }}</div>
                  <div class="user-details">
                    <span class="name">{{ user.name }}</span>
                    <span class="id">{{ '@' }}{{ user.id.substring(0, 8) }}</span>
                  </div>
                </div>
              </mat-option>
              <mat-option *ngIf="foundUsers.length === 0 && !isLoading && searchForm.get('query')?.value" disabled>
                ユーザーが見つかりませんでした
              </mat-option>
            </mat-autocomplete>
          </mat-form-field>
        </form>

        <div class="selected-users" *ngIf="selectedUser">
          <h3>選択されたユーザー</h3>
          <div class="user-chip">
            <div class="avatar-mini">{{ selectedUser.name.charAt(0) }}</div>
            <span class="name">{{ selectedUser.name }}</span>
            <button mat-icon-button (click)="removeSelection()" class="remove-btn">
              <mat-icon>close</mat-icon>
            </button>
          </div>
        </div>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="onCancel()">キャンセル</button>
        <button
          mat-raised-button
          color="primary"
          [disabled]="!selectedUser || isSubmitting"
          (click)="onConfirm()"
        >
          招待を送る
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-container { padding: 8px; color: white; }
    h2 { display: flex; align-items: center; gap: 12px; font-size: 20px; }
    .description { font-size: 13px; opacity: 0.7; margin-bottom: 24px; }
    .full-width { width: 100%; }
    .user-option { display: flex; align-items: center; gap: 12px; padding: 4px 0; }
    .avatar-mini {
      width: 32px; height: 32px; background: #9c88ff; border-radius: 8px;
      display: flex; justify-content: center; align-items: center; font-weight: bold; font-size: 14px;
    }
    .user-details { display: flex; flex-direction: column; }
    .user-details .name { font-size: 14px; font-weight: 500; }
    .user-details .id { font-size: 11px; opacity: 0.5; }
    
    .selected-users { margin-top: 24px; }
    .selected-users h3 { font-size: 12px; opacity: 0.6; text-transform: uppercase; letter-spacing: 1px; margin-bottom: 8px; }
    .user-chip {
      display: flex; align-items: center; gap: 12px; background: rgba(255, 255, 255, 0.1);
      padding: 8px 12px; border-radius: 12px; border: 1px solid rgba(255, 255, 255, 0.1);
    }
    .remove-btn { width: 24px; height: 24px; line-height: 24px; }
    .remove-btn mat-icon { font-size: 18px; width: 18px; height: 18px; }
    
    mat-dialog-actions { padding: 16px 0 8px; }
  `]
})
export class AddMemberDialogComponent {
  searchForm: FormGroup;
  foundUsers: any[] = [];
  selectedUser: any | null = null;
  isLoading = false;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    public dialogRef: MatDialogRef<AddMemberDialogComponent>
  ) {
    this.searchForm = this.fb.group({
      query: ['']
    });

    this.searchForm.get('query')?.valueChanges.pipe(
      debounceTime(300),
      switchMap(query => {
        if (typeof query !== 'string' || query.length < 2) {
          this.foundUsers = [];
          return of([]);
        }
        this.isLoading = true;
        return this.userService.searchUsers(query).pipe(
          finalize(() => this.isLoading = false)
        );
      })
    ).subscribe(res => {
      // res.data がユーザー配列であることを想定
      this.foundUsers = res.data || [];
    });
  }

  displayFn(user: any): string {
    return user && user.name ? user.name : '';
  }

  onUserSelected(user: any): void {
    this.selectedUser = user;
    this.searchForm.get('query')?.setValue('', { emitEvent: false });
  }

  removeSelection(): void {
    this.selectedUser = null;
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  onConfirm(): void {
    if (this.selectedUser) {
      this.dialogRef.close(this.selectedUser.id);
    }
  }
}
