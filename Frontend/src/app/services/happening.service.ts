import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@env/environment';
import { CreateHappening, UpdateHappeningWithArtists } from '@models/happening-modify';
import { HappeningFull } from '@models/happening-full';
import { HappeningSummary } from '@models/happening-summary';

@Injectable({
  providedIn: 'root'
})
export class HappeningService {
  private http = inject(HttpClient);

  private Url = `${environment.apiHappeningUrl}`;

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

  updateHappeningWithArtists(updateHappeningWithArtists: UpdateHappeningWithArtists) {
    return this.http.post(`${this.Url}/update`, updateHappeningWithArtists)
  }

  getHappeningsOfArtist(guid: string): Observable<HappeningSummary[]> {
    return this.http.get<HappeningSummary[]>(`${this.Url}/artist/${guid}`)
  }

  getHappeningsByYearMonth(year:number, month:number, index:number): Observable<HappeningSummary[]> {
    let params = new HttpParams()
      .set('year', year.toString())
      .set('month', month.toString())
      .set('index', index.toString());
    return this.http.get<HappeningSummary[]>(`${this.Url}/s`, { params });
  }
}
