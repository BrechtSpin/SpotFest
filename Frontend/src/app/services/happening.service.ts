import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@env/environment';
import { CreateHappening } from '@models/create-happening';
import { HappeningFull } from '@models/happening-full';
import { HappeningSummary } from '@models/happening-summary';

@Injectable({
  providedIn: 'root'
})
export class HappeningService {
  private Url = `${environment.apiHappeningUrl}`;

  constructor(
    private http: HttpClient
  ) { }

  getHappeningFull(slug: string | null): Observable<HappeningFull> {
    if (!slug) {
      return this.http.get<HappeningFull>(`${this.Url}`)
    } else {
      return this.http.get<HappeningFull>(`${this.Url}/${slug}`)
    }
  }

  createHappening(createHappening: CreateHappening): Observable<{ slug: string }> {
    return this.http.post<{ slug: string }>(`${this.Url}/new`, createHappening);
  }

  getHappeningsOfArtist(guid: string): Observable<HappeningSummary[]> {
    return this.http.get<HappeningSummary[]>(`${this.Url}/artist/${guid}`)
  }

  //deprecated
  //getCurrentAndUpcomingHappenings(): Observable<HappeningSummary[]> {
  //  return this.http.get<HappeningSummary[]>(`${this.Url}/current`)
  //}
}
