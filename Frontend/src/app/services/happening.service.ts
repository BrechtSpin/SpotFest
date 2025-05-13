import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@env/environment';
import { CreateHappening } from '@models/create-happening'
import { HappeningSummary } from '@models/happening-summary';
import { HappeningFull } from '@models/happening-full';

@Injectable({
  providedIn: 'root'
})
export class HappeningService {
  private Url = `${environment.apiHappeningUrl}`;

  constructor(private http: HttpClient) { }

  getHappeningFull(slug: string | null): Observable<HappeningFull> {
    if (!slug) {
      return this.http.get<HappeningFull>(`${this.Url}`)
    } else {
      return this.http.get<HappeningFull>(`${this.Url}/${slug}`)
    }
  }

  createHappening(createHappening: CreateHappening) {
    return this.http.post(`${this.Url}/new`, createHappening);
  }

  getCurrentAndUpcomingHappenings(): Observable<HappeningSummary[]> {
    return this.http.get<HappeningSummary[]>(`${this.Url}/current`)
  }
}
