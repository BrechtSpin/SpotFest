import { Component, effect, signal, inject, Output, EventEmitter } from '@angular/core';
import { debouncedSignal } from '@utils/debounced-signal';
import { ArtistService } from '@services/artist.service'
import { HappeningArtist } from '@models/happening-artist';

@Component({
  selector: 'app-artist-search',
  standalone: true,
  templateUrl: './artist-search.component.html'
})
export class ArtistSearchComponent {
  private artistService = inject(ArtistService);

  searchName = signal('');
  debouncedName = debouncedSignal(this.searchName, 200);

  artists = signal<HappeningArtist[]>([]);
  selectedArtist = signal<HappeningArtist | null>(null);
  error = signal<string | null>(null);

  @Output() artistSelected = new EventEmitter<HappeningArtist>();

  onInput(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchName.set(input.value);
  }

  constructor() {
    effect(() => {  //Search
      const query = this.debouncedName();
      if (query.length > 2) {
        this.artistService.getArtistByNameSpotify(query).subscribe((results) => {
          console.log('Raw results from getArtistByNameSpotify:', results);
          this.artists.set(results.slice(0, 5));
        });
      } else {
        this.artists.set([]);
      }
    },
      { allowSignalWrites: true }
    );
  }

  selectArtist(artist: any) {
    this.selectedArtist.set(artist);
    this.artistSelected.emit(artist);
  }
}
