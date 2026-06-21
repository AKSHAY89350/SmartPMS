import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface EmployeeResponse {
  id: number;
  employeeCode: string;
  fullName: string;
  email: string;
  department: string;
  designation: string;
  level: string;
  joiningDate: string;
  isPaperPMS: boolean;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly apiUrl = environment.apiBaseUrl + '/api/employees';

  constructor(private http: HttpClient) {}

  getEmployees(): Observable<ApiResponse<EmployeeResponse[]>> {
    return this.http.get<ApiResponse<EmployeeResponse[]>>(this.apiUrl);
  }

  getEmployeeByCode(employeeCode: string): Observable<ApiResponse<EmployeeResponse>> {
    return this.http.get<ApiResponse<EmployeeResponse>>(`${this.apiUrl}/by-code/${employeeCode}`);
  }
}