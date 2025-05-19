import { Routes } from '@angular/router';
import { HomepageComponent } from '@components/Homepage/homepage.component';
import { ArtistDetailsComponent } from '@components/artist-details/artist-details.component';
import { HappeningCreateFormComponent } from '@components/happening-create-form/happening-create-form.component';
import { HappeningDetailComponent } from '@components/happening-detail/happening-detail.component';
import { ContactFormComponent } from '@components/contact-form/contact-form.component';
import { NotFound404Component } from '@components/404/404-not-found';

export const routes: Routes = [
  { path: '', component: HomepageComponent, pathMatch: 'full' },
  { path: 'artist/:guid', component: ArtistDetailsComponent },
  { path: 'artist/:guid/:name', component: ArtistDetailsComponent },
  { path: 'happening', component: HappeningCreateFormComponent },
  { path: 'happening/:slug', component: HappeningDetailComponent },
  { path: 'contact', component: ContactFormComponent },
  { path: '404', component: NotFound404Component },
  { path: '**', redirectTo: '' }  //falback
];
