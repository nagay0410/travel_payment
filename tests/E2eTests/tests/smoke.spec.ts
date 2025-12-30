import { test, expect } from '@playwright/test';

test.describe('Travel Payment App Smoke Test', () => {
  test('should complete a full trip workflow', async ({ page }) => {
    // 1. トップページにアクセス
    await page.goto('/');
    await expect(page).toHaveTitle(/Travel Payment/);

    // 2. 新しい旅行を作成
    // ※ ログイン済みの状態、またはログイン導線が実装されている前提
    // ここでは UI 要素の存在を確認しつつ操作を進める
    await page.click('button:has-text("新しい旅行を作成")');
    await page.fill('input[placeholder="旅行名"]', 'テスト旅行 ' + new Date().getTime());
    await page.click('button:has-text("作成")');

    // 3. 旅行ダッシュボードが表示されることを確認
    await expect(page.locator('.trip-title')).toBeVisible();

    // 4. メンバーを招待
    await page.click('button:has-text("メンバーを招待")');
    await page.fill('input[placeholder="ユーザー名を入力..."]', 'Admin'); // デフォルトユーザー
    await page.keyboard.press('ArrowDown');
    await page.keyboard.press('Enter');
    await page.click('button:has-text("招待を送る")');
    // トースト等のフィードバックを待機
    await expect(page.locator('text=メンバーを招待しました')).toBeVisible();

    // 5. 支払いを登録
    await page.click('button:has-text("支払いを追加")');
    await page.fill('input[formControlName="title"]', 'ランチ代');
    await page.fill('input[formControlName="amount"]', '5000');
    // カテゴリ選択、画像アップロードなども必要に応じて追加
    await page.click('button:has-text("登録する")');
    await expect(page.locator('text=支払いを登録しました')).toBeVisible();

    // 6. 精算状況を確認し、精算を完了する
    await page.click('button:has-text("精算状況を確認")');
    await expect(page.locator('.settlement-card')).toBeVisible();
    await page.click('button:has-text("精算を完了する")');
    
    // ダイアログが表示されることを確認
    await expect(page.locator('mat-dialog-container')).toBeVisible();
    await page.click('button:has-text("精算済みにする")');

    // 最終的な完了メッセージ
    await expect(page.locator('text=精算を完了しました')).toBeVisible();
  });
});
