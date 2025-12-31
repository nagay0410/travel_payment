import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';

/**
 * ユーザー情報（詳細設計・シーケンス図準拠）
 */
export interface User {
  id: string;
  username: string;
  email: string;
}

/**
 * バックエンドの認証結果レスポンス構造
 */
export interface AuthenticationResult {
  userId: string;
  userName: string;
  email: string;
  token: string;
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
    
    if ((token && userJson) &&
        (token !== 'undefined') &&
        (userJson !== 'undefined')
    ) {
      this.currentUser.set(JSON.parse(userJson));
      this.isAuthenticated.set(true);
    }
  }

  /**
   * ログイン処理（シーケンス図準拠）
   */
  login(email: string, password: string): Observable<ApiResponse<AuthenticationResult>> {
    return this.http.post<ApiResponse<AuthenticationResult>>('/api/auth/login', { email, password }).pipe(
      tap(res => {
        if (res.success && res.data) {
          localStorage.setItem(this.TOKEN_KEY, res.data.token);
          const user: User = {
            id: res.data.userId,
            username: res.data.userName,
            email: res.data.email
          };
          localStorage.setItem(this.USER_KEY, JSON.stringify(user));
          this.currentUser.set(user);
          this.isAuthenticated.set(true);
        }
      })
    );
  }

  /**
   * ユーザー登録処理
   */
  register(user: { username: string; email: string; password: string }): Observable<void> {
    return this.http.post<void>('/api/users', user);
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
