import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment as env } from '../../environments/environment';
import { User } from '../models/user';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http: HttpClient = inject(HttpClient);

  login(model: any) {
    return this.http.post<User>(`${ env.apiUrl }/auth/login`, model, { withCredentials: true }).pipe(
      map(user => {
        if (user) {
          this.saveToken(user.accessToken);
        }
      })
    );
  }

  saveToken(token: string): void {
    localStorage.setItem('accessToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  refreshToken(authUser: {userName: string}, token: string) {
    //todo: doesn't work yet
    return this.http.post(`${ env.apiUrl }/auth/refresh`, { }, { withCredentials: true })
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem('accessToken');
  }

  register(model: any) {
    return this.http.post<User>(`${ env.apiUrl }/auth/register`, model);
  }
}
