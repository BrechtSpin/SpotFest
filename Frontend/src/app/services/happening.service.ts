import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Happening } from '../models/happening'

@Injectable({
  providedIn: 'root'
})
export class HappeningService {
  private apiUrl = '/happening';

  constructor(private http: HttpClient) { }

  createHappening(happening: Happening) {
    return this.http.post(this.apiUrl, happening);
  }
}
