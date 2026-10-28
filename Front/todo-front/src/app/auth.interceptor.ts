import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const auth = inject(AuthService);
  const token = auth.token;
  const authenticatedRequest = token ? request.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : request;

  return next(authenticatedRequest).pipe(catchError(error => {
    if (!(error instanceof HttpErrorResponse) || error.status !== 401 ||
        request.url.includes('/Users/') || !auth.refreshToken) {
      return throwError(() => error);
    }

    return auth.refresh().pipe(
      switchMap(result => next(request.clone({ setHeaders: { Authorization: `Bearer ${result.token}` } }))),
      catchError(refreshError => {
        auth.clear();
        return throwError(() => refreshError);
      })
    );
  }));
};
