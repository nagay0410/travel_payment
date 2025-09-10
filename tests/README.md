# TravelPayment Tests

## 概要
旅行支払い精算アプリのテストプロジェクト群です。

## テスト戦略
- **テストピラミッド**に基づくテスト構成
- **ユニットテスト**: 70%（高速・低コスト）
- **統合テスト**: 20%（中速・中コスト）
- **E2Eテスト**: 10%（低速・高コスト）

## プロジェクト構成

### API Tests
- **対象**: Web API のエンドポイント
- **フレームワーク**: xUnit + WebApplicationFactory
- **内容**: HTTP リクエスト・レスポンスのテスト

### Application Tests
- **対象**: アプリケーションサービスのビジネスロジック
- **フレームワーク**: xUnit + Moq
- **内容**: サービス層の単体テスト

### Domain Tests
- **対象**: ドメインエンティティと値オブジェクト
- **フレームワーク**: xUnit
- **内容**: ドメインルールのテスト

### Infrastructure Tests
- **対象**: データアクセス層と外部サービス
- **フレームワーク**: xUnit + EF Core In-Memory
- **内容**: リポジトリとデータベース操作のテスト

## テスト実行

### 全テスト実行
```bash
dotnet test
```

### 特定プロジェクトのテスト実行
```bash
cd tests/API
dotnet test

cd tests/Application
dotnet test

cd tests/Domain
dotnet test

cd tests/Infrastructure
dotnet test
```

### カテゴリ別テスト実行
```bash
# 統合テストのみ実行
dotnet test --filter "Category=Integration"

# ユニットテストのみ実行
dotnet test --filter "Category=Unit"
```

## テストカバレッジ
- **目標**: 80%以上のコードカバレッジ
- **測定ツール**: Coverlet
- **レポート**: Codecov で管理

## テストデータ
- **ファクトリ**: TestDataFactory クラス
- **データベース**: In-Memory Database
- **クリーンアップ**: 各テスト後に自動実行

## CI/CD
- **GitHub Actions**: プルリクエスト時に自動実行
- **テスト結果**: カバレッジレポートを自動生成
- **品質ゲート**: テスト失敗時はマージ不可
