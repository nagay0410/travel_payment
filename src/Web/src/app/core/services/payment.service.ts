import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface Payment {
  id: string;
  tripId: string;
  amount: number;
  description: string;
  paidByUserId: string;
  paidByUserName: string;
  categoryId: string;
  categoryName: string;
  paymentDate: string;
  participantUserIds: string[];
  receiptImage?: string;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private readonly API_URL = '/api/payments';
  
  payments = signal<Payment[]>([]);

  constructor(private http: HttpClient) {}

  /**
   * 旅行に紐づく支払い一覧を取得
   */
  getPaymentsByTripId(tripId: string): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/trip/${tripId}`).pipe(
      tap(res => {
        if (res.success) {
          this.payments.set(res.data);
        }
      })
    );
  }

  /**
   * 支払いを新規登録
   */
  createPayment(payment: Partial<Payment>): Observable<any> {
    return this.http.post<any>(this.API_URL, payment);
  }
}
