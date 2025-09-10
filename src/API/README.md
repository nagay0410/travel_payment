# TravelPayment.API

## 概要
旅行支払い精算アプリのWeb APIプロジェクトです。

## 役割
- HTTP エンドポイントの提供
- リクエスト・レスポンスの処理
- 認証・認可の制御
- バリデーション
- エラーハンドリング

## 主要コンポーネント
- **Controllers**: API エンドポイントの実装
- **Middleware**: 認証、ログ、CORS等の処理
- **Filters**: アクション実行前後の処理
- **Program.cs**: アプリケーションの設定・DI設定

## 技術スタック
- ASP.NET Core 8.0
- JWT認証
- Swagger/OpenAPI
- FluentValidation

## 依存関係
- TravelPayment.Application
- TravelPayment.Infrastructure

## 起動方法
```bash
cd src/API
dotnet run
```

## API ドキュメント
起動後、以下のURLでSwagger UIにアクセスできます：
- http://localhost:5000/swagger
- https://localhost:5001/swagger
