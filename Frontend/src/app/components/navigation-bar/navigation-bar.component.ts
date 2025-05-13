import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navigation-bar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navigation-bar.component.html',
  styles: [`
    .navbar-brand img {
      max-height: 40px;
    }
    .navbar-nav .nav-link {
      font-weight: 500;
      text-transform: uppercase;
    }
  `]
})
export class NavigationBarComponent { }
