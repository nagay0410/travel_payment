import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id: string;
  name: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly API_URL = '/api/users';

  constructor(private http: HttpClient) {}

  /**
   * 名前またはメールアドレスでユーザーを検索
   */
  searchUsers(query: string): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/search?q=${query}`);
  }

  /**
   * 現在のログインユーザー情報を取得
   */
  getCurrentUser(): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/me`);
  }
}
