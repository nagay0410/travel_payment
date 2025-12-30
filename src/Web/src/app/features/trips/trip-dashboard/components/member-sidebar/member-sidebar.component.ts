import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { TripService } from '../../../../../core/services/trip.service';
import { MemberAvatarComponent } from '../../../../../shared/components/member-avatar/member-avatar.component';
import { AddMemberDialogComponent } from './add-member-dialog/add-member-dialog.component';

@Component({
  selector: 'app-member-sidebar',
  standalone: true,
  imports: [
    CommonModule, 
    MatCardModule, 
    MatIconModule, 
    MatDividerModule, 
    MatButtonModule, 
    MatDialogModule,
    MatSnackBarModule,
    MemberAvatarComponent
  ],
  template: `
    <mat-card class="sidebar-card glass-morphism">
      <mat-card-header>
        <mat-card-title>
          <mat-icon>group</mat-icon>
          参加メンバー
        </mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <ul class="member-list">
          <li *ngFor="let m of tripService.currentTrip()?.members || []" class="member-item">
            <app-member-avatar [name]="m.name"></app-member-avatar>
            <div class="member-info">
              <span class="member-name">{{ m.name }}</span>
              <span class="role-badge" [class.admin]="m.role === 'Admin'">
                {{ m.role === 'Admin' ? '管理者' : 'メンバー' }}
              </span>
            </div>
          </li>
        </ul>
        
        <mat-divider></mat-divider>
        
        <button mat-stroked-button color="primary" class="add-btn" (click)="onAddMember()">
          <mat-icon>person_add</mat-icon>
          <span>メンバーを招待</span>
        </button>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .sidebar-card {
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(20px);
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
      border-radius: 20px;
      padding: 16px;
    }
    mat-card-title {
      display: flex;
      align-items: center;
      gap: 12px;
      font-size: 18px;
      margin-bottom: 16px;
    }
    mat-card-title mat-icon { color: #9c88ff; }
    
    .member-list { list-style: none; padding: 0; margin: 16px 0; }
    .member-item {
      display: flex;
      align-items: center;
      gap: 16px;
      margin-bottom: 16px;
      padding: 4px;
    }
    .member-info { display: flex; flex-direction: column; gap: 2px; }
    .member-name { font-size: 14px; font-weight: 500; }
    .role-badge {
      font-size: 10px;
      padding: 1px 6px;
      background: rgba(255, 255, 255, 0.1);
      border-radius: 4px;
      width: fit-content;
      opacity: 0.7;
    }
    .role-badge.admin {
      background: rgba(156, 136, 255, 0.2);
      color: #9c88ff;
      border: 1px solid rgba(156, 136, 255, 0.3);
      opacity: 1;
    }
    
    mat-divider { margin: 16px 0; border-color: rgba(255, 255, 255, 0.1); }
    
    .add-btn {
      width: 100%;
      height: 48px;
      border-radius: 12px;
      border-style: dashed;
      color: white;
      border-color: rgba(255, 255, 255, 0.3);
    }
    .add-btn:hover { background: rgba(255, 255, 255, 0.05); }
  `]
})
export class MemberSidebarComponent {
  @Input() tripId: string = '';

  constructor(
    public tripService: TripService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  onAddMember(): void {
    const dialogRef = this.dialog.open(AddMemberDialogComponent, {
      width: '450px'
    });

    dialogRef.afterClosed().subscribe(userId => {
      if (userId && this.tripId) {
        this.tripService.addMember(this.tripId, userId).subscribe({
          next: () => {
            this.snackBar.open('メンバーを招待しました', '閉じる', { duration: 3000 });
            // 更新のために再取得
            this.tripService.getTripById(this.tripId).subscribe();
          },
          error: () => {
            this.snackBar.open('招待に失敗しました', '閉じる', { duration: 3000 });
          }
        });
      }
    });
  }
}
