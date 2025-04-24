import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router'

import { Artist } from '../models/artist'
import { ArtistService } from '../services/artist.service';

@Component({
  selector: 'app-artist-details',
  templateUrl: './artist-details.component.html',
  styleUrl: './artist-details.component.css',
  standalone: true, 
  imports: [CommonModule]
})
export class ArtistDetailsComponent implements OnInit {

  artist$?: Observable<Artist | undefined>;
  id: number;

  constructor(
    private artistService: ArtistService,
    private route: ActivatedRoute
  ) {
    this.artist$ = artistService.artistInfo$;
    this.id = +this.route.snapshot.paramMap.get('id')!
  }

  ngOnInit(): void {
    this.artistService.getArtistById(
      +this.route.snapshot.paramMap.get('id')!)
    console.log(this.artist$);
  }

}
