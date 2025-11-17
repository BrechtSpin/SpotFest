import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '@services/auth.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

@Component({
  selector: 'auth-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './auth-login.component.html'
})
export class LoginComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router)
  private route = inject(ActivatedRoute);

  loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(200)]]
  });
  isLoading = false;
  errorMessage = '';


  alertMessage: string | null = null;
  alertType: string = 'warning'; // Bootstrap alert type
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const msg = params['message'];

      if (msg === 'login-required') {
        this.alertType = 'warning';
        this.alertMessage = 'You must be logged in to access that page.';
      }
    });
  }

  isInvalid(controlName: string) {
    const ctrl = this.loginForm.get(controlName)!;
    return ctrl.invalid && ctrl.touched;
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this.errorMessage = 'Please fill in all the fields.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginForm.getRawValue()).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.errorMessage = error.error.error || 'Login failed. Please try again.';
        this.isLoading = false;
      },
    });
  }
}
