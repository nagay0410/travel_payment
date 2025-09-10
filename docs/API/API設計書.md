# 旅行支払い精算アプリ API設計書

## 1. 概要

### 1.1 目的
旅行支払い精算アプリのバックエンドAPIの仕様を定義する

### 1.2 技術スタック
- **フレームワーク**: ASP.NET Core 8.0
- **認証**: JWT (JSON Web Token)
- **データベース**: SQL Server / PostgreSQL
- **ORM**: Entity Framework Core 8.0
- **API ドキュメント**: Swagger/OpenAPI 3.0

## 2. API エンドポイント設計

### 2.1 認証・認可

#### 2.1.1 ユーザー登録
```
POST /api/auth/register
Content-Type: application/json

Request Body:
{
  "username": "string",
  "email": "string",
  "password": "string",
  "confirmPassword": "string"
}

Response:
{
  "success": true,
  "message": "ユーザー登録が完了しました",
  "data": {
    "userId": "guid",
    "username": "string",
    "email": "string"
  }
}
```

#### 2.1.2 ユーザーログイン
```
POST /api/auth/login
Content-Type: application/json

Request Body:
{
  "email": "string",
  "password": "string"
}

Response:
{
  "success": true,
  "message": "ログインが完了しました",
  "data": {
    "token": "string",
    "refreshToken": "string",
    "expiresIn": "number",
    "user": {
      "userId": "guid",
      "username": "string",
      "email": "string"
    }
  }
}
```

#### 2.1.3 トークン更新
```
POST /api/auth/refresh
Authorization: Bearer {refreshToken}

Response:
{
  "success": true,
  "data": {
    "token": "string",
    "refreshToken": "string",
    "expiresIn": "number"
  }
}
```

### 2.2 旅行管理

#### 2.2.1 旅行一覧取得
```
GET /api/trips
Authorization: Bearer {token}
Query Parameters:
  - page: int (default: 1)
  - pageSize: int (default: 10)
  - status: string (optional: "Planning", "Active", "Completed")
  - search: string (optional)

Response:
{
  "success": true,
  "data": {
    "trips": [
      {
        "tripId": "guid",
        "tripName": "string",
        "description": "string",
        "startDate": "date",
        "endDate": "date",
        "budget": "decimal",
        "status": "string",
        "memberCount": "int",
        "totalPayments": "decimal",
        "createdBy": "string",
        "createdAt": "datetime"
      }
    ],
    "pagination": {
      "page": "int",
      "pageSize": "int",
      "totalCount": "int",
      "totalPages": "int"
    }
  }
}
```

#### 2.2.2 旅行詳細取得
```
GET /api/trips/{tripId}
Authorization: Bearer {token}

Response:
{
  "success": true,
  "data": {
    "tripId": "guid",
    "tripName": "string",
    "description": "string",
    "startDate": "date",
    "endDate": "date",
    "budget": "decimal",
    "status": "string",
    "members": [
      {
        "userId": "guid",
        "username": "string",
        "role": "string",
        "joinedAt": "datetime"
      }
    ],
    "payments": [
      {
        "paymentId": "guid",
        "amount": "decimal",
        "description": "string",
        "category": "string",
        "paidBy": "string",
        "paymentDate": "date"
      }
    ],
    "settlements": [
      {
        "settlementId": "guid",
        "fromUser": "string",
        "toUser": "string",
        "amount": "decimal",
        "status": "string"
      }
    ]
  }
}
```

#### 2.2.3 旅行作成
```
POST /api/trips
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
  "tripName": "string",
  "description": "string",
  "startDate": "date",
  "endDate": "date",
  "budget": "decimal",
  "memberEmails": ["string"]
}

Response:
{
  "success": true,
  "message": "旅行が作成されました",
  "data": {
    "tripId": "guid",
    "tripName": "string"
  }
}
```

#### 2.2.4 旅行更新
```
PUT /api/trips/{tripId}
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
  "tripName": "string",
  "description": "string",
  "startDate": "date",
  "endDate": "date",
  "budget": "decimal",
  "status": "string"
}

Response:
{
  "success": true,
  "message": "旅行情報が更新されました"
}
```

#### 2.2.5 旅行削除
```
DELETE /api/trips/{tripId}
Authorization: Bearer {token}

Response:
{
  "success": true,
  "message": "旅行が削除されました"
}
```

### 2.3 支払い管理

#### 2.3.1 支払い記録作成
```
POST /api/trips/{tripId}/payments
Authorization: Bearer {token}
Content-Type: multipart/form-data

Form Data:
  - amount: decimal
  - description: string
  - categoryId: guid
  - paymentDate: date
  - receiptImage: file (optional)

Response:
{
  "success": true,
  "message": "支払い記録が作成されました",
  "data": {
    "paymentId": "guid",
    "amount": "decimal",
    "description": "string"
  }
}
```

#### 2.3.2 支払い記録更新
```
PUT /api/payments/{paymentId}
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
  "amount": "decimal",
  "description": "string",
  "categoryId": "guid",
  "paymentDate": "date"
}

Response:
{
  "success": true,
  "message": "支払い記録が更新されました"
}
```

#### 2.3.3 支払い記録削除
```
DELETE /api/payments/{paymentId}
Authorization: Bearer {token}

Response:
{
  "success": true,
  "message": "支払い記録が削除されました"
}
```

### 2.4 精算管理

#### 2.4.1 精算計算
```
GET /api/trips/{tripId}/settlements/calculate
Authorization: Bearer {token}

Response:
{
  "success": true,
  "data": {
    "totalAmount": "decimal",
    "perPersonAmount": "decimal",
    "settlements": [
      {
        "fromUser": "string",
        "toUser": "string",
        "amount": "decimal"
      }
    ]
  }
}
```

#### 2.4.2 精算完了
```
POST /api/trips/{tripId}/settlements
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
  "fromUserId": "guid",
  "toUserId": "guid",
  "amount": "decimal",
  "settlementMethod": "string"
}

Response:
{
  "success": true,
  "message": "精算が完了しました",
  "data": {
    "settlementId": "guid"
  }
}
```

### 2.5 カテゴリ管理

#### 2.5.1 カテゴリ一覧取得
```
GET /api/categories
Authorization: Bearer {token}

Response:
{
  "success": true,
  "data": [
    {
      "categoryId": "guid",
      "categoryName": "string",
      "description": "string",
      "icon": "string"
    }
  ]
}
```

## 3. エラーハンドリング

### 3.1 エラーレスポンス形式
```json
{
  "success": false,
  "message": "エラーメッセージ",
  "errors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": "number"
}
```

### 3.2 HTTP ステータスコード
- 200: OK
- 201: Created
- 400: Bad Request
- 401: Unauthorized
- 403: Forbidden
- 404: Not Found
- 422: Unprocessable Entity
- 500: Internal Server Error

### 3.3 バリデーションエラー
```json
{
  "success": false,
  "message": "バリデーションエラーが発生しました",
  "errors": [
    {
      "field": "email",
      "message": "メールアドレスの形式が正しくありません"
    },
    {
      "field": "password",
      "message": "パスワードは8文字以上である必要があります"
    }
  ],
  "statusCode": 422
}
```

## 4. 認証・認可

### 4.1 JWT トークン
- **アクセストークン**: 有効期限 1時間
- **リフレッシュトークン**: 有効期限 7日
- **アルゴリズム**: HS256

### 4.2 認可レベル
- **旅行管理者**: 旅行の編集・削除、メンバー管理
- **一般参加者**: 支払い記録の作成・編集・削除
- **閲覧者**: 旅行情報の閲覧のみ

## 5. レート制限

### 5.1 制限設定
- **認証エンドポイント**: 5回/分
- **一般API**: 100回/分
- **ファイルアップロード**: 10回/分

## 6. ログ・監視

### 6.1 ログ出力
- リクエスト・レスポンスログ
- エラーログ
- 認証ログ
- パフォーマンスログ

### 6.2 監視項目
- API レスポンス時間
- エラー率
- 同時接続数
- データベース接続状況

---

*このAPI設計書は開発の進行に応じて更新される場合があります。*
