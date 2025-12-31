import { HttpInterceptorFn, HttpHandlerFn, HttpRequest } from '@angular/common/http';

/**
 * 全ての HTTP リクエストに対してログを出力するインターセプター
 */
export const loggingInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
	console.log(`Request to: ${req.url}`);
	return next(req);
};
