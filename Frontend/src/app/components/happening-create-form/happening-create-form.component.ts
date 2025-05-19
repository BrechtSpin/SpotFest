import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { HappeningService } from '@services/happening.service'
import { ArtistSearchComponent } from '@components/artist-search/artist-search.component'
import { HappeningArtist } from '@models/happening-artist'

@Component({
  selector: 'app-happening-create-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ArtistSearchComponent],
  templateUrl: './happening-create-form.component.html'
})
export class HappeningCreateFormComponent {
  private fb = inject(FormBuilder);
  private happeningService = inject(HappeningService);

  happeningForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    startDate: ['', Validators.required],
    endDate: [''],
    happeningArtists: this.fb.array([])
  });

  submitted = false;
  errorMessage = '';

  isInvalid(controlName: string) {
    const ctrl = this.happeningForm.get(controlName)!;
    return ctrl.invalid && (ctrl.touched || this.submitted);
  }

  get happeningArtists() {
    return this.happeningForm.get('happeningArtists') as FormArray;
  }

  addArtist() {
    this.happeningArtists.push(this.fb.group({
      name: [''],
      spotifyId: ['', Validators.required]
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
