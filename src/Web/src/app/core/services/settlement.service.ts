import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface Settlement {
  fromUserId: string;
  fromUserName: string;
  toUserId: string;
  toUserName: string;
  amount: number;
}

@Injectable({
  providedIn: 'root'
})
export class SettlementService {
  private readonly API_URL = '/api/settlements';
  
  calculations = signal<Settlement[]>([]);

  constructor(private http: HttpClient) {}

  /**
   * 旅行の精算計算結果を取得
   */
  calculate(tripId: string): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/calculate/${tripId}`).pipe(
      tap(res => {
        if (res.success) {
          this.calculations.set(res.data);
        }
      })
    );
  }

  /**
   * 精算を完了（実行）する
   */
  completeSettlement(tripId: string): Observable<any> {
    return this.http.post<any>(`${this.API_URL}/complete`, { tripId });
  }
}
