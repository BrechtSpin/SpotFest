import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navigation-bar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './navigation-bar.component.html',
  styles: [`
    .navbar-nav .nav-link {
      font-weight: 500;
      text-transform: uppercase;
    }
  `]
})
export class NavigationBarComponent { }
