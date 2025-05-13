import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NavigationBarComponent } from '@components/navigation-bar/navigation-bar.component'
import { HappeningSidebarComponent } from '@components/happening-sidebar/happening-sidebar.component'

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,
    NavigationBarComponent,
    HappeningSidebarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Frontend';
}
