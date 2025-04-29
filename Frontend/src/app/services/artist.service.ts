import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '@env/environment';
import { Artist } from '@models/artist';

@Injectable({
  providedIn: 'root'
})
export class ArtistService {

  private apiUrl = `${environment.apiArtistUrl}`;

  private artistInfoSubject = new BehaviorSubject<Artist | undefined>(undefined);
  public artistInfo$ = this.artistInfoSubject.asObservable();
  constructor(private http: HttpClient) { }

  getArtistById(id: number): Observable<Artist | undefined> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Artist>(url)
      .pipe(catchError(this.handleError<Artist | undefined>('getArtistInfo', undefined)));
  }

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
