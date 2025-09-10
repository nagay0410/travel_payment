# 旅行支払い精算アプリ 要件定義書

## 1. プロジェクト概要

### 1.1 目的
友人と旅行の際、支払いを立て替えて後ほど精算するために、金額を管理することができるアプリケーション

### 1.2 対象ユーザー
- 友人同士で旅行に行く人
- グループ旅行の参加者
- 支払いの精算を効率的に行いたい人

## 2. 機能要件

### 2.1 コア機能

#### 2.1.1 旅行管理
- 旅行の作成、編集、削除
- 旅行名、期間、参加者、予算の設定
- 旅行のステータス管理（計画中、実施中、完了）

#### 2.1.2 メンバー管理
- 旅行参加者の追加・削除
- 参加者の権限管理（管理者、一般参加者）

#### 2.1.3 支払い記録
- 誰が何にいくら支払ったかの記録
- 支払い者、支払い項目、金額、日付の記録
- カテゴリ別の支払い分類（交通費、宿泊費、食費、娯楽費など）
- 写真やレシートの添付（オプション）

#### 2.1.4 精算計算
- 全員の支払い総額を均等割りで計算
- 個人別の精算額の自動算出
- 最小精算回数での精算方法の提案

#### 2.1.5 精算管理
- 精算済み・未精算の状態管理
- 精算履歴の記録
- 精算方法の記録（現金、振込、決済アプリなど）

### 2.2 詳細機能

#### 2.2.1 ダッシュボード
- 旅行一覧の表示
- 精算状況の概要表示
- 最近の支払い記録

#### 2.2.2 レポート機能
- 旅行別の支払いサマリー
- 個人別の支払い・精算状況
- カテゴリ別の支払い分析

#### 2.2.3 通知機能
- 精算期限のリマインダー
- 新しい支払い記録の通知
- 精算完了の通知

## 3. 技術要件

### 3.1 バックエンド（C#）
- **フレームワーク**: ASP.NET Core Web API
- **ORM**: Entity Framework Core
- **データベース**: SQL Server または PostgreSQL
- **認証**: JWT（JSON Web Token）
- **API ドキュメント**: Swagger/OpenAPI
- **ログ**: Serilog
- **テスト**: xUnit

### 3.2 フロントエンド（Angular）
- **フレームワーク**: Angular 17+
- **UI ライブラリ**: Angular Material
- **状態管理**: RxJS
- **ルーティング**: Angular Router
- **HTTP クライアント**: Angular HttpClient
- **フォーム**: Angular Reactive Forms
- **テスト**: Jasmine + Karma

### 3.3 インフラ・運用
- **コンテナ**: Docker
- **CI/CD**: GitHub Actions
- **ホスティング**: Azure App Service または AWS
- **データベース**: Azure SQL Database または AWS RDS

## 4. データベース設計

### 4.1 主要テーブル

#### Users（ユーザー情報）
- UserId (PK)
- Username
- Email
- PasswordHash
- CreatedAt
- UpdatedAt

#### Trips（旅行情報）
- TripId (PK)
- TripName
- Description
- StartDate
- EndDate
- Budget
- Status
- CreatedBy (FK to Users)
- CreatedAt
- UpdatedAt

#### TripMembers（旅行参加者）
- TripMemberId (PK)
- TripId (FK to Trips)
- UserId (FK to Users)
- Role (Admin, Member)
- JoinedAt

#### Categories（支払いカテゴリ）
- CategoryId (PK)
- CategoryName
- Description
- Icon

#### Payments（支払い記録）
- PaymentId (PK)
- TripId (FK to Trips)
- PaidBy (FK to Users)
- CategoryId (FK to Categories)
- Amount
- Description
- PaymentDate
- ReceiptImage
- CreatedAt
- UpdatedAt

#### Settlements（精算記録）
- SettlementId (PK)
- TripId (FK to Trips)
- FromUserId (FK to Users)
- ToUserId (FK to Users)
- Amount
- Status (Pending, Completed)
- SettlementMethod
- CompletedAt
- CreatedAt

### 4.2 リレーション
- 1つの旅行に複数の参加者がいる（1:N）
- 1つの旅行に複数の支払い記録がある（1:N）
- 1つの旅行に複数の精算記録がある（1:N）
- 1つのユーザーが複数の旅行に参加できる（N:M）

## 5. 非機能要件

### 5.1 パフォーマンス
- ページロード時間: 3秒以内
- API レスポンス時間: 1秒以内
- 同時接続ユーザー数: 100人以上

### 5.2 セキュリティ
- ユーザー認証・認可
- データの暗号化
- SQL インジェクション対策
- XSS 対策
- CSRF 対策

### 5.3 可用性
- システム稼働率: 99.9%以上
- データバックアップ: 日次
- 障害復旧時間: 4時間以内

### 5.4 ユーザビリティ
- レスポンシブデザイン（モバイル対応）
- 直感的なUI/UX
- アクセシビリティ対応
- 多言語対応（日本語・英語）

## 6. 開発フェーズ

### フェーズ1: 基本機能（MVP）
- ユーザー認証
- 旅行管理
- 支払い記録
- 基本的な精算計算

### フェーズ2: 高度な機能
- 精算管理
- レポート機能
- 通知機能

### フェーズ3: 拡張機能
- オフライン対応
- データエクスポート
- 高度な分析機能

## 7. 制約事項

### 7.1 技術的制約
- モダンなブラウザでの動作保証
- モバイルデバイスでの操作性確保

### 7.2 運用制約
- データの永続化
- ユーザーデータの保護
- プライバシーの確保

## 8. 成功指標

### 8.1 技術指標
- システムの安定性
- レスポンス時間
- エラー率

### 8.2 ビジネス指標
- ユーザー登録数
- アクティブユーザー数
- ユーザー満足度

---

*この要件定義書は開発の進行に応じて更新される場合があります。*
