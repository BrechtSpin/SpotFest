import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { environment } from '@env/environment';
import { ArtistSummary } from '@models/artist-summary';
import { ArtistWithMetrics } from '@models/artist-with-metrics';

@Injectable({
  providedIn: 'root'
})
export class ArtistService {

  private Url = `${environment.apiArtistUrl}`;
  private SearchUrl = `${environment.apiArtistsearchUrl}`

  constructor(private http: HttpClient) { }

  getArtistWithMetricsByGuid(guid: string, name: string | null): Observable<ArtistWithMetrics> {
    /*if (!guid) { return of(null); }*/
    if (!name) { name = ''; }
    return this.http.get<ArtistWithMetrics>(`${this.Url}/${guid}/${name}`)
  }
  getArtistGuidByName(name: string): Observable<string> {
    return this.http.get<string>(`${this.Url}/s/${name}`)
  }

  getArtistByNameSpotify(searchterm: string): Observable<any> {
    return this.http.get(`${this.SearchUrl}/search`, { params: { name: searchterm } })
  }

  GetArtistsBySearch(query: string, first: number): Observable<ArtistSummary[]> {
    let params = new HttpParams()
      .set('query', query)
      .set('index', first.toString());
    return this.http.get<ArtistSummary[]>(`${this.Url}/s`, { params });
  }

  handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}
