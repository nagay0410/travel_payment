import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';

/**
 * ユーザー情報（詳細設計・シーケンス図準拠）
 */
export interface User {
  id: string;
  username: string;
  email: string;
}

/**
 * ログインレスポンス
 */
export interface AuthResponse {
  success: boolean;
  data: {
    user: User;
    accessToken: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'access_token';
  private readonly USER_KEY = 'current_user';

  // Angular Signals を使用してリアクティブな状態管理を実現
  currentUser = signal<User | null>(null);
  isAuthenticated = signal<boolean>(false);

  constructor(private http: HttpClient) {
    this.loadStoredAuth();
  }

  /**
   * LocalStorage から認証情報を復元
   */
  private loadStoredAuth(): void {
    const token = localStorage.getItem(this.TOKEN_KEY);
    const userJson = localStorage.getItem(this.USER_KEY);
    
    if (token && userJson) {
      this.currentUser.set(JSON.parse(userJson));
      this.isAuthenticated.set(true);
    }
  }

  /**
   * ログイン処理（シーケンス図準拠）
   */
  login(email: string, password: string): Observable<AuthResponse> {
    // 暫定的に /api/auth/login を想定
    return this.http.post<AuthResponse>('/api/auth/login', { email, password }).pipe(
      tap(res => {
        if (res.success) {
          localStorage.setItem(this.TOKEN_KEY, res.data.accessToken);
          localStorage.setItem(this.USER_KEY, JSON.stringify(res.data.user));
          this.currentUser.set(res.data.user);
          this.isAuthenticated.set(true);
        }
      })
    );
  }

  /**
   * ログアウト
   */
  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
  }

  /**
   * 保存されているトークンの取得
   */
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }
}
