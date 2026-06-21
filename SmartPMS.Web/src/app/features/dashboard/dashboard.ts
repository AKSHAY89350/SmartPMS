import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  fullName = '';
  employeeCode = '';
  email = '';
  role = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.fullName = this.authService.getFullName();
    this.employeeCode = this.authService.getEmployeeCode();
    this.email = this.authService.getEmail();
    this.role = this.authService.getRole();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}
