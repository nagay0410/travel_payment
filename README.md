# TravelPayment - 旅行支払い精算アプリ

## 概要
友人と旅行の際、支払いを立て替えて後ほど精算するために、金額を管理することができるアプリケーションです。

## 機能
- **旅行管理**: 旅行の作成、編集、削除
- **メンバー管理**: 旅行参加者の追加・削除
- **支払い記録**: 誰が何にいくら支払ったかの記録
- **精算計算**: 自動的な精算金額の算出
- **精算管理**: 精算済み・未精算の管理

## 技術スタック

### バックエンド
- **フレームワーク**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0
- **データベース**: PostgreSQL 18
- **アーキテクチャ**: クリーンアーキテクチャ

### フロントエンド
- **フレームワーク**: Angular 19+
- **UI ライブラリ**: Angular Material
- **状態管理**: RxJS

## プロジェクト構造

```
TravelPayment/
├── src/                          # ソースコード
│   ├── API/                      # Web API プロジェクト
│   ├── Application/              # アプリケーション層
│   ├── Domain/                   # ドメイン層
│   ├── Infrastructure/           # インフラストラクチャ層
│   └── Shared/                   # 共通ライブラリ
├── tests/                        # テストプロジェクト
│   ├── API/                      # API テスト
│   ├── Application/              # アプリケーション層テスト
│   ├── Domain/                   # ドメイン層テスト
│   └── Infrastructure/           # インフラ層テスト
└── docs/                         # ドキュメント
    ├── 要件定義/                  # 要件定義書
    │   └── requirements.md
    ├── API/                      # API設計
    │   └── API設計書.md
    ├── データベース/              # データベース設計
    │   └── データベース設計書.md
    ├── アーキテクチャ/            # アーキテクチャ設計
    │   └── アーキテクチャ設計書.md
    ├── セキュリティ/              # セキュリティ設計
    │   └── セキュリティ設計書.md
    ├── テスト/                    # テスト設計
    │   └── テスト設計書.md
    ├── 環境構築/                  # 開発環境セットアップ
    │   └── 環境構築.md
    └── 設計書一覧.md              # 設計書一覧
```

## 開発環境のセットアップ

### 前提条件
- .NET 8.0 SDK
- PostgreSQL 18
- Node.js 20+

### バックエンドの起動
```bash
# 依存関係の復元
dotnet restore

# データベースのマイグレーション
cd src/API
dotnet ef database update

# アプリケーションの起動
dotnet run
```

### フロントエンドの起動
```bash
# 依存関係のインストール
npm install

# 開発サーバーの起動
npm start
```

### 詳細手順
[環境構築手順](docs/環境構築/環境構築.md) を参照してください。

## テスト実行

### ユニットテスト (バックエンド)
```bash
dotnet test
```

### E2E テスト (Playwright)
```bash
cd tests/E2eTests
npm install
npx playwright install
npx playwright test
```

## API ドキュメント
起動後、以下のURLでSwagger UIにアクセスできます：
- https://nagay0410.github.io/travel_payment/swagger/

## 設計書
詳細な設計書は `docs/` フォルダに格納されています：
- [設計書一覧](docs/設計書一覧.md)
- [要件定義](docs/要件定義/requirements.md)
- [API設計書](docs/基本設計/API/API設計書.md)
- [データベース設計書](docs/基本設計/データベース/データベース設計書.md)
- [アーキテクチャ設計書](docs/基本設計/アーキテクチャ/アーキテクチャ設計書.md)
- [セキュリティ設計書](docs/基本設計/セキュリティ/セキュリティ設計書.md)
- [テスト設計書](docs/基本設計/テスト/テスト設計書.md)
- [環境構築](docs/環境構築/環境構築.md)

## 開発フェーズ

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

## 貢献
1. このリポジトリをフォーク
2. フィーチャーブランチを作成 (`git checkout -b feature/amazing-feature`)
3. 変更をコミット (`git commit -m 'Add some amazing feature'`)
4. ブランチにプッシュ (`git push origin feature/amazing-feature`)
5. プルリクエストを作成

## ライセンス
このプロジェクトはMITライセンスの下で公開されています。
