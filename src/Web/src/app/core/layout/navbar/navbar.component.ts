import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule, MatToolbarModule, MatButtonModule, MatIconModule, MatMenuModule],
  template: `
    <mat-toolbar color="primary" class="navbar">
      <span class="logo" routerLink="/">Travel Settlement</span>
      <span class="spacer"></span>
      
      <ng-container *ngIf="authService.isAuthenticated();">
        <button mat-button routerLink="/trips">旅行一覧</button>
        <button mat-icon-button [matMenuTriggerFor]="menu">
          <mat-icon>account_circle</mat-icon>
        </button>
        <mat-menu #menu="matMenu">
          <div class="user-info" mat-menu-item disabled>
            {{ authService.currentUser()?.username }}
          </div>
          <button mat-menu-item (click)="onLogout()">
            <mat-icon>logout</mat-icon>
            <span>ログアウト</span>
          </button>
        </mat-menu>
      </ng-container>

      <ng-template #loginBtn>
        <button mat-stroked-button color="accent" routerLink="/login">ログイン</button>
      </ng-template>
    </mat-toolbar>
  `,
  styles: [`
    .navbar {
      background: rgba(48, 63, 159, 0.8) !important;
      backdrop-filter: blur(10px);
      position: sticky;
      top: 0;
      z-index: 1000;
      box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    }
    .spacer { flex: 1 1 auto; }
    .logo { cursor: pointer; font-weight: bold; }
    .user-info { font-size: 12px; color: #666; }
  `]
})
export class NavbarComponent {
  constructor(
    public authService: AuthService,
    private router: Router
  ) {}

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
