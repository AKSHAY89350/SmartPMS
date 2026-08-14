import { Component, ChangeDetectorRef } from '@angular/core';
import { AuthService, RegisterRequest } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { finalize } from 'rxjs/operators';

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
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  register(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.model.employeeCode || !this.model.fullName || !this.model.email || !this.model.password) {
      this.errorMessage = 'All required fields must be filled.';
      this.cdr.detectChanges();
      return;
    }

    this.isLoading = true;
    this.cdr.detectChanges();

    this.authService.register(this.model)
      .pipe(
        // finalize ALWAYS runs — success or error — guarantees button never gets stuck
        finalize(() => {
          this.isLoading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (response) => {
          if (response.success) {
            this.authService.saveLoginData(response);
            this.successMessage = response.message || 'Registration successful.';
            this.cdr.detectChanges();
            this.router.navigate(['/dashboard']);
          } else {
            // Backend returned 200 but success=false
            this.errorMessage = response.message || 'Registration failed. Please try again.';
            this.cdr.detectChanges();
          }
        },
        error: (httpError) => {
          // Backend returns 400 BadRequest with body: { success, message, data }
          // httpError.error contains the parsed JSON body
          const body = httpError?.error;
          this.errorMessage =
            body?.message ||     // camelCase — ASP.NET default JSON serialization
            body?.Message ||     // PascalCase fallback
            body?.title   ||    // ASP.NET validation ProblemDetails
            httpError?.message ||
            'Registration failed. Please try again.';
          this.cdr.detectChanges();
        }
      });
  }
}
