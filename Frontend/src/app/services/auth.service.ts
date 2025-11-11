import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { environment } from '@env/environment';
import { AuthResponse, LoginRequest, RegisterRequest } from '@models/auth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);

  private authUrl = `${environment.apiAuthUrl}`;

  private spotFestUserSubject = new BehaviorSubject<AuthResponse | null>(null);
  public spotFestUser$ = this.spotFestUserSubject.asObservable();
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  init(): void {
    this.checkAuthStatus(); //fetch status between pageloads
  }

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.authUrl}/register`, data);
  }

  login(loginRequest: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.authUrl}/login`, loginRequest, {
      withCredentials: true
    }).pipe(
      tap(response => {
        this.spotFestUserSubject.next(response);
        this.isAuthenticatedSubject.next(true);
      })
    );
  }

  logout(): Observable<any> {
    return this.http.post(`${this.authUrl}/logout`, {}, {
      withCredentials: true
    }).pipe(
      tap(() => {
        this.spotFestUserSubject.next(null);
        this.isAuthenticatedSubject.next(false);
      })
    );
  }

  private checkAuthStatus(): void {
    this.http.get<AuthResponse>(`${this.authUrl}/me`, {
      withCredentials: true
    }).subscribe({
      next: (user) => {
        this.spotFestUserSubject.next(user);
        this.isAuthenticatedSubject.next(true);
      },
      error: () => {
        this.spotFestUserSubject.next(null);
        this.isAuthenticatedSubject.next(false);
      }
    });
  }
}
