# TravelPayment.Shared

## 概要
旅行支払い精算アプリの共通ライブラリプロジェクトです。

## 役割
- 共通のユーティリティクラス
- 共通の拡張メソッド
- 共通の定数・列挙型
- 共通の例外クラス

## 主要コンポーネント
- **Utilities**: 共通ユーティリティ
- **Extensions**: 拡張メソッド
- **Constants**: 定数・列挙型
- **Exceptions**: 共通例外
- **Helpers**: ヘルパークラス

## 技術スタック
- .NET 8.0
- 標準ライブラリ

## 依存関係
- なし（他のプロジェクトから参照される）

## 設計原則
- 再利用性
- 単純性
- 保守性
- テスト可能性

## 主要ユーティリティ
- **DateTimeHelper**: 日付・時刻の操作
- **StringHelper**: 文字列の操作
- **ValidationHelper**: 共通バリデーション
- **SecurityHelper**: セキュリティ関連

## 拡張メソッド
- **EnumerableExtensions**: コレクション操作
- **StringExtensions**: 文字列操作
- **DateTimeExtensions**: 日付操作

## 定数・列挙型
- **TripStatus**: 旅行ステータス
- **TripRole**: 旅行参加者役割
- **PaymentStatus**: 支払いステータス
- **SettlementStatus**: 精算ステータス

## 共通例外
- **TravelPaymentException**: 基底例外クラス
- **ValidationException**: バリデーション例外
- **NotFoundException**: データ未発見例外

## 使用例
```csharp
// 他のプロジェクトで参照
using TravelPayment.Shared.Utilities;
using TravelPayment.Shared.Extensions;
using TravelPayment.Shared.Constants;
```
