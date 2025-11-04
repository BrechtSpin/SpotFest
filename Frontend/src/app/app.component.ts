import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NavigationBarComponent } from '@components/navigation-bar/navigation-bar.component'
import { HappeningSidebarComponent } from '@components/happening-sidebar/happening-sidebar.component'
import { FooterComponent } from '@components/footer/footer.component'

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,
    NavigationBarComponent,
    HappeningSidebarComponent,
    FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'SpotFest';
}
