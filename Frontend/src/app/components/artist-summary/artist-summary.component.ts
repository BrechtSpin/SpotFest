import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-artist-summary',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './artist-summary.component.html',
  styleUrl: './artist-summary.component.css'
})
export class ArtistSummaryComponent {
  @Input() guid!: string;
  @Input() name!: string;
  @Input() pictureUrl!: string;
}
