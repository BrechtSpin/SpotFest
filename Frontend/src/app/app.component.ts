import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { HappeningFormComponent } from './happening/happening-form.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, HappeningFormComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Frontend';
}
