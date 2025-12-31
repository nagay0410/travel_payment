/**
 * バックエンドの共通APIレスポンス構造に対応するジェネリック型。
 * 全てのAPIレスポンスはこの構造に従う。
 */
export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
}
