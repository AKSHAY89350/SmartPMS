import { Component } from '@angular/core';
import { AuthService, RegisterRequest } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  model: RegisterRequest = {
    employeeCode: '',
    fullName: '',
    email: '',
    password: '',
    role: 'Employee'
  };

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  register(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.model.employeeCode || !this.model.fullName || !this.model.email || !this.model.password) {
      this.errorMessage = 'All required fields must be filled.';
      return;
    }

    this.isLoading = true;

    this.authService.register(this.model).subscribe({
      next: (response) => {
        this.isLoading = false;

        if (response.success) {
          this.authService.saveLoginData(response);
          this.successMessage = 'Registration successful.';
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = response.message;
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message ?? 'Registration failed. Please try again.';
      }
    });
  }
}

