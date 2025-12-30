import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface Trip {
  id: string;
  name: string;
  startDate: string;
  endDate: string;
  budget: number;
  totalSpent: number;
  status: '計画中' | '実施中' | '完了';
}

@Injectable({
  providedIn: 'root'
})
export class TripService {
  private readonly API_URL = '/api/trips';
  
  // 状態管理用の Signals
  trips = signal<Trip[]>([]);
  currentTrip = signal<Trip | null>(null);

  constructor(private http: HttpClient) {}

  /**
   * 参加している旅行一覧を取得
   */
  getTrips(): Observable<any> {
    return this.http.get<any>(this.API_URL).pipe(
      tap(res => {
        if (res.success) {
          this.trips.set(res.data);
        }
      })
    );
  }

  /**
   * 特定の旅行詳細を取得
   */
  getTripById(id: string): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/${id}`).pipe(
      tap(res => {
        if (res.success) {
          this.currentTrip.set(res.data);
        }
      })
    );
  }

  /**
   * 旅行を新規作成
   */
  createTrip(trip: Partial<Trip>): Observable<any> {
    return this.http.post<any>(this.API_URL, trip);
  }
}
