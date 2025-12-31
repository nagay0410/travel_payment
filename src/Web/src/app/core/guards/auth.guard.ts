import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * 認証ガード（関数型）
 * 未認証ユーザーをログイン画面にリダイレクトする
 */
export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  // 未認証の場合はログイン画面にリダイレクト
  return router.createUrlTree(['/login']);
};
