import { Component, OnInit } from '@angular/core';
import { HappeningDetailComponent } from'@components/happening-detail/happening-detail.component'

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [HappeningDetailComponent],
  templateUrl: './homepage.component.html',
})

export class HomepageComponent {
}
