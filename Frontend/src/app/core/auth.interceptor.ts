import { inject, Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Route, Router } from '@angular/router';
import { AuthService } from '@services/auth.service';

@Injectable({ providedIn: 'root' })

//intercept all httprequests and makes sure the http-cookie is sent with it
export class AuthInterceptor implements HttpInterceptor {
  private authService = inject(AuthService);
  private router= inject(Router);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const cloned = req.clone({
      withCredentials: true
    });

    return next.handle(cloned).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && !req.url.includes('/api/auth/me')) {
          this.authService.logout();
          this.router.navigate(['/login'], { queryParams: { message: 'login-required' } });
        }
        return throwError(() => error);
      })
    );
  }
}
