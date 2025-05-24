import { Component, computed, inject, OnInit, resource, Signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router'

import { TimelineGraphComponent } from '@components/graph/graph.component';
import { ArtistService } from '@services/artist.service';
import { HappeningService } from '@services/happening.service';
import { ArtistWithMetrics } from '@models/artist-with-metrics';
import { toSignal } from '@angular/core/rxjs-interop';
import { firstValueFrom, map, Observable, of } from 'rxjs';

@Component({
  selector: 'app-artist-details',
  standalone: true,
  imports: [CommonModule, TimelineGraphComponent],
  templateUrl: './artist-details.component.html',
  styleUrl: './artist-details.component.css',
})
export class ArtistDetailsComponent {
  happeningService = inject(HappeningService);
  artistService = inject(ArtistService);
  route = inject(ActivatedRoute);
  router = inject(ActivatedRoute);

  artistRouteGuid: Signal<string | null> = toSignal(
    this.route.paramMap.pipe(
      map(params => params.get('guid'))
    ),
    { initialValue: null }
  );
  artistRouteName: Signal<string | null> = toSignal(
    this.route.paramMap.pipe(
      map(params => params.get('name'))
    ),
    { initialValue: null }
  );

  artistResource = resource<ArtistWithMetrics | null, string | null>({
    request: () => this.artistRouteGuid(),
    loader: async ({ request }) => {
      if (!request) {
        return null;
      }
      const [artist, happenings] = await Promise.all([
        firstValueFrom(
          this.artistService.getArtistWithMetricsByGuid(request, this.artistRouteName() ?? '')
        ),
        firstValueFrom(
          this.happeningService.getHappeningsOfArtist(request)
        )
      ]);
      return { ...artist, artistHappenings: happenings };
    },
    defaultValue: null,
  });

  artist = computed(() => this.artistResource.value());

  dataSets = computed(() => {
    const artist = this.artist();
    if (!artist) { return [] }
    return [
      {
        label: 'Followers',
        metrics: artist.artistMetrics.map(m => ({
          date: m.date,
          value: m.followers
        })),
        happenings: artist.artistHappenings
      },
      {
        label: 'Popularity',
        metrics: artist.artistMetrics.map(m => ({
          date: m.date,
          value: m.popularity
        })),
        happenings: artist.artistHappenings
      },
      {
        label: 'Listeners',
        metrics: artist.artistMetrics.map(m => ({
          date: m.date,
          value: m.listeners
        })),
        happenings: artist.artistHappenings
      }
    ];
  })
}
