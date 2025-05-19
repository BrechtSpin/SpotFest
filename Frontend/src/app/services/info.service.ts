import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@env/environment';
import { ContactFormModel } from '@models/contact-form-model';

@Injectable({
  providedIn: 'root'
})

export class InfoService {
  private http = inject(HttpClient);

  private infoUrl = `${environment.apiInfoUrl}`;

  postContactForm(contactForm: ContactFormModel) {
    return this.http.post(`${this.infoUrl}/contactform`, contactForm);
  }
}
