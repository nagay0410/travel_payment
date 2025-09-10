# TravelPayment.Application

## 概要
旅行支払い精算アプリのアプリケーション層プロジェクトです。

## 役割
- ビジネスロジックの実装
- ユースケースの実装
- ドメインオブジェクトの操作
- 外部サービスとの連携調整

## 主要コンポーネント
- **Services**: ビジネスロジックの実装
- **DTOs**: データ転送オブジェクト
- **Validators**: 入力値検証
- **Mappers**: オブジェクト間の変換
- **Interfaces**: サービス契約の定義

## 技術スタック
- .NET 8.0
- FluentValidation
- AutoMapper
- MediatR（将来的な拡張用）

## 依存関係
- TravelPayment.Domain

## 設計原則
- 単一責任の原則
- 依存性注入
- インターフェース分離
- テスト可能性の確保

## 主要サービス
- **TripService**: 旅行管理のビジネスロジック
- **PaymentService**: 支払い管理のビジネスロジック
- **SettlementService**: 精算計算のビジネスロジック
- **UserService**: ユーザー管理のビジネスロジック

## テスト
```bash
cd tests/Application
dotnet test
```
