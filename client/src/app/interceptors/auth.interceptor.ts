import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);

  let token = authService.getToken();
  if (token) {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${ token }`
      }
    })
  }

  return next(request).pipe(catchError((err: HttpErrorResponse) => {
    if (err.status == 401) {
      // authService.
    }

    return throwError(() => err);
  }));
};
