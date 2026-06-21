import { Component } from '@angular/core';
import { AuthService, LoginRequest } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
model: LoginRequest = {
    email: '',
    password: ''
  };

  errorMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {
    this.errorMessage = '';

    if (!this.model.email || !this.model.password) {
      this.errorMessage = 'Email and password are required.';
      return;
    }

    this.isLoading = true;

    this.authService.login(this.model).subscribe({
      next: (response) => {
        this.isLoading = false;

        if (response.success) {
          this.authService.saveLoginData(response);
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = response.message;
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message ?? 'Login failed. Please try again.';
      }
    });
  }

}
