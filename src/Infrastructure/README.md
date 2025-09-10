# TravelPayment.Infrastructure

## 概要
旅行支払い精算アプリのインフラストラクチャ層プロジェクトです。

## 役割
- データベースアクセス
- 外部サービスとの連携
- 設定管理
- ログ・監視

## 主要コンポーネント
- **Repositories**: データアクセス層の実装
- **Database Context**: Entity Framework Core の設定
- **External Services**: 外部APIとの連携
- **Configuration**: アプリケーション設定

## 技術スタック
- .NET 8.0
- Entity Framework Core 8.0
- SQL Server / PostgreSQL
- Serilog

## 依存関係
- TravelPayment.Domain

## 設計原則
- リポジトリパターン
- ユニットオブワークパターン
- 依存性注入
- 設定の外部化

## 主要リポジトリ
- **UserRepository**: ユーザーデータのアクセス
- **TripRepository**: 旅行データのアクセス
- **PaymentRepository**: 支払いデータのアクセス
- **SettlementRepository**: 精算データのアクセス

## データベース
- **DbContext**: TravelPaymentDbContext
- **マイグレーション**: EF Core Migrations
- **接続文字列**: appsettings.json で管理

## 外部サービス
- **EmailService**: メール送信サービス
- **FileStorageService**: ファイル保存サービス
- **NotificationService**: 通知サービス

## 設定管理
- **DatabaseSettings**: データベース設定
- **JwtSettings**: JWT設定
- **CorsSettings**: CORS設定

## テスト
```bash
cd tests/Infrastructure
dotnet test
```
