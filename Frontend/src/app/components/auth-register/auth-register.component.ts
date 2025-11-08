import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'app-auth-register',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule, RouterModule],
  templateUrl: './auth-register.component.html'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router)

  registerForm = this.fb.nonNullable.group({
    firstName: [],
    lastName: [],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(200)]],
    confirmPassword: ['', [Validators.required]]
  }, {
    validators: this.passwordMatchValidator
  });

  email() { return this.registerForm.get('email'); }
  password() { return this.registerForm.get('password'); }
  confirmpassword() { return this.registerForm.get('confirmPassword'); }
  isLoading = false;
  errorMessage = '';

  passwordMatchValidator(group: AbstractControl) {
    const password = group.get('password')?.value;
    const confirmpassword = group.get('confirmPassword')?.value;
    return password === confirmpassword ? null : {mismatch : true}
  }
  passwordsMatch(): boolean {
    const password = this.registerForm.get('password')?.value;
    const confirmPassword = this.registerForm.get('confirmPassword')?.value;
    return password === confirmPassword;
  }

  isInvalid(controlName: string) {
    const ctrl = this.registerForm.get(controlName)!;
    return ctrl.invalid && ctrl.touched;
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const { confirmPassword, ...payload } = this.registerForm.getRawValue();

    this.authService.postRegister(payload).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Registration failed. Please try again.';
        this.isLoading = false;
      },
    });
  }
}
