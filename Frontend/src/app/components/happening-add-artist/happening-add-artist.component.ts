import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ArtistSearchComponent } from '@components/artist-search/artist-search.component';
import { HappeningService } from '@services/happening.service';

@Component({
  selector: 'app-happening-add-artist',
  standalone: true,
  imports: [CommonModule, ArtistSearchComponent, ReactiveFormsModule],
  templateUrl: './happening-add-artist.component.html'
})
export class HappeningAddArtistComponent {
  private fb = inject(FormBuilder);
  private happeningService = inject(HappeningService);
  private router = inject(Router);

  @Input() guid!: string;
  @Output() refreshRequested = new EventEmitter<void>();

  requestRefresh() {
    this.refreshRequested.emit();
  }

  loading = true;

  happeningForm: FormGroup = this.fb.group({
    guid: [''],
    happeningArtists: this.fb.array([])
  });

  ngOnInit() {
    if (this.guid) {
      this.happeningForm.patchValue({ guid: this.guid });
    }
    this.loading = false
  }

  submit() {
    if (this.happeningForm.valid) {
      this.loading = true;
      this.happeningService.updateHappeningWithArtists(this.happeningForm.value).subscribe({
        next: () => {
          setTimeout(() => {
            this.loading = false;
            this.refreshRequested.emit();
            this.router.navigate([this.router.url]);
          }, 500);
        },
        error: (err) => console.error('Error:', err)
      });
    } else {
      this.happeningForm.markAllAsTouched();
      console.warn('Form is invalid:', this.happeningForm.value);
    }
  }
}
