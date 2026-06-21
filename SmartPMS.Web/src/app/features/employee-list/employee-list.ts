import { Component } from '@angular/core';
import { EmployeeResponse, EmployeeService } from '../employees/employees';
import { AuthService } from '../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-employee-list',
  imports: [CommonModule, RouterLink, DatePipe],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css',
})
export class EmployeeList {
employees: EmployeeResponse[] = [];
  errorMessage = '';
  isLoading = false;

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.errorMessage = '';
    this.isLoading = true;

    this.employeeService.getEmployees().subscribe({
      next: (response) => {
        this.isLoading = false;

        if (response.success) {
          this.employees = response.data;
        } else {
          this.errorMessage = response.message;
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message ?? 'Unable to load employees.';
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
