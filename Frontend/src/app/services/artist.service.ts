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

  //private artistInfoSubject = new BehaviorSubject<ArtistFull | undefined>(undefined);
  //public artistInfo$ = this.artistInfoSubject.asObservable();
  constructor(private http: HttpClient) { }

  getArtistWithMetricsByGuid(guid: string, name: string | null): Observable<ArtistWithMetrics> {
    /*if (!guid) { return of(null); }*/
    if (!name) { name = ''; }
    const myvar = this.http.get<ArtistWithMetrics>(`${this.Url}/${guid}/${name}`)
    const avar = 1 + 2;
    return myvar
  }

  //getArtistById(id: number): Observable<ArtistFull | undefined> {
  //  const url = `${this.apiUrl}/${id}`;
  //  return this.http.get<ArtistFull>(url)
  //    .pipe(catchError(this.handleError<ArtistFull | undefined>('getArtistInfo', undefined)));
  //}

  getArtistByNameSpotify(searchterm: string): Observable<any> {
    return this.http.get(`artist/search`, { params: { name: searchterm }})
  }

  handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}
