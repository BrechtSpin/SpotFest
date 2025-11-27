import { Component, computed, effect, inject, resource, Signal, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router'

import { TimelineGraphComponent } from '@components/graph/graph.component';
import { ArtistService } from '@services/artist.service';
import { HappeningService } from '@services/happening.service';
import { ArtistWithMetrics } from '@models/artist-with-metrics';
import { toSignal } from '@angular/core/rxjs-interop';
import { firstValueFrom, map } from 'rxjs';
import slugify from 'slugify';
import { isGuid } from '@utils/is-guid'
import { NotFound404Component } from '../404/404-not-found';

@Component({
  selector: 'app-artist-details',
  standalone: true,
  imports: [CommonModule, NotFound404Component, TimelineGraphComponent],
  templateUrl: './artist-details.component.html',
  styleUrl: './artist-details.component.css',
})
export class ArtistDetailsComponent {
  happeningService = inject(HappeningService);
  artistService = inject(ArtistService);
  route = inject(ActivatedRoute);
  router = inject(Router);

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
  guidLookupError = signal<string | null>(null);

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

  loading = computed(() => this.artistResource.isLoading());
  error = computed(() => {
    return this.artistResource.error() ?? this.guidLookupError();
  });
  artist = computed(() => this.artistResource.value());

  dataSets = computed(() => {
    const artist = this.artist();
    if (!artist) { return [] }
    return [
      {
        label: 'Listeners',
        metrics: artist.artistMetrics.map(m => ({
          date: m.date,
          value: m.listeners
        })),
        happenings: artist.artistHappenings
      },
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
      }
    ];
  })

  fixUrl = effect(() => {
    const artist = this.artistResource.value();
    const guidMaybe = this.artistRouteGuid();

    if (guidMaybe === null) {
      this.router.navigate(['/']);
      return;
    }

    if (!isGuid(guidMaybe)) {
      this.artistService.getArtistGuidByName(guidMaybe).subscribe({
        next: (guidFromName: string) => {
          this.guidLookupError.set(null);
          this.router.navigate(['/artist', guidFromName]);
          },
        error: () => (this.guidLookupError.set('NotFound'))
      })
      return
    }

    if (artist && guidMaybe) {
      const name = this.artistRouteName();
      const slug = slugify(artist.name, { strict: true, remove: /[*+~.()'"!?:@]/g } )
      if (name !== slug) {
        this.router.navigate(
          ['/artist', guidMaybe, slug],
          { replaceUrl: true }
        );
      }
    }
  })
}
