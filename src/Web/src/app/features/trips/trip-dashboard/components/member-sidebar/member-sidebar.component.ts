import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MemberAvatarComponent } from '../../../../../shared/components/member-avatar/member-avatar.component';

@Component({
  selector: 'app-member-sidebar',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatDividerModule, MemberAvatarComponent],
  template: `
    <mat-card class="sidebar-card">
      <mat-card-header>
        <mat-card-title>参加メンバー</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <ul class="member-list">
          <li *ngFor="let m of members" class="member-item">
            <app-member-avatar [name]="m.name"></app-member-avatar>
            <span class="member-name">{{ m.name }}</span>
            <span class="role-badge" *ngIf="m.role === 'Admin'">管理者</span>
          </li>
        </ul>
        <button class="add-btn">
          <mat-icon>person_add</mat-icon> メンバーを招待
        </button>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .sidebar-card {
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      border: 1px solid rgba(255, 255, 255, 0.1);
      color: white;
    }
    .member-list { list-style: none; padding: 0; margin: 16px 0; }
    .member-item {
      display: flex;
      align-items: center;
      gap: 12px;
      margin-bottom: 12px;
    }
    .member-name { flex: 1; }
    .role-badge {
      font-size: 10px;
      padding: 2px 6px;
      background: rgba(255, 255, 255, 0.1);
      border-radius: 4px;
    }
    .add-btn {
      width: 100%;
      background: none;
      border: 1px dashed rgba(255, 255, 255, 0.3);
      color: white;
      padding: 8px;
      border-radius: 8px;
      cursor: pointer;
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
      transition: background 0.2s;
    }
    .add-btn:hover { background: rgba(255, 255, 255, 0.1); }
  `]
})
export class MemberSidebarComponent {
  @Input() tripId: string = '';

  // 暫定的なデータ（本来は API から取得）
  members = [
    { name: 'あなた', role: 'Admin' },
    { name: '友人A', role: 'Member' },
    { name: '友人B', role: 'Member' }
  ];
}
