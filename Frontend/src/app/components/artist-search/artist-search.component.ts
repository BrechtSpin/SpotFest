import { Component, effect, signal, inject, Output, EventEmitter, OnInit } from '@angular/core';
import { ArtistService } from '@services/artist.service'
import { HappeningArtist } from '@models/happening-artist';

@Component({
  selector: 'app-artist-search',
  standalone: true,
  templateUrl: './artist-search.component.html'
})
export class ArtistSearchComponent {
  private artistService = inject(ArtistService);
  constructor() {
    effect(() => {  //Search
      const query = this.searchName();
      if (query.length > 2) {
        this.artistService.getArtistByNameSpotify(query).subscribe((results) => {
          console.log('Raw results from getArtistByNameSpotify:', results);
          this.artists.set(results.slice(0, 5));
        });
      } else {
        this.artists.set([]);
      }
    }
    );
  }

  searchName = signal('');

  artists = signal<HappeningArtist[]>([]);
  selectedArtist = signal<HappeningArtist | null>(null);
  error = signal<string | null>(null);

  @Output() artistSelected = new EventEmitter<HappeningArtist>();

  onInput(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchName.set(input.value);
  }

  selectArtist(artist: any) {
    this.selectedArtist.set(artist);
    this.artistSelected.emit(artist);
  }

  @Output() deleteMe = new EventEmitter<void>();

  onDeleteClick() {
    this.deleteMe.emit();
  }
}
