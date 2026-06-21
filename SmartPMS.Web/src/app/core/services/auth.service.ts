import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  employeeCode: string;
  fullName: string;
  email: string;
  password: string;
  role: string;
}

export interface AuthResponse {
  success: boolean;
  message: string;
  data: {
    userId: number;
    employeeCode: string;
    fullName: string;
    email: string;
    role: string;
    token: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = environment.apiBaseUrl + '/gateway/auth';

  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request);
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request);
  }

  saveLoginData(response: AuthResponse): void {
    localStorage.setItem('token', response.data.token);
    localStorage.setItem('userId', response.data.userId.toString());
    localStorage.setItem('employeeCode', response.data.employeeCode);
    localStorage.setItem('fullName', response.data.fullName);
    localStorage.setItem('email', response.data.email);
    localStorage.setItem('role', response.data.role);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getFullName(): string {
    return localStorage.getItem('fullName') ?? '';
  }
  getEmail(): string {
    return localStorage.getItem('email') ?? '';
  }
  getRole(): string {
    return localStorage.getItem('role') ?? '';
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
  getEmployeeCode(): string {
    return localStorage.getItem('employeeCode') ?? '';
  }
  logout(): void {
    localStorage.clear();
  }
}
