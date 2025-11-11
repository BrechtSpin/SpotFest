import { Component, signal, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'app-user-status',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './user-status.component.html'
})
export class UserStatusComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);

  isAuthenticated = toSignal(this.authService.isAuthenticated$, { initialValue: false });
  currentUser = toSignal(this.authService.spotFestUser$, { initialValue: null });

  showDropdown = signal(false);
  isLoading = signal(false);

  ngOnInit(): void {
    this.authService.init();
  }

  onLogout(): void {
    this.isLoading.set(true);

    this.authService.logout().subscribe({
      next: () => this.router.navigate(['']),
      error: (error) => {
        console.error('Logout failed:', error);
        this.isLoading.set(false);
      },
      complete: () => this.isLoading.set(false)
    });
  }
}
