import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

import { HappeningSidebarComponent } from'@components/happening-sidebar/happening-sidebar.component'
import { HappeningCreateFormComponent } from '@components/happening-create-form/happening-create-form.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, HappeningSidebarComponent, HappeningCreateFormComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Frontend';
}
