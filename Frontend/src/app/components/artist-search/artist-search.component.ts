import { Component, effect, signal, inject, Output, EventEmitter, OnInit } from '@angular/core';
import { ArtistService } from '@services/artist.service'
import { HappeningArtist } from '@models/happening-artist';
import { FormArray, FormBuilder, FormGroup, FormGroupDirective, FormRecord, Validators } from '@angular/forms';
import { ArtistSearchInputComponent } from './artist-search-input.component';

@Component({
  selector: 'app-artist-search',
  standalone: true,
  imports: [ArtistSearchInputComponent],
  templateUrl: './artist-search.component.html'
})
export class ArtistSearchComponent {
  private fb = inject(FormBuilder);
  private parent = inject(FormGroupDirective)


  get happeningArtists(): FormArray{
  return this.parent.form.get('happeningArtists') as FormArray;
}

addArtist() {
  this.happeningArtists.push(this.fb.group({
    name: [''],
    spotifyId: ['', Validators.required]
  }));
}

onArtistRemove(index: number) {
  this.happeningArtists.removeAt(index)
}

onArtistSelected(artist: HappeningArtist, index: number) {
  this.happeningArtists.at(index).patchValue({
    spotifyId: artist.spotifyId,
    name: artist.name
  })
}
}
