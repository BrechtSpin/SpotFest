import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '@env/environment';
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
    const myvar = this.http.get<ArtistWithMetrics>(`${this.Url}/${guid}/${name}`)
    return myvar
  }

  getArtistByNameSpotify(searchterm: string): Observable<any> {
    return this.http.get(`${this.SearchUrl}/search`, { params: { name: searchterm } })
    //return this.http.get(`artist/search`, { params: { name: searchterm }})
  }

  handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}
