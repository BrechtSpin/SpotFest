import { Component, inject, Signal, signal, resource, computed} from '@angular/core';
import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { fromEvent, lastValueFrom } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { HappeningService } from '@services/happening.service';
import { HappeningFull } from '@models/happening-full';
import { ArtistSummaryComponent } from '@components/artist-summary/artist-summary.component';
import { NotFound404Component } from '../404/404-not-found';

@Component({
  selector: 'app-happening-detail',
  standalone: true,
  imports: [CommonModule, NotFound404Component, ArtistSummaryComponent],
  templateUrl: './happening-detail.component.html',
  styleUrls: ['./happening-detail.component.css'],
})
export class HappeningDetailComponent {
  private route = inject(ActivatedRoute);
  private happeningService = inject(HappeningService);

  slug: Signal<string | null> = toSignal(
    this.route.paramMap.pipe(
      map(params => params.get('slug'))
    ),
    { initialValue: null }
  );

  happeningResource = resource<HappeningFull, string | null>({
    request: () => this.slug(),
    loader: ({ request, abortSignal }) => {
      const obs$ = this.happeningService.getHappeningFull(request).pipe(
        takeUntil(fromEvent(abortSignal, 'abort'))
      );
      return lastValueFrom(obs$);
    }
  });

  happening = computed<HappeningFull>(() => {
    const val = this.happeningResource.value()
    if (val == null) { throw new Error }
    return val
  });
  loading = computed(() => this.happeningResource.isLoading());
  error = computed(() => this.happeningResource.error());
}
