import { Component, computed, effect, inject, resource, signal } from '@angular/core';
import { ArtistService } from '@services/artist.service';
import { ArtistSummary } from '@models/artist-summary';
import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

export interface SearchParameters {
  Query: string
  Index: number
}

@Component({
  selector: 'app-artist-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './artist-list.component.html',
  styleUrls: ['./artist-list.component.css']
})

export class ArtistListComponent {
  private artistService = inject(ArtistService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  readonly alphabet: string[] = Array.from({ length: 26 }, (_, i) => String.fromCharCode(65 + i));
  query = signal<string>('');
  index = signal<number>(0);
  readonly itemsPerPage = 10;

  constructor() {
    // getting params in url on page load
    this.route.queryParamMap.subscribe((map) => {
      const query = map.get('query') ?? '';
      const index = Number(map.get('index') ?? '0');
      this.query.set(query);
      this.index.set(isNaN(index) ? 0 : index);
    });
    //setting params in url on change
    effect(() => {
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: {
          query: this.query(),
          index: this.index(),
        }
      });
    });
  }

  artistsResource = resource<ArtistSummary[], SearchParameters>({
    request: () => ({
      Query: this.query(),
      Index: this.index(),
    }),
    loader: async ({ request }) => {
      if (request.Query === '') return [];
      return await lastValueFrom(
        this.artistService.GetArtistsBySearch(
          request.Query,
          request.Index)
      )
    },
    defaultValue: [] as ArtistSummary[],
  })

  artists = computed<ArtistSummary[]>(() => this.artistsResource.value());

  isNextPage = computed(() => this.artistsResource.value().length === this.itemsPerPage)

  selectLetter(letter: string) {
    this.query.set(letter);
    this.index.set(0);
  }
  nextPage() {
    this.index.set(this.index() + this.itemsPerPage)
  }
  prevPage() {
    let id = this.index() - this.itemsPerPage;
    if (id < 0) id = 0;
    this.index.set(id);
    }
  }
