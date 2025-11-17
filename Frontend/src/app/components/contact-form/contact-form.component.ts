import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InfoService } from '@services/info.service';

@Component({
  selector: 'app-contact-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './contact-form.component.html',
})
export class ContactFormComponent {
  private fb = inject(FormBuilder);
  private infoService = inject(InfoService);

  contactForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
  });
  submitted = false;
  errorMessage = '';

  isInvalid(controlName: string) {
    const ctrl = this.contactForm.get(controlName)!;
    return ctrl.invalid && (ctrl.touched || this.submitted);
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.contactForm.invalid) {
      this.errorMessage = 'Please fill in all the fields.';
      return;
    } else {
      this.infoService.postContactForm(this.contactForm.value).subscribe({
        next: () => (this.submitted = true),
        error: () => (this.errorMessage = 'Submission failed, try again.'),
      });
    }
  }
}
