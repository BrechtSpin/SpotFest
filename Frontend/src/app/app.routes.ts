import { Routes } from '@angular/router';
import { ArtistDetailsComponent } from '@components/artist-details/artist-details.component';
import { HappeningCreateFormComponent } from '@components/happening-create-form/happening-create-form.component';


export const routes: Routes = [
  { path: 'artist/:id', component: ArtistDetailsComponent },
  { path: 'happening', component: HappeningCreateFormComponent}
];
