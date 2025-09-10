# TravelPayment.Domain

## 概要
旅行支払い精算アプリのドメイン層プロジェクトです。

## 役割
- ビジネスルールの定義
- ドメインエンティティの定義
- 値オブジェクトの定義
- ドメインサービスの定義

## 主要コンポーネント
- **Entities**: ビジネスエンティティ
- **Value Objects**: 値オブジェクト
- **Domain Services**: ドメインサービス
- **Interfaces**: ドメイン契約の定義
- **Exceptions**: ドメイン例外

## 技術スタック
- .NET 8.0
- ドメイン駆動設計（DDD）パターン

## 依存関係
- なし（他のレイヤーに依存しない）

## 設計原則
- ドメイン駆動設計
- 豊富なドメインモデル
- 不変性の活用
- ビジネスルールの集約

## 主要エンティティ
- **User**: ユーザー情報
- **Trip**: 旅行情報
- **Payment**: 支払い記録
- **Settlement**: 精算記録
- **Category**: 支払いカテゴリ

## 主要値オブジェクト
- **Money**: 金額（通貨対応）
- **DateRange**: 日付範囲
- **Email**: メールアドレス

## ドメインサービス
- **SettlementCalculator**: 精算計算ロジック
- **PaymentValidator**: 支払い検証ロジック

## テスト
```bash
cd tests/Domain
dotnet test
```
