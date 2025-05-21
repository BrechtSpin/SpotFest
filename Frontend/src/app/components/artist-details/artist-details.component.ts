import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router'

import { TimelineGraphComponent } from '@components/graph/graph.component';
import { ArtistService } from '@services/artist.service';
import { ArtistWithMetrics } from '@models/artist-with-metrics';
import { ArtistMetric } from '@models/artist-metric';

@Component({
  selector: 'app-artist-details',
  standalone: true,
  imports: [CommonModule, TimelineGraphComponent],
  templateUrl: './artist-details.component.html',
  styleUrl: './artist-details.component.css',
})
export class ArtistDetailsComponent implements OnInit {
  artistName = '';
  artistPicture = '';
  metrics: ArtistMetric[] = [];
  dataSets: { label: string; data: { date: string; value: number }[] }[] = [];

  constructor(
    private artistService: ArtistService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const artistGuid = this.route.snapshot.paramMap.get('guid');
    const artistName = this.route.snapshot.paramMap.get('name');
    if (artistGuid === null) { return }


    this.artistService.getArtistWithMetricsByGuid(artistGuid, artistName).subscribe({
      next: (res: ArtistWithMetrics) => {
        this.artistName = res.name;
        this.artistPicture = res.pictureMediumUrl;
        this.metrics = res.artistMetrics;

        this.dataSets = [
          {
            label: 'Followers',
            data: this.metrics.map(m => ({
              date: m.date,
              value: m.followers
            }))
          },
          {
            label: 'Popularity',
            data: this.metrics.map(m => ({
              date: m.date,
              value: m.popularity
            }))
          },
          {
            label: 'Listeners',
            data: this.metrics.map(m => ({
              date: m.date,
              value: m.listeners
            }))
          }
        ];
      },
      error: err =>
        console.error('Fout bij ophalen artiest-metrics', err)
    });
  }
}
