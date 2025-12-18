import { Component, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

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
  private router = inject(Router)

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

  get happeningArtists(): FormArray  {
    return this.happeningForm.get('happeningArtists') as FormArray;
  }

  submit() {
    if (this.happeningForm.valid) {
      const rawValue = this.happeningForm.value;
      const sanitized = {
        ...rawValue,
        endDate: rawValue.endDate || null,
      };
      this.happeningService.createHappening(sanitized).subscribe({
        next: (res) => this.router.navigate(['/happening', res.slug]),
        error: (err) => console.error('Error:', err)
      });
    } else {
      this.happeningForm.markAllAsTouched();
      console.warn('Form is invalid:', this.happeningForm.value);
    }
  }
}
