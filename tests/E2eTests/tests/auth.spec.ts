import { test, expect } from '@playwright/test';

test.describe('認証フロー', () => {
  test.beforeEach(async ({ page }) => {
    // 各テスト前にLocalStorageをクリア
    await page.context().clearCookies();
    await page.goto('/');
    await page.evaluate(() => localStorage.clear());
  });

  test('未認証アクセス: ルートにアクセスするとログイン画面にリダイレクトされる', async ({ page }) => {
    await page.goto('/');
    await expect(page).toHaveURL(/\/login/);
    await expect(page.locator('text=ログイン')).toBeVisible();
  });

  test('未認証アクセス: /trips に直接アクセスするとログイン画面にリダイレクトされる', async ({ page }) => {
    await page.goto('/trips');
    await expect(page).toHaveURL(/\/login/);
  });

  test('未認証アクセス: /trips/create に直接アクセスするとログイン画面にリダイレクトされる', async ({ page }) => {
    await page.goto('/trips/create');
    await expect(page).toHaveURL(/\/login/);
  });

  test('ログインフロー: 正しい認証情報でログインするとメイン画面に遷移する', async ({ page }) => {
    // ログイン画面にアクセス
    await page.goto('/login');
    await expect(page.locator('text=ログイン')).toBeVisible();

    // 認証情報を入力
    await page.fill('input[formControlName="email"]', 'test@example.com');
    await page.fill('input[formControlName="password"]', 'password');
    await page.click('button[type="submit"]');

    // /trips に遷移することを確認
    await expect(page).toHaveURL(/\/trips/, { timeout: 10000 });

    // LocalStorageにトークンが保存されることを確認
    const token = await page.evaluate(() => localStorage.getItem('access_token'));
    expect(token).not.toBeNull();
  });

  test('ログアウトフロー: ログアウトするとログイン画面に遷移する', async ({ page }) => {
    // まずログイン状態をシミュレート（LocalStorageにトークンを設定）
    await page.goto('/login');
    await page.evaluate(() => {
      localStorage.setItem('access_token', 'test-token');
      localStorage.setItem('current_user', JSON.stringify({ id: '1', username: 'test', email: 'test@example.com' }));
    });

    // ページをリロードして認証状態を反映
    await page.goto('/trips');

    // ログアウトボタンをクリック
    await page.click('button:has-text("ログアウト")');

    // /login に遷移することを確認
    await expect(page).toHaveURL(/\/login/);

    // LocalStorageからトークンが削除されることを確認
    const token = await page.evaluate(() => localStorage.getItem('access_token'));
    expect(token).toBeNull();
  });

  test('認証済みアクセス: ログイン状態で保護されたページにアクセスできる', async ({ page }) => {
    // LocalStorageにトークンを設定してログイン状態をシミュレート
    await page.goto('/login');
    await page.evaluate(() => {
      localStorage.setItem('access_token', 'test-token');
      localStorage.setItem('current_user', JSON.stringify({ id: '1', username: 'test', email: 'test@example.com' }));
    });

    // 保護されたページにアクセス
    await page.goto('/trips');

    // リダイレクトされずに /trips に留まることを確認
    await expect(page).toHaveURL(/\/trips/);
  });
});
