import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment as env } from '../../environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http: HttpClient = inject(HttpClient);

  login(model: any) {
    return this.http.post<User>(`${ env.apiUrl }/auth/login`, model)
  }

  register(model: any) {
    return this.http.post<User>(`${ env.apiUrl }/auth/register`, model)
  }
}
