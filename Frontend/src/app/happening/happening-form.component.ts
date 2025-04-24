import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { HappeningService } from '../services/happening.service'
import { ArtistFinderComponent } from '../artist-finder/artist-finder.component'
import { HappeningArtist } from '../models/happening-artist'

@Component({
  selector: 'app-happening-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ArtistFinderComponent],
  templateUrl: './happening-form.component.html'
})
export class HappeningFormComponent {
  happeningForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private happeningService: HappeningService
  ) {
    this.happeningForm = this.fb.group({
      name: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: [''],
      happeningArtists: this.fb.array([])
    });
  }

  get happeningArtists() {
    return this.happeningForm.get('happeningArtists') as FormArray;
  }

  addArtist() {
    this.happeningArtists.push(this.fb.group({
      name: [''],
      spotifyId: ['']
    }));
  }

  removeArtist(index: number) {
    this.happeningArtists.removeAt(index);
  }

  onArtistSelected(artist: HappeningArtist, index: number) {
    console.log('Selected artist:', artist);
    this.happeningArtists.at(index).patchValue({
      spotifyId: artist.spotifyId,
      name: artist.name
    })
    console.log(this.happeningArtists.value)
  }

  submit() {
    if (this.happeningForm.valid) {
      //console.log('Form value:', this.happeningForm.value);
      const rawValue = this.happeningForm.value;
      console.log('raw form:', rawValue);
      const sanitized = {
        ...rawValue,
        endDate: rawValue.endDate || null,
      };
      console.log('Sanitized form:', sanitized);
      this.happeningService.createHappening(sanitized).subscribe({
        next: (res) => console.log('Created:', res),
        error: (err) => console.error('Error:', err)
      });
    } else {
      this.happeningForm.markAllAsTouched();
      console.warn('Form is invalid:', this.happeningForm.value);
    }
  }
}
